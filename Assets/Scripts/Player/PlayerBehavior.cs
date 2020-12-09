﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

[System.Serializable]
public class PlayerBehavior : MonoBehaviour
{
    //static private GameManager mGameManager;
    //static public void SetGameManager(GameManager g){ mGameManager = g; }
    public PlayerHitBox mPlayerHitbox = null;
    public Animator irisAnimator = null;

    Rigidbody2D rbody;
    public float moveSpeed { get; set; }
    public float normalSpeed { get; set; }
    public float focusSpeed { get; set; }

    public Vector2 mousePos { get; set; }
    public Vector2 movementVector { get; set; }
    public Vector3 moveDir { get; set; }
    public Vector3 slideDir { get; set; }
    public float slideSpeed { get; set; }

    public Camera cam;

    public bool isDashButtonDown { get; set;}
    public float DashAmount = 2f;
    public float dashCoolDown = 4.5f;
    public float dashAfterSec { get; set; }

    public float captureCoolDown = 1.2f;
    public float captureAfterSec;

    public float shootCoolDown = 0.4f;
    public float shootAfterSec;

    public bool isCapturing { get; set; } = false;
    public BubbleSpirit capturedBubble { get; set; }

    public int trapCount { get; set; }  = 0;
    public float extraTrap { get; set; } = 0;
    public int trapCountCap = 4;

    //Increase by 0.25f or 0.5f when upgrading 
    public float trapUpgrade { get; set; } = 0;

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
    private PlayerState movementState { get; set; }

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
        normalSpeed = 4.7f;
        DashAmount = 2f;
        setDefaultState();
    }

    public float angle;
    // Update is called once per frame

    void Update()
    {
        //normalSpeed = 400f;
        if (GameManager.theManager.canMove == true)
            buttonControl();
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rbody.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        if (trapCount > trapCountCap)
           setTrapCount(trapCountCap);

        if(startBlinking == true)
             SpriteBlinkingEffect();
        countdownCooldown();
        switch (movementState)
        {
            case PlayerState.NORMAL:
                moveSpeed = normalSpeed;
                mPlayerHitbox.hide();
                playerMovementControls();
                break;
            case PlayerState.ROLLING:
                HandleRolling();
                break;
            case PlayerState.FOCUS:
                moveSpeed = focusSpeed;
                mPlayerHitbox.show();
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
            slideSpeed = 8000f;
        } else if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
           && movementState != PlayerState.ROLLING)
        {
            movementState = PlayerState.FOCUS;
        } else if(movementState != PlayerState.ROLLING)
        {
            movementState = PlayerState.NORMAL;
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
            else if (isCapturing == true)
            {
                bool launchSuccess = capturedBubble.tryLaunch(
                    ((Vector3)mousePos - transform.position).normalized);

                if (launchSuccess)
                {
                    Debug.Log("launch success");
                    captureAfterSec = captureCoolDown;
                    captureState = CaptureState.IDLE;
                    isCapturing = false;
                    capturedBubble = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && trapCount >= 4)
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as
                                   GameObject);
            e.transform.localPosition = transform.localPosition;
            e.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            captureAfterSec = captureCoolDown;
            SubTrapForSplash();
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
        Debug.Log("this is speed" + normalSpeed);
        moveSpeed = normalSpeed;
        focusSpeed = normalSpeed / 2;
        dashCoolDown = 4.5f;
        captureCoolDown = 1.2f;
        shootCoolDown = 0.4f;
        trapCountCap = 4;
        isCapturing = false;
        capturedBubble = null;
        captureState = CaptureState.IDLE;
        movementState = PlayerState.NORMAL;
    }

    private void HandleRolling()
    {
        float rollSpeed = moveSpeed + 6f;
        float step = rollSpeed * Time.deltaTime;
        GameManager.theManager.isInvincible = true;
        //rbody.MovePosition(transform.position + moveDir * slideSpeed * Time.fixedDeltaTime);
        if(slideSpeed > 20f)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, step);
        }
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, step);
        //moveSpeed = focusSpeed;
        slideSpeed -= slideSpeed * moveSpeed * Time.fixedDeltaTime;
        
        if(slideSpeed <= 20f)
        {
            GameManager.theManager.isInvincible = false;
            movementState = PlayerState.NORMAL;
            moveSpeed = normalSpeed;
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
             case "Wall Top":
             case "Wall":
                if(movementState == PlayerState.ROLLING)
                {
                    slideSpeed = 15f;
                }
                break;
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
                CameraShaker.Instance.ShakeOnce(4f,2f,0.1f,1f);
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

    public void SubTrapForSplash()
    {
        trapCount -=4;
        RunStatistics.Instance.trapCount = trapCount;
    }


    public BubbleSpirit getbubbleSprite()
    {
        return capturedBubble;
    }

}
