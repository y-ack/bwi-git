using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player = null;
    public Grid mGrid;
    public Camera cam = null;
    private MapGenerator currentLevel;
    private float spawnRadius = 20f;
    private int ranX = 0;
    private int ranY = 0;
    private int normalCap;
    //minNormal and maxNormal can be set based on the level difficulty.
    private int minNormal = 20;
    private int maxNormal = 50;
    private int maxUnitSize;

    private int bossCap;
    private int minBoss = 30;
    private int maxBoss = 75;
    public GameObject[] boss;
    public BubbleSpirit[] bossChild;

    public List<int> unitsSize = new List<int>();
    public GameObject[] bubbles;
    public BubbleSpirit[] bubbleChild;

    private bool spawnPosFound = false;
    List<List<MapGenerator.Coord>> spawnPool;
    private void Start()
    {
        currentLevel = mGrid.GetComponent<MapGenerator>();
        spawnPool = mGrid.GetComponent<MapGenerator>().GetRegions(0);  
        if (spawnPool == null)
        {
            Debug.Log("Unable to find spawnable tile for enemy!");
        } 
    }
    

    public void spawnNormal(float difficulty)
    {
        normalCap = getNormalCap(difficulty);
        if (currentLevel.canSpawn() == true)
        {
            StartCoroutine(setPlayerNormal());
            setMaxUnitSize(difficulty);
            StartCoroutine(setNormal(normalCap));
        }
    }
    

    public IEnumerator setNormal(int totalBubble)
    {   
        setTotalUnit(totalBubble);
        Vector2 p = player.transform.position;    

        foreach(int unit in unitsSize) 
        { 
            //Trying to get a spawnable location
            while (spawnPosFound == false)
            {
                ranX = Random.Range(2,currentLevel.width);
                ranY = Random.Range(2,currentLevel.height);
                if (currentLevel.cavePoints[ranX,ranY] == 0 &&
                    currentLevel.cavePoints[ranX + 2,ranY] == 0 &&
                    currentLevel.cavePoints[ranX - 2,ranY] == 0 &&
                    currentLevel.cavePoints[ranX,ranY + 2] == 0 &&
                    currentLevel.cavePoints[ranX,ranY - 2] == 0)
                {
                    if((Mathf.Pow(ranX - p.x, 2f) + Mathf.Pow(ranY - p.y, 2f)) > Mathf.Pow(spawnRadius, 2f))
                    { 
                        spawnPosFound = true;
                    }
                }               
            }  
            spawnPosFound = false; 
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
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-1,2);
                        by = Random.Range(-1,2);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, 3));  
                }
                else
                {
                    //For spawning bigger units 
                }
                GameManager.theManager.addBubble();
            }  
                                                                                                              
        }
    yield return new WaitForSeconds(3f);
    }

    public void spawnBoss(float difficulty)
    {
        //TODO: Instantiate this array length based on the difficulty.

        bossCap = getBossCap(difficulty);
        if (currentLevel.canSpawn() == true)
        {
            StartCoroutine(setPlayerBoss());
            StartCoroutine(setBoss(bossCap));
        }
    }


    public IEnumerator setBoss(int bossCap)
    {     
        while (spawnPosFound == false)
        {
            //Spawn in the middle of the map in an 30x15 area
            ranX = Random.Range(currentLevel.width/2 - 15, currentLevel.width/2 + 16);
            ranY = Random.Range(currentLevel.height/2 - 7, currentLevel.height/2 + 8);  
            //Spawn 5 units away from a wall             
            if (currentLevel.cavePoints[ranX,ranY] == 0 &&
                currentLevel.cavePoints[ranX + 5,ranY] == 0 &&
                currentLevel.cavePoints[ranX - 5,ranY] == 0 &&
                currentLevel.cavePoints[ranX,ranY + 5] == 0 &&
                currentLevel.cavePoints[ranX,ranY - 5] == 0)
            {
                spawnPosFound = true;
            }              
        }   
        spawnPosFound = false;
        GameObject e = Instantiate(Resources.Load("Prefabs/BubbleUnit")) as GameObject;
        BubbleUnit bossParent = e.GetComponent<BubbleUnit>();
        bossParent.transform.localPosition = new Vector3(ranX, ranY, 0);
        
        boss = new GameObject[bossCap];
        bossChild = new BubbleSpirit[bossCap];
        int bx = 0;
        int by = 0;
        for(int i = 0; i < bossCap;i++)
        {              
            boss[i] = Instantiate(Resources.Load("Prefabs/BubbleSpirit")) as GameObject;
            bossChild[i] = boss[i].GetComponent<BubbleSpirit>();
            if(i == 0)
            {
                
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, 4));
            }
            else if (i > 0 && i <= 6)
            {
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-1,2);
                    by = Random.Range(-1,2);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                        
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, 4));  
            }
            else if (i > 6 && i <= 21)
            {
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-2,3);
                    by = Random.Range(-2,3);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, 3));  
            }
            else
            {
                //For spawning bigger units 
            }
            GameManager.theManager.addBubble(); 
                                                                                                              
        }
    yield return new WaitForSeconds(3f);
    }


    
    public IEnumerator setPlayerNormal()
    { 
        while (spawnPosFound == false)
        {           
            ranX = Random.Range(2,currentLevel.width - 2);
            ranY = Random.Range(2,currentLevel.height - 2);
            if (currentLevel.cavePoints[ranX,ranY] == 0 &&
                currentLevel.cavePoints[ranX + 1,ranY] == 0 &&
                currentLevel.cavePoints[ranX - 1,ranY] == 0 &&
                currentLevel.cavePoints[ranX,ranY + 1] == 0 &&
                currentLevel.cavePoints[ranX,ranY - 1] == 0)
            {             
                player.transform.localPosition = new Vector3(ranX, ranY, 0);
                spawnPosFound = true;    
            }
        }
        spawnPosFound = false;
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator setPlayerBoss()
    { 
        /* Trying to get spawnable location Xs
        -----------------------------
        |            5          x   |
        |  x   ---------------      |
        |      |             |      |
        |  10  |             |  10  |
        |      |             |      |
        |      ---------------      |
        |            5     x        |
        -----------------------------  */
        while (spawnPosFound == false)
        {           
            ranX = Random.Range(0,2);
            if (ranX == 0)
            {
                ranX = Random.Range(2,currentLevel.width - 50);
            }
            else
            {
                ranX = Random.Range(currentLevel.width - 10, currentLevel.width - 2);
            }
            ranY = Random.Range(0,2);
            if (ranY == 0)
            {
                ranY = Random.Range(2,currentLevel.height - 33);          
            }
            else
            {
                ranY = Random.Range(currentLevel.height - 7, currentLevel.height - 2);
            }
            if (currentLevel.cavePoints[ranX,ranY] == 0 &&
                currentLevel.cavePoints[ranX + 1,ranY] == 0 &&
                currentLevel.cavePoints[ranX - 1,ranY] == 0 &&
                currentLevel.cavePoints[ranX,ranY + 1] == 0 &&
                currentLevel.cavePoints[ranX,ranY - 1] == 0)
            {             
                player.transform.localPosition = new Vector3(ranX, ranY, 0);
                spawnPosFound = true;    
            }
        }
        spawnPosFound = false;
        yield return new WaitForSeconds(0.5f);
    }

    public int getNormalCap(float difficulty)
    {
        int normalCap = minNormal + Mathf.RoundToInt(difficulty * 0.25f);
        if (normalCap >= maxNormal)
            return maxNormal;
        else
            return normalCap;
    }
    public int getBossCap(float difficulty)
    {
        int bossCap = minBoss + Mathf.RoundToInt(difficulty * 0.25f);
        if (bossCap >= maxBoss)
            return maxBoss;
        else
            return bossCap;
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

    public void setTotalUnit( int totalBubble)
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


}


