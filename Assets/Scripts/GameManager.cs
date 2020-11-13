using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerBehavior mPlayer = null;
    // Start is called before the first frame update
    void Start()
    {
        PlayerBulletBehavior.setParent(mPlayer);
        CaptureBulletBehavior.setParent(mPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
