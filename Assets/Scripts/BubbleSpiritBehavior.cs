using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpiritBehavior : MonoBehaviour
{
    public enum BubbleColor
    {
        red = 0,
        blue,
        yellow,
        rainbow
    }
    
    public GameObject testingInitialParent; //don't use this for production spawning
    private GridLayout parentGrid; // this one is ok to keep, since it's just cached
    public BubbleColor color;
    public Vector3Int gridPosition;
    //BubbleBulletPattern; //maybe an array
    bool cleared; //set this when cleared/matched so that recursive flood fill works

    void Start()
    {
        transform.parent = testingInitialParent.transform;
        parentGrid = transform.parent.GetComponent<GridLayout>();
        SnapToGrid();
    }

    void Update()
    {
        
    }

    private void SnapToGrid()
    {
        transform.localPosition = parentGrid.CellToLocal(gridPosition);
    }
    private void AdoptedBy(/*GameObject newParentUnit*/)
    {
        /* for combining/launching captured bubbles to set a new parent bubbleunit*/
    }
}
