using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBulletPatternLinear : BubbleBulletPattern
{
    public override void FireAt(float angle)
    {
        var bullet = (BubbleBullet)Instantiate(bulletPrefab, this.transform, false);

        bullet.velocity = new Vector3(Mathf.Cos(angle),
                                      Mathf.Sin(angle),
                                      0) * velocityParameters[0];
        bullet.angularVelocity = angularVelocity;
        bullet.acceleration = acceleration;
        bullet.accelerationTimeout = accelerationTime;
    }
}
