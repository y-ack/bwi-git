using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBeam : MonoBehaviour
{
    public const float bulletSpeed = 15f;
    void FixedUpdate()
    {
        transform.localPosition += transform.up * (bulletSpeed * Time.fixedDeltaTime);
    }

    private void destroySelf()
    {
        Destroy(transform.gameObject);  // kills self
    }
}
