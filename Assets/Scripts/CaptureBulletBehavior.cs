﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBulletBehavior : MonoBehaviour
{
    static private PlayerBehavior ParentPlayer = null;
    static public void setParent(PlayerBehavior g) { ParentPlayer = g; }

    private const float bulletSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.up * (bulletSpeed * Time.smoothDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision){        
        if (collision.gameObject.name != "Player") 
        {        
            destroySelf();
            if(collision.gameObject.tag == "RedBubble")
            {
                ParentPlayer.SetCapture(0);
            }
            if(collision.gameObject.tag == "BlueBubble")
            {
                ParentPlayer.SetCapture(1);
            }
            if(collision.gameObject.tag == "YellowBubble")
            {
                ParentPlayer.SetCapture(2);
            }
        }
    }
    private void destroySelf()
    {
        //ParentPlayer.eggDestroyed(this);
        //destroyed = true;
        Destroy(transform.gameObject);  // kills self
    }
}