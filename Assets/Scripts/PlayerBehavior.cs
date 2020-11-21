using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody2D rbody;
    private float moveSpeed;
    private float normalSpeed = 20f;
    private float focusSpeed;

    Vector2 mousePos;
    Vector2 movementVector;
    private Vector3 moveDir;

    public Camera cam;

    private bool isDashButtonDown;
    private float DashAmount = 5f;
    public float dashCoolDown = 1f;
    private float dashAfterSec = 0;

    public float captureCoolDown = 1f;
    private float captureAfterSec = 0;

    public bool isCapturing = false;
    private BubbleSpirit capturedBubble;

    public enum CaptureState
    {
        IDLE,
        CAPTURING
    };
    private CaptureState captureState;

    public enum PlayerState
    {
        NORMAL,
        ROLLING,
        FOCUS,
        DEAD
    };
    private PlayerState movementState;

    // Start is called before the first frame update
    private void Awake() {
        rbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        cam = Camera.main;

        rbody.gravityScale = 0;
        //set to 10 for testing, should discuss this later on.
        moveSpeed = normalSpeed;
        focusSpeed = normalSpeed / 2;
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
        switch(captureState)
        {
            case CaptureState.CAPTURING:
                //capturedBubble.transform.position = new Vector3(transform.position.x + 3, transform.position.y + 3, 0);
                //capturedBubble.transform.position.y = transform.position.y;
                //capturedBubble.transform.rotation = transform.rotation;
                break;
            case CaptureState.IDLE:
                break;
        }

        switch(movementState)
        {
            case PlayerState.NORMAL:
                moveSpeed = normalSpeed;
                break;
            case PlayerState.ROLLING:
                break;
            case PlayerState.FOCUS:
                moveSpeed = focusSpeed;
                break;
            case PlayerState.DEAD:
                break;
        }
    }

    float angle;
    void FixedUpdate()
    {
        //rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.fixedDeltaTime);
        rbody.velocity = moveDir * moveSpeed;
        Vector2 lookDir = mousePos - rbody.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rbody.rotation = angle;

        if (isDashButtonDown == true)
        {
            Dashing();

        }
    }

    public void SetCapture(BubbleSpirit bubbleSpirit)
    {
        isCapturing = true;
        capturedBubble = bubbleSpirit;
        captureState = CaptureState.CAPTURING;
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
            e.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//transform.localRotation;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (captureAfterSec <= 0 && isCapturing == false)
            {
                GameObject e = Instantiate(Resources.Load("Prefabs/net") as
                                   GameObject);
                e.transform.localPosition = transform.localPosition;
                e.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//transform.localRotation;
                captureAfterSec = captureCoolDown;
            }
            if (isCapturing == true)
            {
                isCapturing = capturedBubble.tryLaunch(
                    ((Vector3)mousePos - transform.position).normalized);
                if (isCapturing)
                    captureState = CaptureState.IDLE;
            }
        }
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementState = PlayerState.FOCUS;
        }
        else
        {
            movementState = PlayerState.NORMAL;
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
                    if(RunStatistics.Instance.currentLife == 0)
                    {
                        GameManager.theManager.setLose();
                    }
                    else
                    {
                        GameManager.theManager.playerHit();
                    }
                    
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
