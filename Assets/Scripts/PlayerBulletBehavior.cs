using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehavior : MonoBehaviour
{
    static private PlayerBehavior ParentPlayer = null;
    static public void setParent(PlayerBehavior g) { ParentPlayer = g; }

    private const float bulletSpeed = 10f;
    private float lifeSpan;
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

    private void OnTriggerEnter2D(Collider2D collision){        
        if (collision.gameObject.tag == "RedBubble" || collision.gameObject.tag == "BlueBubble" || collision.gameObject.tag == "YellowBubble") 
        {        
            Debug.Log("Logging");
            Destroy(collision.gameObject);
            GameManager.theManager.bubbleCleared();
            destroySelf();
        }
        if(collision.gameObject.tag != "Player")
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
