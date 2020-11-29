﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    public Vector3 velocity;
    public float angularVelocity;
    public float acceleration;
    public float accelerationTimeout;
    private void Awake() 
    {
        FindObjectOfType<AudioManager>().Play("Bubble_Shoot"); 
    }
    // Update is called once per frame
    void FixedUpdate()
    {
            if (accelerationTimeout > 0f)
            {
                velocity += velocity.normalized * acceleration *
                    Mathf.Min(Time.fixedDeltaTime, accelerationTimeout);
                accelerationTimeout -= Time.fixedDeltaTime;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                          Quaternion.Euler(0, 0, 90),
                                                          angularVelocity *
                                                          Time.fixedDeltaTime);
            transform.position += velocity * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D other = collision.collider;
        switch (other.gameObject.tag)
        {
            case "PlayerHitbox":
                destroyYoSelf();
                break;
                //case "Wall":
            case "Wall Top":
                destroyYoSelf();
                break;
            case "PlayerGraze":
                RunStatistics.Instance.grazeTime += Time.deltaTime;
                // TODO[BETA] graze sound
                break;
            default:
                break;
        }
    }
    private void destroyYoSelf()
    {
        Destroy(transform.gameObject);
    }
}
