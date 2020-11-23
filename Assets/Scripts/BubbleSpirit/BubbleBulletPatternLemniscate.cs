using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBulletPatternLemniscate : BubbleBulletPattern
{
    public override void FireAt(float angle)
    {

        var bullet = (BubbleBullet)Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;

        var v = velocityParameters;
        var l = Mathf.Sin((float)(v[3] + v[4] * angle));
        var speed = Mathf.Pow((float)(v[0] + v[1] * angle + v[2]
                                      * l), (float)v[5]);
        if (speed != speed) speed = 0.1f; // float underflow issue ...
        bullet.velocity = new Vector3(Mathf.Cos(angle),
                                      Mathf.Sin(angle),
                                       0) * speed;
        bullet.angularVelocity = angularVelocity;
        //could use vector for acceleration too ...
        bullet.acceleration = acceleration;
        bullet.accelerationTimeout = accelerationTime;
    }
}
