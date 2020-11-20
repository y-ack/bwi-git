using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject redBubble = null;
    public GameObject yellowbubble = null;
    public GameObject blueBubble = null;
    public GameObject player = null;
    public Grid mGrid;
    public Camera cam = null;
    private MapGenerator currentLevel;
    private float spawnRadius = 20f;
    private int ranX = 0;
    private int ranY = 0;
    private int bubbleCap;
    //minBubble and maxBubble can be set based on the level difficulty.
    private int minBubble = 10;
    private int maxBubble = 20;
    private bool playerSpawned = false;
    List<List<MapGenerator.Coord>> spawnPool;
    private void Start()
    {
    }

    public void spawnWorld()
    {
        currentLevel = mGrid.GetComponent<MapGenerator>();
        GameObject[] bubbleColors = { redBubble, yellowbubble, blueBubble };
        bubbleCap = Random.Range(minBubble,maxBubble);
        GameManager.theManager.currentBubbleSpirit = new GameObject[bubbleCap];

        if (currentLevel.canSpawn() == true)
        {
            StartCoroutine(SpawnPlayer());
            StartCoroutine(SpawnObj(0, bubbleColors, bubbleCap));
        }
    }

    public IEnumerator SpawnObj(int TileType, GameObject[] bubbleArray, int cap)
    {  
        spawnPool = mGrid.GetComponent<MapGenerator>().GetRegions(TileType);  
      
        if (spawnPool == null)
        {
            Debug.Log("Unable to find spawnable tile for enemy!");
        } 

        while(cap > 0)
        { 
            ranX = Random.Range(2,currentLevel.width);
            ranY = Random.Range(2,currentLevel.height);
            if (currentLevel.cavePoints[ranX,ranY] == TileType &&
                currentLevel.cavePoints[ranX + 1,ranY] == TileType &&
                currentLevel.cavePoints[ranX - 1,ranY] == TileType &&
                currentLevel.cavePoints[ranX,ranY + 1] == TileType &&
                currentLevel.cavePoints[ranX,ranY - 1] == TileType)
            {
                Vector2 p = player.transform.position;    
                    if( (Mathf.Pow(ranX - p.x, 2f) + Mathf.Pow(ranY - p.y, 2f)) > Mathf.Pow(spawnRadius, 2f))
                    {
                        GameObject e = Instantiate(Resources.Load("Prefabs/BubbleUnit")) as GameObject;
                        BubbleUnit bubbleParent = e.GetComponent<BubbleUnit>();
                        bubbleParent.transform.localPosition = new Vector3(ranX, ranY, 0);

                        GameObject f = Instantiate(Resources.Load("Prefabs/BubbleSpirit")) as GameObject;
                        BubbleSpirit bubbleChild = f.GetComponent<BubbleSpirit>();
                        bubbleChild.setParent(bubbleParent, new Vector2Int(0, 0));
                        bubbleChild.SetColor(Random.Range(0, 3));
                        
                        GameObject d = Instantiate(Resources.Load("Prefabs/BubbleSpirit")) as GameObject;
                        BubbleSpirit bubbleChild2 = d.GetComponent<BubbleSpirit>();
                        bubbleChild2.setParent(bubbleParent, new Vector2Int(1, 0));
                        bubbleChild2.SetColor(Random.Range(0, 3));

                        GameManager.theManager.addBubble();
                        //GameManager.theManager.currentBubbleSpirit[cap-1] = e;
                        cap--;      
                        
                    }
            }
        }
    yield return new WaitForSeconds(3f);
    }
    public IEnumerator SpawnPlayer()
    { 
        spawnPool = mGrid.GetComponent<MapGenerator>().GetRegions(0);
        if (spawnPool == null)
            {
                Debug.Log("Unable to find spawnable tile for player!");
            }
        while (playerSpawned == false)
        {           
            ranX = Random.Range(2,currentLevel.width);
            ranY = Random.Range(2,currentLevel.height);
            if (currentLevel.cavePoints[ranX,ranY] == 0 &&
                currentLevel.cavePoints[ranX + 1,ranY] == 0 &&
                currentLevel.cavePoints[ranX - 1,ranY] == 0 &&
                currentLevel.cavePoints[ranX,ranY + 1] == 0 &&
                currentLevel.cavePoints[ranX,ranY - 1] == 0)
            {             
                player.transform.localPosition = new Vector3(ranX, ranY, 0);
                playerSpawned = true;    
            }
        }
        playerSpawned = false;
        yield return new WaitForSeconds(1f);
    }
}
