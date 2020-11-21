using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct BubbleNeighbors
{
    public BubbleSpirit NW;
    public BubbleSpirit NE;
    public BubbleSpirit E;
    public BubbleSpirit SE;
    public BubbleSpirit SW;
    public BubbleSpirit W;

    public List<BubbleSpirit> List()
    {
        return new List<BubbleSpirit> { NW, NE, E, SE, SW, W };
    }
}

public class BubbleUnit : MonoBehaviour
{
    public Vector2Int[][] oddr_directions = new Vector2Int[][]
    {
        new Vector2Int[] {
            new Vector2Int(+1,  0), new Vector2Int(0, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1,  0), new Vector2Int(-1, +1), new Vector2Int( 0, +1)
        },
        new Vector2Int[] {
            new Vector2Int(+1,  0), new Vector2Int(+1, -1), new Vector2Int( 0, -1),
            new Vector2Int(-1,  0), new Vector2Int( 0, +1), new Vector2Int(+1, +1),
        }
    };
    private float moveTimer;
    private Dictionary<Vector2Int, BubbleSpirit> grid = new
        Dictionary<Vector2Int, BubbleSpirit>();


    public BubbleSpirit cellOrNull(Vector2Int cellPos)
    {
        return grid.ContainsKey(cellPos) ? grid[cellPos] : null;
    }

    public BubbleNeighbors getNeighbors(BubbleSpirit b)
    {
        // Debug.Log("BubbleUnit's grid:");
        // foreach (KeyValuePair<Vector2Int, BubbleSpirit> kvp in grid)
        // {
        //     Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value.color));
        // }
        var pos = b.gridPosition;
        var parity = pos.y & 1;
        return new BubbleNeighbors()
        {
            NW = cellOrNull(pos + oddr_directions[parity][2]),
                NE = cellOrNull(pos + oddr_directions[parity][1]),
                E = cellOrNull(pos + oddr_directions[parity][0]),
                SE = cellOrNull(pos + oddr_directions[parity][5]),
                SW = cellOrNull(pos + oddr_directions[parity][4]),
                W = cellOrNull(pos + oddr_directions[parity][3])
                };
    }

    public void addBubble(BubbleSpirit b)
    {
        //there's a curious behavior here: if a bubble believes it
        //belongs somewhere where another already exists, won't it overwrite?
        // i don't know if this is possible in practice, yet, but i want to
        // add a safeguard just in case. possible that player could
        // release a bubble ON TOP OF existing ones, for example, and then
        // the grid would resolve to the same position...
        grid.Add(b.gridPosition, b);
    }

    /*
    private bool pathExists(BubbleSpirit start, BubbleSpirit end)
    {
        //depth first search, wow this sucks
        start.searched = true;
        var s_neighbors = getNeighbors(start);
        if (grid.TryGetValue(s_neighbors.NW))
        {
            
        }
    }
    */
    public void removeBubble(BubbleSpirit b)
    {
        //just leave disjoint for now ...
        //TODO[ALPHA] do pathExists to check need to cut the unit
        grid.Remove(b.gridPosition);
    }

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // movement code
    }
}
