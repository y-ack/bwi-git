using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pseudo enum
[Serializable]
public struct BubbleColor
{
    public static const Texture2D redTexture;
    public static const Texture2D blueTexture;
    public static const Texture2D yellowTexture;
    public static const Texture2D garbageTexture;

    public const int red = 0,
        blue = 1,
        yellow = 2,
        rainbow = 3,
        garbage = 4;
    public const int count = garbage + 1;
    // match function that ignores 'garbage' block color
    // and matches any color against 'rainbow'
    public static bool match(int a, int b)
    {
        return (a != garbage || b != garbage)
            && (a == b
                || (a == rainbow || b == rainbow));
    }

    public static Texture2D getTexture(int color)
    {
        switch (color)
        {
            case BubbleColor.red:
                return redTexture;
            case BubbleColor.blue:
                return blueTexture;
            case BubbleColor.yellow:
                return yellowTexture;
            default:
                return garbageTexture; //well, no, but to help it compile
        }
    }
}


public class BubbleSpirit : MonoBehaviour
{
    public enum State {    
        NORMAL = 0,
        CAPTURED,
        LAUNCHED,
        CLEARED
    }
    
    public GameObject testingInitialParent; //don't use this for production spawning
    public BubbleUnit parentUnit;
    private GridLayout parentGrid; // this one is ok to keep, since it's just cached
    public Vector2Int gridPosition;

    public State state; // could probably be private
    public int color;
    private Vector3 launchDirection;
    public bool searched; // for parent path searching ... not impl yet.

    public BubbleBulletPattern pattern; //maybe an array
    bool cleared;

    void Start()
    {    }

    void Update()
    {
        // searched = false; // ... this is bad.

        // only states with frame behavior are capture and launch,
        // where they have projectile-like behavior
        switch (state)
        {
            case State.CAPTURED:
                float speed = 5.0f;
                transform.position = Vector3.MoveTowards(transform.position,
                                                         transform.parent.position,
                                                         speed);
                break;
            case State.LAUNCHED:
                // travel in launchDirection until a collision happens
                transform.position += launchDirection * Time.deltaTime;
                break;
        }
        // TODO[RETRO] add gleam animation for bubble spirits
    }

    // get colors from BubbleColor.red/blue/etc
    public void SetColor(int bubbleColor)
    {
        if (bubbleColor >= BubbleColor.count)
        {
            Debug.LogError("bad bubble color!! got: " + bubbleColor, this);
        }
        color = bubbleColor;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.material.mainTexture = BubbleColor.getTexture(bubbleColor);
    }

    public bool tryLaunch(Vector3 direction)
    {
        if (state != State.CAPTURED)
        {
            Debug.LogWarning("tried to launch a spirit that isn't captured");
            return false;
        } else if
                (Vector3.Distance(transform.position,
                                  transform.parent.position)
                 < 2.0f) {
            Debug.Log("not close enough to player for launch! wait on anim");
            return false;
        } else
        {
            state = State.LAUNCHED;
            launchDirection = direction;
            return true;
        }
    }

    // called once grid position is determined.
    private void UpdateGridPosition()
    {
        transform.localPosition = parentGrid.CellToLocal((Vector3Int)gridPosition);
    }

    // used when an independent bubble collides and is added to a bubbleunit
    // e.g. before capture chaining to determine the closest grid placement
    // TODO[BETA]: lerp to new localposition for smoother snap?
    private void SnapToGrid()
    {
        gridPosition = (Vector2Int)parentGrid.WorldToCell(transform.position);
        UpdateGridPosition();
    }

    private void AdoptedBy(BubbleUnit newParentUnit)
    {
        /* for combining/launching captured bubbles to set new parent */
        transform.parent = newParentUnit.transform;

        // should this be in start or moved to a separate UpdateParent()?
        parentGrid = transform.parent.GetComponent<GridLayout>();
        parentUnit = newParentUnit;
        SnapToGrid();
        newParentUnit.addBubble(this);
    }
    
    //        (-1, 1) ( 0, 1)
    //    (-1, 0) ( 0, 0) ( 1, 0)
    //        (-1,-1) ( 0,-1)
    public void setParent(BubbleUnit newParentUnit, Vector2Int cell)
    {
        gridPosition = cell;
        AdoptedBy(newParentUnit);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (state)
        {
            case State.NORMAL:
                if (other.gameObject.tag == "Bullet")
                {
                    Clear();
                }
                // handled by parent unit?: also maybe TODO[ALPHA]?
                //b) wall: cannot pass through, bounces off
                //c) flora: bounces off rocks, passes through bushes
                //d) others: ignored
                break;

            case State.LAUNCHED:
                // hits a bubble: stick, get parent, add, and call trymatch
                if (other.gameObject.tag == "BubbleSpirit")
                {
                    AdoptedBy(other.GetComponent<BubbleSpirit>().parentUnit);
                    tryMatch();
                }
                break;
        }
    }

    private void tryMatch()
    {
        BubbleNeighbors bn = parentUnit.getNeighbors(this);
        bool hasMatch = false;
        //it's possible that the one we collided with is not the match,
        //but there is a match after snapping, so check all
        foreach (BubbleSpirit neighbor in bn.List())
        {
            if (neighbor != null &&
                BubbleColor.match(neighbor.color, this.color))
            {
                hasMatch = true;
                break;
            }
            if (hasMatch)
            {
                ChainClear();
            }
        }
    }

    public void Clear()
    {
        cleared = true;
        RunStatistics.Instance.bubblesCleared++;
        parentUnit.removeBubble(this);
        transform.parent = null;
        parentUnit = null;
        //trigger animation, yield and delete
        
        Destroy(this, 1.0f);
    }

    public void ChainClear()
    {
        if (cleared)
        { 
            return;
        }
        
        Clear();
        RunStatistics.Instance.bubblesChainCleared[color]++;

        List<BubbleSpirit> bn_list = parentUnit.getNeighbors(this).List();
        foreach (BubbleSpirit bn in bn_list)
        {
            if (bn != null &&
                BubbleColor.match(this.color, bn.color))
            {
                bn.ChainClear();
            }
        }
    }
}
