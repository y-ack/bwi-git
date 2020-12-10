using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplasherCircle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "BubbleSpirit")
        {
            Destroy(transform.gameObject);
        }
    }
}
