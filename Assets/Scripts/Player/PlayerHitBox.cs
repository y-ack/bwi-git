using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    [SerializeField]private PlayerBehavior ParentPlayer = null;
    public void setParent(PlayerBehavior g) { ParentPlayer = g; }

    [SerializeField] private Vector2 offset;
    private SpriteRenderer sprite;
    public int sortingOrder = 0;

    void Start()
    {      
        transform.parent = ParentPlayer.transform;
        transform.localPosition = (Vector3)offset;
    }

    void Update()
    {
        //Debug.Log(sprite.sortingOrder);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "EnemyBullet":
                ParentPlayer.Hit();
                break;
        }
    }

    public void show()
    {
        sprite = GetComponent<SpriteRenderer>();
        sortingOrder = 1;
        sprite.sortingOrder = sortingOrder;
    }
    public void hide()
    {
        sprite = GetComponent<SpriteRenderer>();
        sortingOrder = -1;
        sprite.sortingOrder = sortingOrder;        
    }
}
