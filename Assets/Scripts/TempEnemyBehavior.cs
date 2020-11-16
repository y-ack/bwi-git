using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyBehavior : MonoBehaviour
{
    
    public GameObject theGrid = null;
    Rigidbody2D rbody;
    private float latestDirectionChangeTime;
    private float directionChangeTime = 3f;
    private float characterVelocity = 5f;
    private float lerpSpeed = 0.005f;
    private float moveTimer;
    List<List<MapGenerator.Coord>> validCoords;
    private MapGenerator currentLevel;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    private const float bulletSpeed = 10f;

    public SpriteRenderer sprender;
    // Start is called before the first frame update

    public enum BubbleState
    {
        PATROL,
        CAPTURED,
        SHOOTING
    };
    private BubbleState currentState;

    void Start()
    {
        moveTimer = 0f;
        rbody = GetComponent<Rigidbody2D>();
        theGrid = GameObject.Find("Grid");
        currentLevel = theGrid.GetComponent<MapGenerator>();
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
        currentLevel.GetRegions(1);    
        sprender = gameObject.GetComponent<SpriteRenderer>();
        if (currentLevel == null)
        {
            Debug.Log("Unable to find wall tile in BubbleBehavior!");
        } 
    }

    void calcuateNewMovementVector()
    {     
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy
        if (currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y + 1)] == 1 &&
            currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x +1), Mathf.FloorToInt(transform.position.y)] == 1)
        {
            //Debug.Log("Wall Top,Right");
            movementDirection = new Vector2(Random.Range(-2.0f, 0f), Random.Range(-2.0f, 0f));
        }
        else if (currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x + 1), Mathf.FloorToInt(transform.position.y)] == 1 &&
                currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y - 1)] == 1)
        {
            //Debug.Log("Wall Right,Bottom");
            movementDirection = new Vector2(Random.Range(-2.0f, 0f), Random.Range(0f, 2.0f));
        }
        else if (currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x - 1), Mathf.FloorToInt(transform.position.y)] == 1 &&
                currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y - 1)] == 1)
        {
            //Debug.Log("Wall Bottom,Left");
            movementDirection = new Vector2(Random.Range(0f, 2.0f), Random.Range(0f, 2.0f));
        }
        else if (currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x -1), Mathf.FloorToInt(transform.position.y)] == 1 &&
                currentLevel.cavePoints[Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y + 1)] == 1)
        {
            //Debug.Log("Wall Left,Top");
            movementDirection = new Vector2(Random.Range(0f, 2.0f), Random.Range(-2.0f, 0f));
        }
        else
        {
            //Debug.Log("No Wall");
            movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        }               
        //movementPerSecond = movementDirection * characterVelocity;
        //Debug.Log("m/s: "+movementPerSecond+"dir: "+movementDirection + "Pos: " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // If The Gamemanager canMove = false, no in game object should be able to move.
        if (GameManager.theManager.canMove == true){
            stateControl();
        }
    }

    private void stateControl()
    {
        if (currentState == BubbleState.PATROL)
        {
            moveTimer += Time.smoothDeltaTime;
            // If the timer reaches direction time, calculate a new movement vector
            if (moveTimer > directionChangeTime)
            {
                directionChangeTime = directionChangeTime = Random.Range(3f, 6f);
                moveTimer = 0f;
                StartCoroutine(BubbleWander());
            }
            //[1] This will fix the bug where bubbles keep bashing their heads to the wall. 
            rbody.MovePosition(rbody.position + movementDirection * characterVelocity * Time.deltaTime);

            //[2] This is for moving bubble when we don't use rigid body but need to fix the bug where bubbles trying to move toward walls.
            //Vector3 p = new Vector3(transform.position.x + movementDirection.x,transform.position.y + movementDirection.y,transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, p, lerpSpeed);
        }
        if (currentState == BubbleState.CAPTURED)
        {
            //sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
            //transform.localPosition += transform.up * (bulletSpeed * Time.smoothDeltaTime);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (currentState == BubbleState.CAPTURED)
            {
                currentState = BubbleState.SHOOTING;
            }
        }
        if (currentState == BubbleState.SHOOTING)
        {
            sprender.enabled = true;
            transform.localPosition += transform.up * (bulletSpeed * Time.smoothDeltaTime);
        }

        /*
         * //if the changeTime was reached, calculate a new movement vector  
         * if (Time.time - latestDirectionChangeTime > directionChangeTime)
         * {
         * directionChangeTime = Random.Range(3f,6f);
         * latestDirectionChangeTime = Time.time; 
         * StartCoroutine(BubbleWander());
         * }
         * */
    }

    IEnumerator BubbleWander()
    {
        yield return StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        calcuateNewMovementVector();
        /*
        if (Time.time - latestDirectionChangeTime > directionChangeTime){
            latestDirectionChangeTime = Time.time;  
            calcuateNewMovementVector();       
        }      
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), 
        transform.position.y + (movementPerSecond.y * Time.deltaTime));
        */
        yield return new WaitForSeconds(5);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        //taken mostly verbatim from class collision example
        if (collision.gameObject.tag == "Bullet")
        {
            DestroySelf();
        }
        if (collision.gameObject.tag == "Capture")
        {
            currentState = BubbleState.CAPTURED;
        }

        if (collision.gameObject.tag == "Wall Top" && currentState == BubbleState.SHOOTING ||
            collision.gameObject.tag == "Wall" && currentState == BubbleState.SHOOTING)
        {
            DestroySelf();
        }

        if (collision.gameObject.tag == "Wall Top" && currentState == BubbleState.CAPTURED ||
            collision.gameObject.tag == "Wall" && currentState == BubbleState.CAPTURED)
        {
            Debug.Log("Colliding wall");
            calcuateNewMovementVector();
        }

        if (collision.gameObject.tag != "Player")
        {
            if (GetState() == BubbleState.SHOOTING)
            {
                DestroySelf();
            }
        }
        if (collision.gameObject.tag == "RedBubble" || collision.gameObject.tag == "BlueBubble" || collision.gameObject.tag == "YellowBubble")
        {
            /*
            FixedJoint joint = gameObject.AddComponent<FixedJoint>(); 
            // sets joint position to point of contact
            joint.anchor = collision.contacts[0].point; 
            // conects the joint to the other object
            joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
            */
            TempEnemyBehavior shotBubble = collision.gameObject.GetComponent<TempEnemyBehavior>();
            if (shotBubble.GetState() == BubbleState.SHOOTING)
            {
                if (collision.gameObject.tag == "RedBubble" || collision.gameObject.tag == "BlueBubble" || collision.gameObject.tag == "Yellowbubble")
                {
                    DestroySelf();
                }
            }
        }
        /*
        TempEnemyBehavior shotBubble = collision.gameObject.GetComponent<TempEnemyBehavior>();
        if(shotBubble.GetState() == BubbleState.SHOOTING)
        {
            if (collision.gameObject.tag == "RedBubble" || collision.gameObject.tag == "BlueBubble" || collision.gameObject.tag == "Yellowbubble")
            {
                DestroySelf();
            }
        }
        */

    }
    private void DestroySelf()
    {
        //ParentPlayer.eggDestroyed(this);
        //destroyed = true;
        GameManager.theManager.bubbleCleared();
        Destroy(transform.gameObject);  // kills self
    }

    public void SetState(int stateNumber)
    {
        switch(stateNumber)
        {
            case 0:
                break;
                currentState = BubbleState.PATROL;
            case 1:
                currentState = BubbleState.CAPTURED;
                break;
            case 2:
                currentState = BubbleState.SHOOTING;
                break;
        }
    }

    private BubbleState GetState()
    {
        return currentState;
    }
}
