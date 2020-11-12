using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public GameManager theManager;
    Rigidbody2D rbody;
    private float moveSpeed;
    Vector2 mousePos;
    public Camera cam;
    Vector2 movementVector;
    private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        rbody = GetComponent<Rigidbody2D>();
        rbody.gravityScale = 0;
        moveSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove == true)
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
    }

    void FixedUpdate()
    {
        if(canMove == true)
        {
            rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.fixedDeltaTime);
            Vector2 lookDir = mousePos - rbody.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rbody.rotation = angle;
        }
    }

    public void moveControl(bool iCommand)
    {
        canMove = iCommand;
    }
}
