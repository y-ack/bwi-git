using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player = null;
    public Grid mGrid;
    public Camera cam = null;
    private MapGenerator currentLevel;
    private float spawnRadius = 15f;
    private int ranX = 0;
    private int ranY = 0;
    private int normalCap;
    //minNormal and maxNormal can be set based on the level difficulty.
    private int minNormal;
    private int maxNormal = 50;
    private int minUnitSize = 2;
    private int maxUnitSize;
    private int totalColor = 2;

    private int bossCap;
    private int minBoss;
    private int maxBoss = 120;
    public GameObject[] boss;
    public BubbleSpirit[] bossChild;
    public List<int> unitsSize = new List<int>();
    public GameObject[] bubbles;
    public BubbleSpirit[] bubbleChild;

    private bool spawnPosFound = false;
    List<List<MapGenerator.Coord>> spawnPool;
    private void Start()
    {
        minNormal = Random.Range(15,20);
        minBoss = Random.Range(25,30);
    }
    

    public void spawnNormal(float difficulty)
    {
        currentLevel = mGrid.GetComponent<MapGenerator>();
        spawnPool = mGrid.GetComponent<MapGenerator>().GetRegions(0);  
        if (spawnPool == null)
        {
            Debug.Log("Unable to find spawnable tile for enemy!");
        } 
        normalCap = getNormalCap(difficulty);
        if (currentLevel.canSpawn() == true)
        {
            StartCoroutine(setPlayerNormal());
            setMaxUnitSize(difficulty);
            StartCoroutine(setNormal(normalCap, difficulty));
        }
    }
    

    public IEnumerator setNormal(int totalBubble, float difficulty)
    {   
        setTotalUnit(difficulty, totalBubble);
        setTotalColor(difficulty);
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
                //Don't be mad with those magic numbers, I calculated the total cells each ring
                //and got those but i'm too lazy to formular them :(
                if(i == 0)
                {
                    
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 3, difficulty);
                }
                else if (i < 7)
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
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 3, difficulty);
                }
                else if (i < 22)
                {
                    //Try to spawn a bubble if the cell is not occupied
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-2,3);
                        by = Random.Range(-2,3);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 3, difficulty);
                }
                else if (i < 43)
                {
                    //Try to spawn a bubble if the cell is not occupied
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-3,4);
                        by = Random.Range(-3,4);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 3, difficulty);
                }
                else if (i < 79)
                {
                    //Try to spawn a bubble if the cell is not occupied
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-4,5);
                        by = Random.Range(-4,5);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 3, difficulty);
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
        currentLevel = mGrid.GetComponent<MapGenerator>();
        spawnPool = mGrid.GetComponent<MapGenerator>().GetRegions(0);  
        if (spawnPool == null)
        {
            Debug.Log("Unable to find spawnable tile for enemy!");
        } 
        bossCap = getBossCap(difficulty);
        if (currentLevel.canSpawn() == true)
        {
            StartCoroutine(setPlayerBoss());
            StartCoroutine(setBoss(bossCap, difficulty));
            StartCoroutine(setNormal((int)(bossCap / 3f), difficulty / 3f));
        }
    }


    public IEnumerator setBoss(int bossCap, float difficulty)
    {     
        while (spawnPosFound == false)
        {
            //Spawn in the middle of the map
            ranX = Random.Range(Mathf.RoundToInt(currentLevel.width/2 - currentLevel.width/4), Mathf.RoundToInt(currentLevel.width/2 + currentLevel.width/4));
            ranY = Random.Range(Mathf.RoundToInt(currentLevel.height/2 - currentLevel.height/4), Mathf.RoundToInt(currentLevel.height/2 + currentLevel.height/4));  
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
            //Don't be mad with those magic numbers, I calculated the total cells each ring
            //and got those but i don't know how :(
            if(i == 0)
            {             
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], bossCap, difficulty);
            }
            else if (i < 7)
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
                bossChild[i].SetColor(Random.Range(0, totalColor));  
                BulletPatternGenerator.instance.addToBubble(bossChild[i], bossCap, difficulty);
            }
            else if (i < 22)
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
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], bossCap, difficulty);
            }
            else if (i < 43)
            {
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-3,4);
                    by = Random.Range(-3,4);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], bossCap, difficulty);
            }
            else if (i < 79)
            {
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-4,5);
                    by = Random.Range(-4,5);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], bossCap, difficulty);
            }
            else if (i < 122)
            {
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-5,6);
                    by = Random.Range(-5,6);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], bossCap, difficulty);
            }
            else
            {
                //For spawning bigger units 
                //To find the next hexagon ring area: 
            }            
            GameManager.theManager.addBubble();         
        }
    yield return new WaitForSeconds(2f);
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

    public void setTotalUnit(float difficulty, int totalBubble)
    {
        int bubbleLeft = totalBubble;  
        setUnitSize(difficulty);    
        while (bubbleLeft > 0)
        {
            if (bubbleLeft <= 0)
            {
                break;
            }
            int ranUnitSize = Random.Range(minUnitSize,maxUnitSize + 1);
            if (ranUnitSize > bubbleLeft)
            {
                ranUnitSize = Random.Range(minUnitSize,bubbleLeft + 1);
            } 
            unitsSize.Add(ranUnitSize);
            bubbleLeft -= ranUnitSize;
        }
    }
    public void setUnitSize(float difficulty)
    {
        //Equivalent to level 2 if curve is 2
        if (difficulty < 4)
        {
            minUnitSize = 2;
        }
         //Equivalent to level 7 if curve is 2
        else if (difficulty < 24)
        {
            minUnitSize = 3;
        }
        else
        {
            minUnitSize = 4;
        }
    }
    public void setTotalColor(float difficulty)
    {
        
        //Equivalent to level 2 if curve is 2
        if (difficulty < 4)
        {
            totalColor = 2;
        }
        //Equivalent to level 7 if curve is 2
        else if (difficulty < 24)
        {
            totalColor = 3;
        }
        else
        {
            totalColor = 4;
        }
    }

}


