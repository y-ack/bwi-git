using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public abstract class BubbleBulletPattern : MonoBehaviour
{
    public BubbleBullet bulletPrefab;
    public float[] velocityParameters;

    public int step = 0;
    public int bulletCount = 0;
    public float baseAngle = 0f;
    public float angleDelta;
    public float angularVelocity = 0f;
    public float acceleration = 0f;
    public float accelerationTime = 0f;
    public float delayTime = 0f;
    public float patternCooldown = 2f;
    public float patternAngleDeltaAfter = 0f;

    public float lifetime = 0;

    private PlayerBehavior playerTarget;
    public void Start()
    {
        playerTarget =
    (PlayerBehavior)FindObjectOfType(typeof(PlayerBehavior));
        if (!playerTarget) { Debug.Log("Bullet system cannot find player"); }
    }

    public void Update()
    {
        if(GameManager.theManager.canMove == true)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                while (step < bulletCount && lifetime <= 0f)
                {
                    float angle = Vector3.Angle(transform.position, playerTarget.transform.position)
                        + baseAngle + angleDelta * step;
                    FireAt(angle);

                    ++step;
                    lifetime += delayTime;
                }
                if (lifetime <= 0f)
                {
                    step = 0;
                    lifetime = patternCooldown;
                    baseAngle += patternAngleDeltaAfter;
                } //else {
            }
        }
    }
    public void setPatternParameters(float[] v)
    {
        velocityParameters = v;
    }

    abstract public void FireAt(float angle);
    
    // public void FireTowards(Transform target)
    // {
    //     FireAt(Vector3.Angle(transform.position, target.position));
    // }
    public void Fire()
    {
        FireAt(0.0f);
    }
    // public void FireTowardsPlayer()
    // {
    //     FireTowards(playerTarget.transform);
    // }
}
