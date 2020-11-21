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
    private int minBubble = 20;
    private int maxBubble = 100;
    private int maxUnitSize;
    public List<int> unitsSize = new List<int>();

    public GameObject[] bubbles;
    public BubbleSpirit[] bubbleChild;
    private bool playerSpawned = false;
    List<List<MapGenerator.Coord>> spawnPool;
    private void Start()
    {
    }
    public int getBubbleCap(float difficulty)
    {
        return minBubble + Mathf.RoundToInt(difficulty * 0.25f);
    }

    //Placeholder for difficulty, can be set to curve later.
    public void setMaxUnitSize(float difficulty)
    {
        if (difficulty <= 25f)
        {
            maxUnitSize = 3;
        }
        else if (difficulty <= 50f)
        {
            maxUnitSize = 4;
        }
        else if (difficulty <= 75f)
        {
            maxUnitSize = 5;
        }
        else
        {
            maxUnitSize = 6;
        }
    }
    public void getTotalUnit( int totalBubble)
    {
        int bubbleLeft = totalBubble;      
        while (bubbleLeft > 0)
        {
            if (bubbleLeft <= 0)
            {
                break;
            }
            int ranUnitSize = Random.Range(1,maxUnitSize + 1);
            if (ranUnitSize > bubbleLeft)
            {
                ranUnitSize = Random.Range(1,bubbleLeft + 1);
            } 
            unitsSize.Add(ranUnitSize);
            bubbleLeft -= ranUnitSize;
        }
    }

    public void spawnBubbles(float difficulty)
    {
        currentLevel = mGrid.GetComponent<MapGenerator>();
        GameObject[] bubbleColors = { redBubble, yellowbubble, blueBubble };
        bubbleCap = getBubbleCap(difficulty);
        GameManager.theManager.currentBubbleSpirit = new GameObject[bubbleCap];

        if (currentLevel.canSpawn() == true)
        {
            StartCoroutine(SpawnPlayer());
            setMaxUnitSize(difficulty);
            StartCoroutine(SpawnObj(0, bubbleColors, bubbleCap));
        }
    }
    

    public IEnumerator SpawnObj(int TileType, GameObject[] bubbleArray, int totalBubble)
    {  
        spawnPool = mGrid.GetComponent<MapGenerator>().GetRegions(TileType);  
        if (spawnPool == null)
        {
            Debug.Log("Unable to find spawnable tile for enemy!");
        } 
        getTotalUnit(totalBubble);
        Vector2 p = player.transform.position;    
        foreach(int unit in unitsSize) 
        { 
            //Trying to get a spawnable location
            while (true)
            {
                ranX = Random.Range(2,currentLevel.width);
                ranY = Random.Range(2,currentLevel.height);
                if (currentLevel.cavePoints[ranX,ranY] == TileType &&
                    currentLevel.cavePoints[ranX + 2,ranY] == TileType &&
                    currentLevel.cavePoints[ranX - 2,ranY] == TileType &&
                    currentLevel.cavePoints[ranX,ranY + 2] == TileType &&
                    currentLevel.cavePoints[ranX,ranY - 2] == TileType)
                {
                    if((Mathf.Pow(ranX - p.x, 2f) + Mathf.Pow(ranY - p.y, 2f)) > Mathf.Pow(spawnRadius, 2f))
                    { 
                        break;
                    }
                }               
            }   
            GameObject e = Instantiate(Resources.Load("Prefabs/BubbleUnit")) as GameObject;
            BubbleUnit bubbleParent = e.GetComponent<BubbleUnit>();
            bubbleParent.transform.localPosition = new Vector3(ranX, ranY, 0);
                        
            bubbles = new GameObject[unit];
            bubbleChild = new BubbleSpirit[unit];
            int bx = 0;
            int by = 0;
            for(int i = 0; i < unit;i++)
            {              
                bubbles[i] = Instantiate(Resources.Load("Prefabs/BubbleSpirit")) as GameObject;
                bubbleChild[i] = bubbles[i].GetComponent<BubbleSpirit>();
                if(i == 0)
                {
                    
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, 3));
                }
                else if (i > 0 && i <= 6)
                {
                    //Try to spawn a bubble if the cell is not occupied
                    while (true)
                    {
                        bx = Random.Range(-1,2);
                        by = Random.Range(-1,2);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            break;                          
                        }
                        else
                        {
                            continue;
                        }
                    }
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, 3));  
                }
                else
                {
                    //For Bigger unit (like Bosses)
                }
                GameManager.theManager.addBubble();
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

    /*
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

                        GameManager.theManager.addBubble();GameManager.theManager.addBubble();
                        //GameManager.theManager.currentBubbleSpirit[cap-1] = e;
                        cap-=2; //you're spawning two right now
                        
                    }
            }
        }
    yield return new WaitForSeconds(3f);
    }*/


}
