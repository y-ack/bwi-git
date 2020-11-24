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
    public float moveSpeed;
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
    public float dashCoolDown = 5f;
    private float dashAfterSec;

    public float captureCoolDown = 3f;
    private float captureAfterSec;

    public float shootCoolDown = 0.4f;
    public float shootAfterSec;

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
        setDefaultStat();
    }

    // Update is called once per frame
    void Update()
    {
        countdownCooldown();
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
        if (Input.GetKeyDown(KeyCode.Space) && dashAfterSec <= 0)
        {
            FindObjectOfType<AudioManager>().Play("Iris_Rolling");
            dashAfterSec = dashCoolDown;
            Debug.Log("cool down " + dashAfterSec);
            Debug.Log(dashCoolDown);
            movementState = PlayerState.ROLLING;
            slideSpeed = 150f;
        }
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
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
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

    public void setDefaultStat()
    {
        moveSpeed = normalSpeed;
        focusSpeed = normalSpeed / 2;
        dashCoolDown = 5f;
        captureCoolDown = 3f;
        shootCoolDown = 0.4f;
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
    public void SetCapture(BubbleSpirit bubbleSpirit)
    {
        FindObjectOfType<AudioManager>().Play("Iris_Capturing");
        isCapturing = true;
        capturedBubble = bubbleSpirit;
        captureState = CaptureState.CAPTURING;
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
        if (shootAfterSec > 0)
        {
            shootAfterSec -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "EnemyBullet":
                Hit();
                break;
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
