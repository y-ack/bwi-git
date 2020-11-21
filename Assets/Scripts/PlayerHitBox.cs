using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    static private PlayerBehavior ParentPlayer = null;
    static public void setParent(PlayerBehavior g) { ParentPlayer = g; }

    public int sortingOrder = 0;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ParentPlayer.transform.position;
        sprite.sortingOrder = sortingOrder;
        Debug.Log(sprite.sortingOrder);
    }
    public void show()
    {
        Debug.Log("Showing");
        sortingOrder = 1;
    }
    public void hide()
    {
        Debug.Log("Hiding");
        sortingOrder = 0;
    }
}
