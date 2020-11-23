using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Mathematics;

public abstract class BubbleBulletPattern : MonoBehaviour
{
    public BubbleBullet bulletPrefab;
    public double[] velocityParameters;

    public int step = 0;
    public int bulletCount = 0;
    public float baseAngle = 0f;
    public float angleVariation = 0f;
    public float angleDelta;
    public float angularVelocity = 0f;
    public float acceleration = 0f;
    public float accelerationTime = 0f;
    public float delayTime = 0f;
    public float patternCooldown = 2f;
    public float patternAngleDeltaAfter = 0f;

    public void Init(double[] velocityParameters,
                     int bulletCount = 0,
                     float baseAngle = 0f,
                     float angleVariation = 0f,
                     float angleDelta = float.NaN,
                     float angularVelocity = 0f,
                     float acceleration = 0f,
                     float accelerationTime = 0f,
                     float delayTime = 0f,
                     float patternCooldown = 2f,
                     float patternAngleDeltaAfter = 0f)
    {
        this.velocityParameters = velocityParameters;
        if (bulletCount > 0 && float.IsNaN(angleDelta))
        {
            this.bulletCount = bulletCount;
            this.angleDelta = (2.0f * Mathf.PI) / bulletCount;
        } else if (bulletCount == 0 && !float.IsNaN(angleDelta))
        {
            this.bulletCount = Mathf.RoundToInt((2.0f * Mathf.PI) / angleDelta);
            this.angleDelta = angleDelta;
        } else
        {
            this.bulletCount = bulletCount;
            this.angleDelta = angleDelta;
        }
        this.angleVariation = angleVariation;
        this.baseAngle = baseAngle;
        this.angularVelocity = angularVelocity;
        this.acceleration = acceleration;
        this.accelerationTime = accelerationTime;
        this.delayTime = delayTime;
        this.patternCooldown = patternCooldown;
        this.patternAngleDeltaAfter = patternAngleDeltaAfter;
    }

    private float lifetime;
    private float patternLifetime = 0.0001f;

    private PlayerBehavior playerTarget;
    public BubbleSpirit parentBubble;
    public void Start()
    {
        playerTarget =
            (PlayerBehavior)FindObjectOfType(typeof(PlayerBehavior));
        if (!playerTarget) { Debug.Log("Bullet system cannot find player"); }
    }

    const float activationRadius = 15f;
    public void Update()
    {
        if(GameManager.theManager.canMove// &&
           //parentBubble.state == BubbleSpirit.State.NORMAL
        )
        {
            patternLifetime -= Time.deltaTime;
            var playerDist = Vector3.Distance(transform.position, playerTarget.transform.position);
            if (((lifetime <= 0 && delayTime != 0) || playerDist < activationRadius)
                && patternLifetime <= 0)
            {
                lifetime -= Time.deltaTime;
                // while there are more bullets in the cycle and it's time
                while (step < bulletCount && lifetime <= 0f)
                {
                    float angle = Mathf.Atan2(
                        playerTarget.transform.position.y - transform.position.y,
                        playerTarget.transform.position.x - transform.position.x
                    ) + baseAngle + angleDelta * step +
                        Random.Range(-angleVariation,angleVariation);
                    FireAt(angle);
                    
                    ++step;
                    lifetime += delayTime;
                }
                // no more bullets or delay is 0; prime next pattern use
                if (lifetime <= 0f)
                {
                    step = 0;
                    patternLifetime = patternCooldown;
                    baseAngle += patternAngleDeltaAfter;
                } //else {
            }
        }
    }
    public void setPatternParameters(double[] v)
    {
        velocityParameters = v;
    }

    abstract public void FireAt(float angle);
    
    // public void FireTowards(Transform target)
    // {
    //     FireAt(Vector3.Angle(transform.position, target.position));
    // }
}
