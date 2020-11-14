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

    private bool isDashButtonDown;
    private float DashAmount = 5f;
    private float dashCoolDown = 5f;
    private float dashAfterSec = 0;
    private float captureCoolDown = 8f;
    private float captureAfterSec = 0;
    private bool capturedBubble = false;

    public enum CaptureState
    {
        RED,
        BLUE,
        YELLOW
    };
    private CaptureState currentState;

    // Start is called before the first frame update
    private void Awake() {
        rbody = GetComponent<Rigidbody2D>();
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
        if (GameManager.theManager.canMove == true)
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
            countdownCooldown();
            if (Input.GetMouseButtonDown(0))
            {
                GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as
                                       GameObject);
                e.transform.localPosition = transform.localPosition;
                e.transform.localRotation = transform.localRotation;
            }
            if (Input.GetMouseButtonDown(1))
            {
                if(captureAfterSec <= 0)
                {
                    GameObject e = Instantiate(Resources.Load("Prefabs/net") as
                                       GameObject);
                    e.transform.localPosition = transform.localPosition;
                    e.transform.localRotation = transform.localRotation;
                    captureAfterSec = captureCoolDown;
                }
                if(capturedBubble == true)
                {
                    spawnCapturedBubble();
                    capturedBubble = false;
                }
            }
        }   
    }

    void FixedUpdate()
    {
        if(GameManager.theManager.canMove == true)
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

    public void SetCapture(int inputState)
    {
        capturedBubble = true;
        switch(inputState)
        {
            case 0:
                currentState = CaptureState.RED;
                break;
            case 1:
                currentState = CaptureState.BLUE;
                break;
            case 2:
                currentState = CaptureState.YELLOW;
                break;
        }
    }


    public void spawnCapturedBubble()
    {
        GameObject f;
        switch(currentState)
        {
            case CaptureState.RED:
                f = Instantiate(Resources.Load("Prefabs/CapturedBubbles/CapturedRedBubble") as
                    GameObject);
                f.transform.localPosition = transform.localPosition;
                f.transform.localRotation = transform.localRotation;
                break;
            case CaptureState.BLUE:
                f = Instantiate(Resources.Load("Prefabs/CapturedBubbles/CapturedBlueBubble") as
                    GameObject);
                f.transform.localPosition = transform.localPosition;
                f.transform.localRotation = transform.localRotation;
                    break;
            case CaptureState.YELLOW:
                f = Instantiate(Resources.Load("Prefabs/CapturedBubbles/CapturedYellowBubble") as
                    GameObject);
                f.transform.localPosition = transform.localPosition;
                f.transform.localRotation = transform.localRotation;
                break;
        }
    }

    public void Dashing()
    {
        rbody.MovePosition(transform.position + moveDir * DashAmount);
        isDashButtonDown = false;
        dashAfterSec = dashCoolDown;
    }

    public void countdownCooldown()
    {
        if (dashAfterSec > 0)
        {
            dashAfterSec -= Time.deltaTime;
        }
        if (captureAfterSec > 0)
        {
            captureAfterSec -= Time.deltaTime;
        }
    }


}
