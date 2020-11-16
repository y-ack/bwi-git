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
            velocity += new Vector3(acceleration, acceleration, 0f) *
                Mathf.Min(Time.deltaTime, accelerationTimeout);
            accelerationTimeout -= Time.deltaTime;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                      Quaternion.Euler(0,0,90),
                                                      angularVelocity *
                                                      Time.smoothDeltaTime);
        transform.position += velocity * Time.smoothDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
            //reset game, lose game
            //player hits bullet and dies
            //stats
                break;
            case "Wall":
            case "Wall Top":
                Debug.Log("Colliind Wall");
                destroyYoSelf();
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
