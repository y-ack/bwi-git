using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyBehavior : MonoBehaviour
{
    
    public GameObject theGrid = null;
    Rigidbody2D rbody;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;
    private float characterVelocity = 2f;

    List<List<MapGenerator.Coord>> validCoords;
    private bool moveDone = false;
    private MapGenerator currentLevel;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        theGrid = GameObject.Find("Grid");
        currentLevel = theGrid.GetComponent<MapGenerator>();
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
    }
    void calcuateNewMovementVector(){
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * characterVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        //if the changeTime was reached, calculate a new movement vector
        StartCoroutine(BubbleWander());
    }
    IEnumerator BubbleWander()
    {
        yield return StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        if (Time.time - latestDirectionChangeTime > directionChangeTime){
            latestDirectionChangeTime = Time.time;
            calcuateNewMovementVector();
        }
        //rbody.MovePosition(rbody.position + movementDirection * movementPerSecond * Time.deltaTime);

        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), 
        transform.position.y + (movementPerSecond.y * Time.deltaTime));
        yield return new WaitForSeconds(2);
    }



    private void OnTriggerEnter2D(Collider2D collision){
        //taken mostly verbatim from class collision example
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Capture")
        {
            destroySelf();
        }
        if (collision.gameObject.tag == "Wall Top")
        {
            Debug.Log("Colliding wall");
            calcuateNewMovementVector();
        }
        
    }
    private void destroySelf()
    {
        //ParentPlayer.eggDestroyed(this);
        //destroyed = true;
        Destroy(transform.gameObject);  // kills self
    }
}
