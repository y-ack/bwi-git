using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedBubblesBehavior : MonoBehaviour
{
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
        }
    }
    private void destroySelf()
    {
        //ParentPlayer.eggDestroyed(this);
        //destroyed = true;
        Destroy(transform.gameObject);  // kills self
    }
}
