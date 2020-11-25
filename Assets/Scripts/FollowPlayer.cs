using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject follow;
    private void Start() {
        follow = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.transform.position;
    }
}
