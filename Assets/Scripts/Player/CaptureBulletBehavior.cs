using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBulletBehavior : MonoBehaviour
{
    static private PlayerBehavior ParentPlayer = null;
    static public void setParent(PlayerBehavior g) { ParentPlayer = g; }

    public bool disabled = false;
    private const float bulletSpeed = 15f;
    void Update()
    {
        transform.localPosition += transform.up * (bulletSpeed * Time.smoothDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (disabled) return;
        switch (collision.gameObject.tag)
        {
            case "BubbleSpirit":
                if (collision.GetComponent<BubbleSpirit>().state == BubbleSpirit.State.NORMAL)
                {
                    BubbleSpirit capturedBubble = collision.GetComponent<BubbleSpirit>();
                    ParentPlayer.SetCapture(capturedBubble);
                    destroySelf();
                }
                break;
            case "Wall Top":
                disabled = true;
                destroySelf();
                break;
            default:
                return;
        }
    }

    private void destroySelf()
    {
        //ParentPlayer.eggDestroyed(this);
        //destroyed = true;
        Destroy(transform.gameObject);  // kills self
    }
}
