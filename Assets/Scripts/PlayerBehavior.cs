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
    private float dashCoolDown = 1f;
    private float dashAfterSec = 0;
    private float captureCoolDown = 1f;
    private float captureAfterSec = 0;
    private bool isCapturing = false;

    private GameObject capturedBubble;

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
        movementVector = new Vector2(Input.GetAxis("Horizontal"),
                                     Input.GetAxis("Vertical"));

        rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.deltaTime);
        rbody.angularVelocity = 0f;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        moveDir = new Vector3(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")).normalized;

        if(GameManager.theManager.canMove == true)
        {
            buttonControl();
        }
    }

    void FixedUpdate()
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

    public void SetCapture(int inputState, GameObject inputBubble)
    {
        isCapturing = true;
        capturedBubble = inputBubble;
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

    private void buttonControl()
    {
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
            if (captureAfterSec <= 0)
            {
                GameObject e = Instantiate(Resources.Load("Prefabs/net") as
                                   GameObject);
                e.transform.localPosition = transform.localPosition;
                e.transform.localRotation = transform.localRotation;
                captureAfterSec = captureCoolDown;
            }
            if (isCapturing == true)
            {
                spawnCapturedBubble();
                isCapturing = false;
            }
        }
    }


    public void spawnCapturedBubble()
    {
        GameObject f;
        TempEnemyBehavior temp;
        switch(currentState)
        {
            case CaptureState.RED:
                temp = capturedBubble.GetComponent<TempEnemyBehavior>();
                temp.transform.localPosition = transform.localPosition;
                temp.transform.localRotation = transform.localRotation;
                temp.SetState(1);
                break;
            /*
                f = Instantiate(Resources.Load("Prefabs/EnemyBubbles/RedBubble") as
                    GameObject);
                //f = Instantiate(Resources.Load("Prefabs/CapturedBubbles/CapturedRedBubble") as
                //    GameObject);
                temp = f.GetComponent<TempEnemyBehavior>();
                temp.transform.localPosition = transform.localPosition;
                temp.transform.localRotation = transform.localRotation;        
                temp.SetState(1);
                break;
            */
            case CaptureState.BLUE:
                temp = capturedBubble.GetComponent<TempEnemyBehavior>();
                temp.transform.localPosition = transform.localPosition;
                temp.transform.localRotation = transform.localRotation;
                temp.SetState(1);
                break;
            /*
                f = Instantiate(Resources.Load("Prefabs/EnemyBubbles/BlueBubble") as
                    GameObject);
                //f = Instantiate(Resources.Load("Prefabs/CapturedBubbles/CapturedBlueBubble") as
                //    GameObject);
                temp = f.GetComponent<TempEnemyBehavior>();
                temp.transform.localPosition = transform.localPosition;
                temp.transform.localRotation = transform.localRotation;
                temp.SetState(1);
                break;
                */
            case CaptureState.YELLOW:
                temp = capturedBubble.GetComponent<TempEnemyBehavior>();
                temp.transform.localPosition = transform.localPosition;
                temp.transform.localRotation = transform.localRotation;
                temp.SetState(1);
                break;
            /*
                f = Instantiate(Resources.Load("Prefabs/EnemyBubbles/YellowBubble") as
                    GameObject);
                //f = Instantiate(Resources.Load("Prefabs/CapturedBubbles/CapturedYellowBubble") as
                //    GameObject);
                temp = f.GetComponent<TempEnemyBehavior>();
                temp.transform.localPosition = transform.localPosition;
                temp.transform.localRotation = transform.localRotation;
                temp.SetState(1);
                break;
                */
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.theManager.isInvincible == false)
        {
            switch (collision.gameObject.tag)
            {
                case "EnemyBullet":
                case "RedBubble":
                case "BlueBubble":
                case "YellowBubble":
                    GameManager.theManager.setLose();
                    //reset game, lose game
                    //player hits bullet and dies
                    //stats
                    //reset game, lose game
                    //player hits bullet and dies
                    //stats
                    break;
                default:
                    break;
            }
        }
    }
}
