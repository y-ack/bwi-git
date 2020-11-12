using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody2D rbody;
    private float moveSpeed;
    Vector2 mousePos;
    public Camera cam;
    Vector2 movementVector;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.gravityScale = 0;
        //set to 10 for testing, should discuss this later on.
        moveSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get user's current movement input     
        movementVector = new Vector2(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        //rbody.isKinematic = false;
        rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.deltaTime);
        //rbody.velocity = Vector3.zero;   
        rbody.angularVelocity = 0f;  
        //transform.localRotation = Quaternion.identity;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

    }
    void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rbody.rotation = angle;
    }
}
