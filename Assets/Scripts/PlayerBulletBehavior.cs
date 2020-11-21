﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehavior : MonoBehaviour
{
    static private PlayerBehavior ParentPlayer = null;
    static public void setParent(PlayerBehavior g) { ParentPlayer = g; }

    private const float bulletSpeed = 15f;
    Vector2 bulletDirection;
    private float lifeSpan;

    public bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        lifeSpan = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.up * (bulletSpeed * Time.smoothDeltaTime);
        
        lifeSpan -= Time.deltaTime;
        if(lifeSpan <= 0 && Vector3.Distance(ParentPlayer.transform.position,transform.position) > 4f)
        {
            destroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (disabled) return;
        
        switch (c.gameObject.tag)
        {
            case "Wall Top":
                disabled = true;
                destroySelf();
                break;
            case "BubbleSpirit":
                //disabled by bs collision
                if (c.GetComponent<BubbleSpirit>().state == BubbleSpirit.State.NORMAL)
                    destroySelf();
                break;
            default:
                return;
        };
    }
    private void destroySelf()
    {
        //ParentPlayer.eggDestroyed(this);
        //destroyed = true;
        Destroy(transform.gameObject);  // kills self
    }
}
