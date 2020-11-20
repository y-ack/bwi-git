using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pseudo enum
[Serializable]
public struct BubbleColor
{
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
}

public class BubbleSpirit : MonoBehaviour
{
    public Texture2D redTexture = null;
    public Texture2D blueTexture = null;
    public Texture2D yellowTexture = null;
    private Sprite mySprite;
    //public Texture2D[] colorTextures = new Texture2D[]{redTexture, blueTexture, yellowTexture};

    public enum State {
        NORMAL = 0,
        CAPTURED,
        LAUNCHED,
        CLEARED
    }
    
    public BubbleUnit parentUnit;
    private GridLayout parentGrid; // this one is ok to keep, since it's just cached
    public Vector2Int gridPosition;

    public State state; // could probably be private
    public int color;
    private float bubbleSpeed = 15f;
    private Vector3 launchDirection;
    public bool searched; // for parent path searching ... not impl yet.

    public BubbleBulletPattern pattern; //maybe an array
    bool cleared;

    void Start()
    { state = State.NORMAL; }

    void Update()
    {
        switch(state)
        {
            case State.LAUNCHED:
            transform.localPosition += transform.up * (bubbleSpeed * Time.smoothDeltaTime);
            break;
        }
        // searched = false; // ... this is bad.
        /*
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
        */
        // TODO[RETRO] add gleam animation for bubble spirits
    }
    public void SetLaunched()
    {
        state = State.LAUNCHED;
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
        // this sucks and unity is not making it easy on me
        // swapping out materials didnt work as easily as hoped
        // maybe need to be looked into more
        // alternate method: create sprite and swap out sprites completely
        if (color == BubbleColor.red)
            mySprite = Sprite.Create(redTexture, new Rect(0.0f, 0.0f, redTexture.width, redTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sr.sprite = mySprite;
            //sr.material.mainTexture = redTexture;
        if (color == BubbleColor.blue)
            mySprite = Sprite.Create(blueTexture, new Rect(0.0f, 0.0f, blueTexture.width, blueTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sr.sprite = mySprite;
            //sr.material.mainTexture = blueTexture;
        if (color == BubbleColor.yellow)
            mySprite = Sprite.Create(yellowTexture, new Rect(0.0f, 0.0f, yellowTexture.width, yellowTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sr.sprite = mySprite;
            //sr.material.mainTexture = yellowTexture;
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
        parentUnit.addBubble(this);
    }
    
    //        (-1, 1) ( 0, 1)
    //    (-1, 0) ( 0, 0) ( 1, 0)
    //        (-1,-1) ( 0,-1)
    public void setParent(BubbleUnit newParentUnit, Vector2Int cell)
    {
        AdoptedBy(newParentUnit);
        gridPosition = cell;
        UpdateGridPosition();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Capture")
        {
            state = State.CAPTURED;
        }
    }
    
    /*
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
    */
    
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
