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
        purple = 3,
        rainbow = 4,
        garbage = 5;
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
    public Texture2D purpleTexture = null;
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

    [SerializeField] private float orbit_x = 0.9f;
    [SerializeField] private float orbit_y = 0.4f;
    void Update()
    {
        // searched = false; // ... this is bad.
        
        // only states with frame behavior are capture and launch,
        // where they have projectile-like behavior
        float delta = bubbleSpeed * Time.smoothDeltaTime;
        switch (state)
        {
            case State.CAPTURED:
                float a = transform.parent.GetComponent<PlayerBehavior>().angle;
                var lookDir = Vector3.RotateTowards(transform.parent.up, -transform.parent.right,
                                                    a * Mathf.Deg2Rad, 1.0f);
                lookDir.y *= orbit_y; lookDir.x *= orbit_x;
                Debug.Log(transform.localPosition);
                if (Vector3.Distance(transform.localPosition, Vector3.zero) <= 1.5f)
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                                                                  lookDir,
                                                                  delta);
                else
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                                                                  Vector3.zero,
                                                                  delta);
                break;
            case State.LAUNCHED:
                // travel in launchDirection until a collision happens
                transform.position += launchDirection * delta; //delta bugged
                break;
        }        
        // TODO[RETRO] add gleam animation for bubble spirits
    }
        void OnTriggerEnter2D(Collider2D other)
    {
        switch (state)
        {
            case State.NORMAL:
                if(other.gameObject.tag == "Bullet"
                   && !other.GetComponent<PlayerBulletBehavior>().disabled)
                {
                    other.GetComponent<PlayerBulletBehavior>().disabled = true;
                    Clear();
                }
                if(other.gameObject.tag == "Capture"
                   && !other.GetComponent<CaptureBulletBehavior>().disabled)
                {
                    other.GetComponent<CaptureBulletBehavior>().disabled = true;
                    Captured();
                }
                break;

            case State.LAUNCHED:
                // hits a bubble: stick, get parent, add, and call trymatch
                if (other.gameObject.tag == "BubbleSpirit")
                {
                    AdoptedBy(other.GetComponent<BubbleSpirit>().parentUnit);
                    state = State.NORMAL;
                    tryMatch();
                } else if (other.gameObject.tag == "Wall Top")
                {
                    Clear();
                }
                //TODO if hits a wall, reparent into new unit instead? discuss
                break;
        }
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
            //sr.material.mainTexture = redTexture;
        if (color == BubbleColor.blue)
            mySprite = Sprite.Create(blueTexture, new Rect(0.0f, 0.0f, blueTexture.width, blueTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            //sr.material.mainTexture = blueTexture;
        if (color == BubbleColor.yellow)
            mySprite = Sprite.Create(yellowTexture, new Rect(0.0f, 0.0f, yellowTexture.width, yellowTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
           //sr.material.mainTexture = yellowTexture;
        if (color == BubbleColor.purple)
            mySprite = Sprite.Create(purpleTexture, new Rect(0.0f, 0.0f, purpleTexture.width, purpleTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
           //sr.material.mainTexture = purpleTexture;

        
        sr.sprite = mySprite;
    }

    public void setLaunch()
    {
        state = State.LAUNCHED;
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
                 > 2.0f) {
            Debug.Log("not close enough to player for launch! wait on anim");
            return false;
            } else
        {
            state = State.LAUNCHED;
            launchDirection = direction;
            Unparent();
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

    private void Unparent() 
    {
        if (parentUnit) parentUnit.removeBubble(this);
        transform.parent = null;
        parentUnit = null;
    }
    //        (-1, 1) ( 0, 1)
    //    (-1, 0) ( 0, 0) ( 1, 0)
    //        (-1,-1) ( 0,-1)
    public void setParent(BubbleUnit newParentUnit, Vector2Int cell)
    {
        gridPosition = cell;
        // move to separate updateparent?
        transform.parent = newParentUnit.transform;
        parentGrid = transform.parent.GetComponent<GridLayout>();
        parentUnit = newParentUnit;
        parentUnit.addBubble(this);
        UpdateGridPosition();
    }

    private void Captured()
    {
        state = State.CAPTURED;
        Unparent();
        transform.parent = GameObject.FindWithTag("Player").transform;
    }
        
    private void tryMatch()
    {
        BubbleNeighbors bn = parentUnit.getNeighbors(this);
        Debug.Log(string.Join(",",bn.List()));
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
        }
        if (hasMatch)
        {
            ChainClear();
        }
        
    }
    
    public void Clear()
    {
        cleared = true;
        state = State.CLEARED;
        RunStatistics.Instance.bubblesCleared++;
        GameManager.theManager.bubbleCleared();
        Unparent();
        //trigger animation, yield and delete

        Destroy(gameObject, 0.2f);
    }

    public void ChainClear()
    {
        if (cleared)
        { 
            return;
        }
        
        RunStatistics.Instance.bubblesChainCleared[color]++;
        cleared = true;

        List<BubbleSpirit> bn_list = parentUnit.getNeighbors(this).List();
        foreach (BubbleSpirit bn in bn_list)
        {
            if (bn != null &&
                BubbleColor.match(color, bn.color))
            {
                Debug.Log(bn.color);
                bn.ChainClear();
            }
        }
        Clear();
    }
}
