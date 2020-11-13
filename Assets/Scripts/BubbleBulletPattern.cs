using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BubbleBulletPattern : MonoBehaviour
{
    protected GameObject bulletPrefab;
    protected float[] velocityParameters;

    public float step = 0f;
    public float baseAngle = 0f;
    public float angleDelta;
    public float angularVelocity = 0f;
    public float acceleration = 0f;
    public float delayTime = 0f;
    public float patternCooldown = 2f;

    public void setPatternParameters(float[] v)
    {
        velocityParameters = v;
    }
    abstract public void FireAt(float angle);
    public void FireTowards(Component target)
    {
        FireAt(Vector3.Angle(transform.position, target.transform.position));
    }
    public void Fire()
    {
        FireAt(0.0f);
    }
}
