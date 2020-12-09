using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializablePlayerProjectile 
{
    public SerializableVector bulletDirection; // vector2
    public SerializableVector bulletRotation; // Quaternion
    public float lifeSpan;
    public int rebounds;
    public bool disabled;
}
