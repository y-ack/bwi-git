using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    public Vector3 velocity;
    public float angularVelocity;
    public float acceleration;
    public float accelerationTimeout;

    // Update is called once per frame
    void Update()
    {
            if (accelerationTimeout > 0f)
            {
                velocity += velocity.normalized * acceleration *
                    Mathf.Min(Time.deltaTime, accelerationTimeout);
                accelerationTimeout -= Time.deltaTime;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                          Quaternion.Euler(0, 0, 90),
                                                          angularVelocity *
                                                          Time.smoothDeltaTime);
            transform.position += velocity * Time.smoothDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerHitbox":
                Debug.Log("Hit Player");
                destroyYoSelf();
                break;
                //case "Wall":
            case "Wall Top":
                Debug.Log("Colliding Wall");
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
