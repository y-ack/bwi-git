using System.Collections;
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

    private void OnCollisionEnter2D(Collision2D collision){        
        if (collision.gameObject.name != "Player") 
        {        
            GameObject m = collision.gameObject;
            destroySelf();
            if(collision.gameObject.tag == "RedBubble")
            {
                ParentPlayer.SetCapture(0, m);
            }
            if(collision.gameObject.tag == "BlueBubble")
            {
                ParentPlayer.SetCapture(1, m);
            }
            if(collision.gameObject.tag == "YellowBubble")
            {
                ParentPlayer.SetCapture(2, m);
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
