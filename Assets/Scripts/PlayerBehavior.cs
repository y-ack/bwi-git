using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody2D rbody;
    private float moveSpeed;
    Vector2 mousePos;

    Vector2 movementVector;

    public Camera cam;

    private Vector3 moveDir;

    private bool canMove;
    private bool isDashButtonDown;
    private float DashAmount = 5f;
    private float dashCoolDown = 5f;
    private float dashAfterSec = 0;

    // Start is called before the first frame update
    private void Awake() {
        rbody = GetComponent<Rigidbody2D>();
        canMove = false;
    }

    void Start()
    {
        cam = Camera.main;

        rbody.gravityScale = 0;
        //set to 10 for testing, should discuss this later on.
        moveSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            movementVector = new Vector2(Input.GetAxis("Horizontal"),
                                     Input.GetAxis("Vertical"));

            rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.deltaTime);
            rbody.angularVelocity = 0f;
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            moveDir = new Vector3(Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")).normalized;


            if (Input.GetKeyDown(KeyCode.F) && (dashAfterSec <= 0))
            {
                isDashButtonDown = true;
            }
            if (dashAfterSec > 0)
            {
                dashAfterSec -= Time.deltaTime;
            }
            if (Input.GetMouseButtonDown(0))
            {
                GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as
                                       GameObject);
                e.transform.localPosition = transform.localPosition;
                e.transform.localRotation = transform.localRotation;
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameObject e = Instantiate(Resources.Load("Prefabs/net") as
                                       GameObject);
                e.transform.localPosition = transform.localPosition;
                e.transform.localRotation = transform.localRotation;
            }
        }   
    }

    public void SetCapture(){
        GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as
                                   GameObject);
            e.transform.localPosition = transform.localPosition;
            e.transform.localRotation = transform.localRotation;
    }

    void FixedUpdate()
    {
        if(canMove == true)
        {
            //rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.fixedDeltaTime);
            rbody.velocity = moveDir * moveSpeed;
            Vector2 lookDir = mousePos - rbody.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rbody.rotation = angle;

            if (isDashButtonDown == true)
            {
                Dashing();

            }
        }
    }

    public void Dashing()
    {
        rbody.MovePosition(transform.position + moveDir * DashAmount);
        isDashButtonDown = false;
        dashAfterSec = dashCoolDown;
    }

    public void setMove(bool iMove)
    {
        canMove = iMove;
    }




}
