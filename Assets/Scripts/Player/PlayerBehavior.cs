using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //static private GameManager mGameManager;
    //static public void SetGameManager(GameManager g){ mGameManager = g; }
    public PlayerHitBox mPlayerHitbox = null;
    public Animator irisAnimator = null;

    Rigidbody2D rbody;
    public float moveSpeed;
    public float normalSpeed = 4f;
    private float focusSpeed;

    Vector2 mousePos;
    Vector2 movementVector;
    private Vector3 moveDir;
    private Vector3 slideDir;
    private float slideSpeed;

    public Camera cam;

    private bool isDashButtonDown;
    public float DashAmount = 2f;
    public float dashCoolDown = 5f;
    private float dashAfterSec;

    public float captureCoolDown = 1.4f;
    public float captureAfterSec;

    public float shootCoolDown = 0.4f;
    public float shootAfterSec;

    public bool isCapturing = false;
    private BubbleSpirit capturedBubble;

    private int trapCount = 0;
    private float extraTrap = 0;
    private int trapCountCap = 10;

    //Increase by 0.25f or 0.5f when upgrading 
    private float trapUpgrade = 0;

    public float spriteBlinkingTimer = 0.0f;
    public float spriteBlinkingMiniDuration = 0.1f;
    public float spriteBlinkingTotalTimer = 0.0f;
    public float spriteBlinkingTotalDuration = 1.5f;
    public bool startBlinking = false;

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
        // this body needs dynamic to get rigidbody collision with walls
        // but we don't want physics simulation with other objects
        // set physics simulation to ignore all other layers
        Physics.IgnoreLayerCollision(11, 8); // bubble bullet
        Physics.IgnoreLayerCollision(11, 9); // bubble spirit
        Physics.IgnoreLayerCollision(11, 10); // player bullet
        Physics.IgnoreLayerCollision(11, 12); // player hitbox
    }

    void Start()
    {
        cam = Camera.main;
        movementState = PlayerState.NORMAL;
        resetTrapCount();
        rbody.gravityScale = 0;
        GameManager.theManager.isInvincible = false;
        //set to 10 for testing, should discuss this later on.
        setDefaultState();
    }

    public float angle;
    // Update is called once per frame

    void Update()
    {
        if (GameManager.theManager.canMove == true)
            buttonControl();

        countdownCooldown();
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rbody.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        if (trapCount > trapCountCap)
           setTrapCount(trapCountCap);

        if(startBlinking == true)
             SpriteBlinkingEffect();

        switch(movementState)
        {
            case PlayerState.NORMAL:
                moveSpeed = normalSpeed;
                mPlayerHitbox.hide();
                //GameManager.theManager.isInvincible = false;
                playerMovementControls();
                break;
            case PlayerState.ROLLING:
                HandleRolling();
                //GameManager.theManager.isInvincible = true;
                break;
            case PlayerState.FOCUS:
                moveSpeed = focusSpeed;
                mPlayerHitbox.show();
                //GameManager.theManager.isInvincible = false;
                playerMovementControls();
                break;
            case PlayerState.DEAD:
                break;
        }
    }

    
    private void buttonControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashAfterSec <= 0)
        {
            FindObjectOfType<AudioManager>().Play("Iris_Rolling");
            dashAfterSec = dashCoolDown;
            movementState = PlayerState.ROLLING;
            slideSpeed = 150f;
        }
        if (((Input.GetMouseButton(0) && shootAfterSec <= 0) && (trapCount > 0)) || 
            ((Input.GetKey(KeyCode.K) && shootAfterSec <= 0) && (trapCount > 0)))
        {
            FindObjectOfType<AudioManager>().Play("Iris_Trap"); 
            FindObjectOfType<AudioManager>().Play("Iris_Trap2");
            GameObject e = Instantiate(Resources.Load("Prefabs/Trap") as
                                   GameObject);
            e.transform.localPosition = transform.localPosition;
            e.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//transform.localRotation;
            GameObject l = Instantiate(Resources.Load("Prefabs/LightSources/PlayerTrapPointLight") as
                                   GameObject);
            l.transform.localPosition = transform.localPosition;
            l.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            shootAfterSec = shootCoolDown;
            subtrapCount();
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.L))
        {
            if (captureAfterSec <= 0 && !isCapturing)
            {
                FindObjectOfType<AudioManager>().Play("Iris_CaptureA");
                FindObjectOfType<AudioManager>().Play("Iris_CaptureB");
                GameObject e = Instantiate(Resources.Load("Prefabs/Capture") as
                                   GameObject);
                e.transform.localPosition = transform.localPosition;
                e.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                captureAfterSec = captureCoolDown;
            }
            if (isCapturing == true)
            {
                bool launchSuccess = capturedBubble.tryLaunch(
                    ((Vector3)mousePos - transform.position).normalized);

                if (launchSuccess)
                {
                    Debug.Log("launch succes");
                    captureAfterSec = captureCoolDown;
                    captureState = CaptureState.IDLE;
                    isCapturing = false;
                    capturedBubble = null;
                }
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

        rbody.MovePosition(rbody.position + movementVector * moveSpeed * Time.fixedDeltaTime);
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

    public void setDefaultState()
    {
        moveSpeed = normalSpeed;
        focusSpeed = normalSpeed / 2;
        dashCoolDown = 5f;
        captureCoolDown = 1.4f;
        shootCoolDown = 0.4f;
        isCapturing = false;
        capturedBubble = null;
        captureState = CaptureState.IDLE;
        movementState = PlayerState.NORMAL;
    }

    private void HandleRolling()
    {
        GameManager.theManager.isInvincible = true;
        rbody.MovePosition(transform.position + moveDir * slideSpeed * Time.fixedDeltaTime);
        slideSpeed -= slideSpeed * 8f * Time.fixedDeltaTime;
        if(slideSpeed <= 20f)
        {
            GameManager.theManager.isInvincible = false;
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
        if (dashAfterSec >= 0) { dashAfterSec -= Time.fixedDeltaTime; }
        if (captureAfterSec >= 0) { captureAfterSec -= Time.fixedDeltaTime; }
        if (shootAfterSec >= 0) { shootAfterSec -= Time.fixedDeltaTime; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            // case "EnemyBullet":
            // case "BubbleSpirit":
            //     Hit();
            //     break;
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
                //GameManager.theManager.SendLeaderboard();
            }
            else
            {
                GameManager.theManager.playerHit();
                //dunno why this isnt making invincible
                GameManager.theManager.isInvincible = true;
                startBlinking = true;
            }
        }
    }

    private void SpriteBlinkingEffect()
    {
        GameManager.theManager.isInvincible = false;
        spriteBlinkingTotalTimer += Time.deltaTime;
        if(spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            startBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;   // according to 
            //your sprite
            //GameManager.theManager.isInvincible = false;
            return;
        }
        GameManager.theManager.isInvincible = true;
        spriteBlinkingTimer += Time.deltaTime;
        if(spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (this.gameObject.GetComponent<SpriteRenderer> ().enabled == true) {
                this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;  //make changes
            } else {
                this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;   //make changes
            }
        }
    }

    public void setTrapUpgrade(float amount)
    {
        trapUpgrade += amount;
    }


    public int getTrapCount()
    {
        return trapCount;
    }

    public void setTrapCount(int amount)
    {
        trapCount = amount;
        RunStatistics.Instance.trapCount = trapCount;
    }
    public void resetTrapCount()
    {
        trapCount = 0;
        RunStatistics.Instance.trapCount = trapCount;
    }
    public void addtrapCount()
    {
        trapCount++;
        extraTrap += trapUpgrade;
        if (extraTrap >= 1.0f)
        {
            trapCount++;
            extraTrap--;
        }
        RunStatistics.Instance.trapCount = trapCount;
    }
    public void subtrapCount()
    {
        Debug.Log("Shot once!");
        trapCount--;
        RunStatistics.Instance.trapCount = trapCount;
    }


    public BubbleSpirit getbubbleSprite()
    {
        return capturedBubble;
    }

}
