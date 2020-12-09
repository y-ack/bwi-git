using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableBubbleSpirit 
{

    public SerializableVector bubblePosition; // vector3
    public SerializableVector gridPosition; // vector2int
    public string spritePath;
    public State state;

    public enum State
    {
        NORMAL = 0,
        CAPTURED,
        LAUNCHED,
        CLEARED
    }

    public int color;
    public int rebounds = 1;
    public SerializableVector launchDirection; // vector3
    public bool searched; // for parent path searching ... not impl yet.

    public double[] patternParameter; //maybe an array
    public bool cleared;
    public bool isChain;
}
