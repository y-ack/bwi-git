using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pseudo enum
public struct BubbleColor
{

    public const int red = 0,
        blue = 1,
        yellow = 2,
        rainbow = 3,
        garbage = 4;
    public const int count = garbage;    
    // match function that ignores 'garbage' block color
    // and matches any color against 'rainbow'
    public static bool match(int a, int b)
    {
        return (a != 4 || b != 4)
            && (a == b
                || (a == 3 || b == 3));
    }
}


public class BubbleSpiritBehavior : MonoBehaviour
{

    public GameObject testingInitialParent; //don't use this for production spawning
    private GridLayout parentGrid; // this one is ok to keep, since it's just cached
    public int color;
    public Vector3Int gridPosition;
    //BubbleBulletPattern; //maybe an array
    bool cleared; //set this when cleared/matched so that recursive flood fill works

    void Start()
    {
        // only for testing, instead set transform.parent to transform of
        // whatever bubble unit it's being added to at spawn
        transform.parent = testingInitialParent.transform;

        // should this be in start or moved to a separate UpdateParent()?
        parentGrid = transform.parent.GetComponent<GridLayout>();
        UpdateGridPosition();

        ChainClear();
    }

    void Update()
    {
        // should probably stay empty? when will bubbles move without parent?
        // maybe when captured?
        // TODO[BETA] add gleam animation for bubble spirits
    }

    // called once grid position is determined.

    private void UpdateGridPosition()
    {
        transform.localPosition = parentGrid.CellToLocal(gridPosition);
    }

    // used when an independent bubble collides and is added to a bubbleunit
    // e.g. before capture chaining to determine the closest grid placement
    // TODO[BETA]: lerp to new localposition for smoother snap?
    private void SnapToGrid()
    {
        gridPosition = parentGrid.WorldToCell(transform.position);
        UpdateGridPosition();
    }

    private void AdoptedBy(/*GameObject newParentUnit*/)
    {
        /* for combining/launching captured bubbles to set a new parent bubbleunit*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //a) player bullet: set cleared flag, unset parent transform
        //                  trigger animation, delete/return to pool

        // handled by parent unit?: also maybe TODO[ALPHA]?
        //b) wall: cannot pass through, bounces off
        //c) flora: bounces off rocks, passes through bushes
        //d) others: ignored
    }

    void Clear()
    {
        cleared = true;
        RunStatistics.Instance.bubblesCleared++;
        transform.parent = null;
        //trigger animation, yield and delete
    }
    void ChainClear()
    {
        // recursive border case
        if (cleared)
        {
            return;
        }
        
        Clear();
        RunStatistics.Instance.bubblesChainCleared[color]++;
        // TODO[DEMO] check neighbors, call ChainClear if color matches
        //            currently don't have a way to check neighbors.
        //            store and query the parent BubbleUnit array?

    }
}
