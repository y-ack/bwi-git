﻿using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct BubbleNeighbors
{
    public Vector2Int origin;
    public List<BubbleSpirit> neighbors;
    public List<Vector2Int> offsets;

    public const int NW = 0;
    public const int NE = 1;
    public const int E  = 2;
    public const int SE = 3;
    public const int SW = 4;
    public const int W  = 5;
    
    public Dictionary<Vector2Int,BubbleSpirit> Dictionary()
    {
        var d = new Dictionary<Vector2Int, BubbleSpirit>();
        for (int i = 0; i < 6; ++i)
        {
            d.Add(offsets[i] + origin, neighbors[i]);
        }
        return d;
    }
}

public class BubbleUnit : MonoBehaviour
{
    //TODO change to List<> :(
    public Vector2Int[][] oddr_directions = new Vector2Int[][]
    {
        new Vector2Int[] {
            new Vector2Int(-1, -1), new Vector2Int( 0, -1), new Vector2Int(+1, 0),
            new Vector2Int( 0, +1), new Vector2Int(-1, +1), new Vector2Int(-1, 0)
        },
        new Vector2Int[] {
            new Vector2Int( 0, -1), new Vector2Int(+1, -1), new Vector2Int(+1, 0),
            new Vector2Int(+1, +1), new Vector2Int( 0, +1), new Vector2Int(-1, 0),
        }
    };
    private float moveTimer;
    private Dictionary<Vector2Int, BubbleSpirit> grid = new
        Dictionary<Vector2Int, BubbleSpirit>();
    private int bubbleCount;


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
        var neighbors = new BubbleNeighbors()
        {
            origin = pos,
            offsets = new List<Vector2Int>(oddr_directions[parity])
        };
        neighbors.neighbors = new List<BubbleSpirit>();
        for (int i = 0; i < 6; ++i)
        {
            neighbors.neighbors.Add(cellOrNull(neighbors.offsets[i] + pos));
        }
        return neighbors;
    }

    public Vector2Int nearestEmpty(BubbleSpirit b)
    {
        if (!grid.ContainsKey(b.gridPosition))
        {
            return b.gridPosition;
        }
        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        BubbleNeighbors n = getNeighbors(b);
        b.gridPosition = n.offsets[n.neighbors.FindIndex(x => x == null)];
        return b.gridPosition;
    }

    public void addBubble(BubbleSpirit b)
    {
        //there's a curious behavior here: if a bubble believes it
        //belongs somewhere where another already exists, won't it overwrite?
        // i don't know if this is possible in practice, yet, but i want to
        // (IT IS)
        // add a safeguard just in case. possible that player could
        // release a bubble ON TOP OF existing ones, for example, and then
        // the grid would resolve to the same position...
        nearestEmpty(b);
        grid.Add(b.gridPosition, b);
        ++bubbleCount;
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
        --bubbleCount;
        //if (--bubbleCount == 0) destroySelf();
    }

    private void destroySelf()
    {
        Destroy(gameObject);
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
