using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    static private GameManager mGameManager;
    static public void SetGameManager(GameManager g){ mGameManager = g; }
    public PlayerHitBox mPlayerHitbox = null;
    public Animator irisAnimator = null;

    Rigidbody2D rbody;
    private float moveSpeed;
    private float normalSpeed = 15f;
    private float focusSpeed;

    Vector2 mousePos;
    Vector2 movementVector;
    private Vector3 moveDir;
    private Vector3 slideDir;
    private float slideSpeed;

    public Camera cam;

    private bool isDashButtonDown;
    private float DashAmount = 5f;
    public float dashCoolDown = 1f;
    private float dashAfterSec = 0;

    public float captureCoolDown = 1f;
    private float captureAfterSec = 0;

    public float shootCoolDown = 0.4f;
    public float shootAfterSec = 0;

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
        movementState = PlayerState.NORMAL;

        rbody.gravityScale = 0;
        //set to 10 for testing, should discuss this later on.
        moveSpeed = normalSpeed;
        focusSpeed = normalSpeed / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.theManager.canMove == true)
        {
            buttonControl();
        }
        switch(movementState)
        {
            case PlayerState.NORMAL:
                moveSpeed = normalSpeed;
                mPlayerHitbox.hide();
                mGameManager.isInvincible = false;
                playerMovementControls();
                break;
            case PlayerState.ROLLING:
                HandleRolling();
                mGameManager.isInvincible = true;
                break;
            case PlayerState.FOCUS:
                moveSpeed = focusSpeed;
                mPlayerHitbox.show();
                mGameManager.isInvincible = false;
                playerMovementControls();
                break;
            case PlayerState.DEAD:
                break;
        }
    }

    public float angle;
    void FixedUpdate()
    {
        //rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.fixedDeltaTime);
        rbody.velocity = moveDir * moveSpeed;
        Vector2 lookDir = mousePos - rbody.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //rbody.rotation = angle;
    }
    private void buttonControl()
    {
        if (Input.GetKeyDown(KeyCode.F) && (dashAfterSec <= 0))
        {
            FindObjectOfType<AudioManager>().Play("Iris_Rolling");
            movementState = PlayerState.ROLLING;
            slideSpeed = 150f;
        }
        countdownCooldown();
        if (Input.GetMouseButton(0) && shootAfterSec <= 0)
        {
            FindObjectOfType<AudioManager>().Play("Iris_Trap"); 
            FindObjectOfType<AudioManager>().Play("Iris_Trap2");
            GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as
                                   GameObject);
            e.transform.localPosition = transform.localPosition;
            e.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//transform.localRotation;
            shootAfterSec = shootCoolDown;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (captureAfterSec <= 0 && isCapturing == false)
            {
                FindObjectOfType<AudioManager>().Play("Iris_CaptureA");
                FindObjectOfType<AudioManager>().Play("Iris_CaptureB");
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
        if(Input.GetKey(KeyCode.Space))
        {
            movementState = PlayerState.FOCUS;
        }
        else if(movementState != PlayerState.ROLLING)
        {
            movementState = PlayerState.NORMAL;
        }
    }

    private void playerMovementControls()
    {
        movementVector = new Vector2(Input.GetAxis("Horizontal"),
                                     Input.GetAxis("Vertical"));

        float horizontalSpeed = Mathf.Abs(Input.GetAxis("Horizontal") * moveSpeed);
        float verticalSpeed = Mathf.Abs(Input.GetAxis("Vertical") * moveSpeed);

        irisAnimator.SetFloat("Speed", horizontalSpeed + verticalSpeed);

        rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.smoothDeltaTime);
        rbody.angularVelocity = 0f;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        directionHandling();

        moveDir = new Vector3(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")).normalized;
    }

    private void directionHandling()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > transform.localPosition.x)
        {
            if ((mousePos.y - transform.localPosition.y) > 2.5)
            {
                irisAnimator.SetBool("isUp", true);
                irisAnimator.SetBool("isDown", false);
                irisAnimator.SetBool("isLeft", false);
                irisAnimator.SetBool("isRight", false);
            }
            else if((mousePos.y - transform.localPosition.y) < -2.5)
            {
                irisAnimator.SetBool("isUp", false);
                irisAnimator.SetBool("isDown", true);
                irisAnimator.SetBool("isLeft", false);
                irisAnimator.SetBool("isRight", false);
            }
            else
            {
                irisAnimator.SetBool("isUp", false);
                irisAnimator.SetBool("isDown", false);
                irisAnimator.SetBool("isLeft", false);
                irisAnimator.SetBool("isRight", true);
            }

        }
        else
        {
            if ((mousePos.y - transform.localPosition.y) > 2.5)
            {
                irisAnimator.SetBool("isUp", true);
                irisAnimator.SetBool("isDown", false);
                irisAnimator.SetBool("isLeft", false);
                irisAnimator.SetBool("isRight", false);
            }
            else if ((mousePos.y - transform.localPosition.y) < -2.5)
            {
                irisAnimator.SetBool("isUp", false);
                irisAnimator.SetBool("isDown", true);
                irisAnimator.SetBool("isLeft", false);
                irisAnimator.SetBool("isRight", false);
            }
            else
            {
                irisAnimator.SetBool("isUp", false);
                irisAnimator.SetBool("isDown", false);
                irisAnimator.SetBool("isLeft", true);
                irisAnimator.SetBool("isRight", false);
            }
        }
    }

    private void HandleRolling()
    {
        rbody.MovePosition(transform.position + moveDir * slideSpeed * Time.smoothDeltaTime);
        slideSpeed -= slideSpeed * 8f * Time.smoothDeltaTime;
        if(slideSpeed <= 20f)
        {
            movementState = PlayerState.NORMAL;
        }
    }

    /*
    public void Dashing()
    {
        rbody.MovePosition(transform.position + moveDir * DashAmount);
        isDashButtonDown = false;
        dashAfterSec = dashCoolDown;
    }
    */
    public void SetCapture(BubbleSpirit bubbleSpirit)
    {
        FindObjectOfType<AudioManager>().Play("Iris_Capturing");
        isCapturing = true;
        capturedBubble = bubbleSpirit;
        captureState = CaptureState.CAPTURING;
    }

    public void countdownCooldown()
    {
        if (captureAfterSec > 0)
        {
            captureAfterSec -= Time.deltaTime;
        }
        if (shootAfterSec > 0)
        {
            shootAfterSec -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            default:
                break;
        }
    }
    public void Hit()
    {
        if (GameManager.theManager.isInvincible == false)
        {
            if (RunStatistics.Instance.currentLife == 0)
            {
                GameManager.theManager.setLose();
            }
            else
            {
                GameManager.theManager.playerHit();
            }
        }
    }
}
