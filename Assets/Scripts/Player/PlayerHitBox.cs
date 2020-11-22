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
        sprite = GetComponent<SpriteRenderer>();
        transform.parent = ParentPlayer.transform;
        transform.localPosition = (Vector3)offset;
    }

    void Update()
    {
        //Debug.Log(sprite.sortingOrder);
    }

    public void show()
    {
        sortingOrder = 1;
        sprite.sortingOrder = sortingOrder;
    }
    public void hide()
    {
        sortingOrder = -1;
        sprite.sortingOrder = sortingOrder;        
    }
}
