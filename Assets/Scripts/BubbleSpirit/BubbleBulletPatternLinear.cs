using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBulletPatternLinear : BubbleBulletPattern
{
    public override void FireAt(float angle)
    {
        var bullet = (BubbleBullet)Instantiate(bulletPrefab);


        bullet.transform.position = transform.position;

        bullet.velocity = new Vector3(Mathf.Cos(angle),
                                      Mathf.Sin(angle),
                                      0) * (float)velocityParameters[0];
        bullet.angularVelocity = angularVelocity;
        //could use vector for acceleration too ...
        bullet.acceleration = acceleration;
        bullet.accelerationTimeout = accelerationTime;
    }
}
