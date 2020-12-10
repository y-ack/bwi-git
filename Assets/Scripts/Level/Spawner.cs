using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player = null;
    public Grid mGrid;
    public Camera cam = null;
    private MapGenerator currentLevel;
    private float spawnRadius = 1f;
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
    public List<int> unitsSize;
    public GameObject[] bubbles;
    public BubbleSpirit[] bubbleChild;
    public int unitCount = 0;
    private bool spawnPosFound = false;
    List<List<MapGenerator.Coord>> spawnPool;
    private void Start()
    {
        unitsSize = new List<int>();
        minNormal = Random.Range(15,20);
        minBoss = Random.Range(25,30);
    }
    

    public void spawnNormal(float difficulty)
    {
        Debug.Log("unitCount before reset: "+ unitCount);  
        unitCount = 0;
        
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
            unitsSize.RemoveAll(unit => unit > 0);
            setTotalUnit(normalCap, difficulty);
            setTotalColor(difficulty);
            StartCoroutine(setNormal(difficulty));

        }
    }
    
    void Clear(int i)
    {
        
    }
    public IEnumerator setNormal(float difficulty)
    {   
        
        Vector2 p = player.transform.position;    
        int ring = 1;
        foreach(int unit in unitsSize) 
        { 
            unitCount++;
            //Debug.Log("unitsSize["+ unit +"] = " + unitsSize[unit]);
            //Trying to get a spawnable location
            while (spawnPosFound == false)
            {
                ranX = Random.Range(2,currentLevel.width);
                ranY = Random.Range(2,currentLevel.height);
                if (currentLevel.cavePoints[ranX,ranY] == 0 &&
                    currentLevel.cavePoints[ranX + ring,ranY] == 0 &&
                    currentLevel.cavePoints[ranX - ring,ranY] == 0 &&
                    currentLevel.cavePoints[ranX,ranY + ring] == 0 &&
                    currentLevel.cavePoints[ranX,ranY - ring] == 0 &&
                    currentLevel.cavePoints[ranX + ring,ranY + ring] == 0 &&
                    currentLevel.cavePoints[ranX - ring,ranY - ring] == 0 &&
                    currentLevel.cavePoints[ranX + ring,ranY - ring] == 0 &&
                    currentLevel.cavePoints[ranX - ring,ranY + ring] == 0)
                {
                    if((Mathf.Pow(ranX - p.x, 2f)/(15*15) + Mathf.Pow(ranY - p.y, 2f)/(10*10)) > Mathf.Pow(spawnRadius, 2f))
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
                    ring = 1;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 0, difficulty);
                }
                else if (i < 7)
                {
                    ring = 2;
                    //Try to spawn a bubble if the cell is not occupied
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-ring + 1,ring);
                        by = Random.Range(-ring + 1,ring);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 0, difficulty);
                }
                else if (i < 22)
                {
                    ring = 3;
                    //Try to spawn a bubble if the cell is not occupied
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-ring + 1,ring);
                        by = Random.Range(-ring + 1,ring);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 0, difficulty);
                }
                else if (i < 43)
                {
                    ring = 4;
                    //Try to spawn a bubble if the cell is not occupied
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-ring + 1,ring);
                        by = Random.Range(-ring + 1,ring);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 0, difficulty);
                }
                else if (i < 79)
                {
                    ring = 5;
                    //Try to spawn a bubble if the cell is not occupied
                    while (spawnPosFound == false)
                    {
                        bx = Random.Range(-ring + 1,ring);
                        by = Random.Range(-ring + 1,ring);
                        Vector2Int tmp = new Vector2Int(bx,by);
                        if (bubbleParent.cellOrNull(tmp) == null)
                        {
                            spawnPosFound = true;                        
                        }                       
                    }
                    spawnPosFound = false;
                    bubbleChild[i].setParent(bubbleParent, new Vector2Int(bx, by));
                    bubbleChild[i].SetColor(Random.Range(0, totalColor));  
                    BulletPatternGenerator.instance.addToBubble(bubbleChild[i], 0, difficulty);
                }
                else
                {
                    //For spawning bigger units 
                }
                GameManager.theManager.addBubble();
            }  
        }
    yield return new WaitForSeconds(0);
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
            unitsSize.RemoveAll(unit => unit > 0);
            setTotalUnit(Mathf.RoundToInt(bossCap / 4f), difficulty);
            setTotalColor(difficulty);
            StartCoroutine(setNormal(difficulty));
        }
    }


    public IEnumerator setBoss(int bossCap, float difficulty)
    {     
        int ring = 1;
        while (spawnPosFound == false)
        {
            //Spawn in the middle of the map
            ranX = Random.Range(Mathf.RoundToInt(currentLevel.width/2f - currentLevel.width/4f), Mathf.RoundToInt(currentLevel.width/2f + currentLevel.width/4f));
            ranY = Random.Range(Mathf.RoundToInt(currentLevel.height/2f - currentLevel.height/4f), Mathf.RoundToInt(currentLevel.height/2f + currentLevel.height/4f));  
            //Spawn 6 units away from a wall in all directions             
            if (currentLevel.cavePoints[ranX,ranY] == 0 &&
                currentLevel.cavePoints[ranX + ring,ranY] == 0 &&
                currentLevel.cavePoints[ranX - ring,ranY] == 0 &&
                currentLevel.cavePoints[ranX,ranY + ring] == 0 &&
                currentLevel.cavePoints[ranX,ranY - ring] == 0 &&
                currentLevel.cavePoints[ranX + ring,ranY + ring] == 0 &&
                currentLevel.cavePoints[ranX - ring,ranY - ring] == 0 &&
                currentLevel.cavePoints[ranX + ring,ranY - ring] == 0 &&
                currentLevel.cavePoints[ranX - ring,ranY + ring] == 0
                )
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
            //Don't be mad with those magic numbers, I calculated the total cells for each ring
            //and got those but i'm too lazy to formular them :(
            if(i == 0)
            {    
                ring = 1;         
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], 1, difficulty);
            }
            else if (i < 7)
            {
                ring = 2;
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-ring + 1,ring);
                    by = Random.Range(-ring + 1,ring);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                        
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));  
                BulletPatternGenerator.instance.addToBubble(bossChild[i], 1, difficulty);
            }
            else if (i < 22)
            {
                ring = 3;
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-ring + 1,ring);
                    by = Random.Range(-ring + 1,ring);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], 1, difficulty);
            }
            else if (i < 43)
            {
                ring = 4;
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-ring + 1,ring);
                    by = Random.Range(-ring + 1,ring);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], 1, difficulty);
            }
            else if (i < 79)
            {
                ring = 5;
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-ring + 1,ring);
                    by = Random.Range(-ring + 1,ring);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], 1, difficulty);
            }
            else if (i < 122)
            {
                ring = 6;
                //Try to spawn a bubble if the cell is not occupied
                while (spawnPosFound == false)
                {
                    bx = Random.Range(-ring + 1,ring);
                    by = Random.Range(-ring + 1,ring);
                    Vector2Int tmp = new Vector2Int(bx,by);
                    if (bossParent.cellOrNull(tmp) == null)
                    {
                        spawnPosFound = true;                         
                    }
                }
                spawnPosFound = false;
                bossChild[i].setParent(bossParent, new Vector2Int(bx, by));
                bossChild[i].SetColor(Random.Range(0, totalColor));
                BulletPatternGenerator.instance.addToBubble(bossChild[i], 1, difficulty);
            }
            else
            {
                //For spawning bigger units 
                //To find the next hexagon ring area: 
            }            
            GameManager.theManager.addBubble();         
        }
    yield return new WaitForSeconds(0);
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
        yield return new WaitForSeconds(0);
    }

    public IEnumerator setPlayerBoss()
    { 
        /* Trying to get spawnable location Xs
        -----------------------------
        |            3          x   |
        |  x   ---------------      |
        |      |             |      |
        |  5   |             |  5   |
        |      |             |      |
        |      ---------------      |
        |            3     x        |
        -----------------------------  */
        while (spawnPosFound == false)
        {           
            ranX = Random.Range(0,2);
            if (ranX == 0)
            {
                ranX = Random.Range(2,Mathf.RoundToInt(currentLevel.width/10f) + 2);
            }
            else
            {
                ranX = Random.Range(Mathf.RoundToInt(currentLevel.width - (currentLevel.width/10f)) - 2, currentLevel.width - 2);
            }
            ranY = Random.Range(0,2);
            if (ranY == 0)
            {
                ranY = Random.Range(2,Mathf.RoundToInt(currentLevel.height/10f) + 2);          
            }
            else
            {
                ranY = Random.Range(Mathf.RoundToInt(currentLevel.height - (currentLevel.height/5f)) - 2, currentLevel.height - 2);
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
        yield return new WaitForSeconds(0);
    }

    public int getNormalCap(float difficulty)
    {
        int normalCap = minNormal + Mathf.RoundToInt(difficulty * 0.5f);
        if (normalCap >= maxNormal)
            return maxNormal;
        else
            return normalCap;
    }
    public int getBossCap(float difficulty)
    {
        int bossCap = minBoss + Mathf.RoundToInt(difficulty * 0.5f);
        if (bossCap >= maxBoss)
            return maxBoss;
        else
            return bossCap;
    }

    //Placeholder for difficulty, can be set to curve later.
    //Set unit size min and max based on difficulty
    public void setUnitSize(float difficulty)
    {
        //Equivalent to level 2 if curve is 2
        if (difficulty < 4)
        {
            minUnitSize = 2;
            maxUnitSize = 4;
        }
         //Equivalent to level 7 if curve is 2
        else if (difficulty < 25)
        {
            minUnitSize = 3;
            maxUnitSize = 5;
        }
        else
        {
            minUnitSize = 4;
            maxUnitSize = 6;
        }
    }
    //Set total units from the number of bubbles
    public void setTotalUnit( int totalBubble,float difficulty)
    {
        int bubbleLeft = totalBubble;  
        setUnitSize(difficulty);    
        while (bubbleLeft > 0)
        {
            if (bubbleLeft <= 0)
            {
                break;
            }
            if (bubbleLeft == 1)
            {
                unitCount++;
                unitsSize.Add(1);
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
   
    public void setTotalColor(float difficulty)
    {
        
        //Equivalent to level 2 if curve is 2
        if (difficulty < 4)
        {
            totalColor = 2;
        }
        //Equivalent to level 7 if curve is 2
        else if (difficulty < 10)
        {
            totalColor = 3;
        }
        else
        {
            totalColor = 4;
        }
    }

    public void quickSpawnUnit()
    {
        QuickSaveData theSaveData = SaveSystem.quickLoad();


        for (int i = 0; i < theSaveData.currentBubbleUnit.Length; i++)
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/BubbleUnit")) as GameObject;
            e.transform.localPosition = theSaveData.currentBubbleUnit[i].unitPosition.getVectorThree();
            e.GetComponent<BubbleUnit>().initialPosition = theSaveData.currentBubbleUnit[i].initialPosition.getVectorThree();
            e.GetComponent<BubbleUnit>().movePosition = theSaveData.currentBubbleUnit[i].movePosition.getVectorThree();
            e.GetComponent<BubbleUnit>().timeToMove = theSaveData.currentBubbleUnit[i].timeToMove;
            e.GetComponent<BubbleUnit>().moveTimer = theSaveData.currentBubbleUnit[i].moveTimer;

            SerializableBubbleSpirit[] unitChildren = theSaveData.currentBubbleUnit[i].childrenBubble;

            for (int j = 0; j < unitChildren.Length; j++)
            {
                GameObject f = Instantiate(Resources.Load("Prefabs/BubbleSpirit")) as GameObject;
                f.transform.position = unitChildren[j].bubblePosition.getVectorThree();

                switch (unitChildren[j].state)
                {
                    case SerializableBubbleSpirit.State.CAPTURED:
                        f.GetComponent<BubbleSpirit>().state = BubbleSpirit.State.CAPTURED;
                        break;

                    case SerializableBubbleSpirit.State.LAUNCHED:
                        f.GetComponent<BubbleSpirit>().state = BubbleSpirit.State.LAUNCHED;
                        break;

                    case SerializableBubbleSpirit.State.CLEARED:
                        f.GetComponent<BubbleSpirit>().state = BubbleSpirit.State.CLEARED;
                        break;

                    default:
                        f.GetComponent<BubbleSpirit>().state = BubbleSpirit.State.NORMAL;
                        break;
                }

                f.GetComponent<BubbleSpirit>().rebounds = unitChildren[j].rebounds;
                f.GetComponent<BubbleSpirit>().launchDirection = unitChildren[j].launchDirection.getVectorThree();
                f.GetComponent<BubbleSpirit>().cleared = unitChildren[j].cleared;
                f.GetComponent<BubbleSpirit>().isChain = unitChildren[j].isChain;
                f.GetComponent<BubbleSpirit>().SetColor(unitChildren[j].color);
                f.GetComponent<BubbleSpirit>().setParent(e.GetComponent<BubbleUnit>(), unitChildren[j].gridPosition.getVectorTwoInt());

                 BulletPatternGenerator.instance.addSavedPattern(f.GetComponent<BubbleSpirit>(), unitChildren[j].patternType,
                                                                unitChildren[j].patternParameter, unitChildren[j].lifeTime,
                                                                unitChildren[j].patternLifeTime);
                 
            }

            e.GetComponent<BubbleUnit>().radius = theSaveData.currentBubbleUnit[i].radius;
            e.GetComponent<BubbleUnit>().bubbleCount = theSaveData.currentBubbleUnit[i].bubbleCount;
        }
    }

    public void quickSpawnEnemyProjectile()
    {
        QuickSaveData theSaveData = SaveSystem.quickLoad();

        for (int i = 0; i < theSaveData.currentBubbleProjectile.Length; i++)
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/BubbleBulletPrefab")) as GameObject;
            e.transform.localPosition = theSaveData.currentBubbleProjectile[i].projectilePosition.getVectorThree();
            e.transform.localRotation = theSaveData.currentBubbleProjectile[i].projectilePosition.getQuaternion();
            e.GetComponent<BubbleBullet>().velocity = theSaveData.currentBubbleProjectile[i].velocity.getVectorThree();
            e.GetComponent<BubbleBullet>().angularVelocity = theSaveData.currentBubbleProjectile[i].angularVelocity;
            e.GetComponent<BubbleBullet>().acceleration = theSaveData.currentBubbleProjectile[i].acceleration;
            e.GetComponent<BubbleBullet>().accelerationTimeout = theSaveData.currentBubbleProjectile[i].accelerationTimeout;

        }
    }

    public void quickSpawnPlayerProjectile()
    {
        QuickSaveData theSaveData = SaveSystem.quickLoad();

        for (int i = 0; i < theSaveData.capturePlayerProjectile.Length; i++)
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/Capture") as
                                   GameObject);

            e.transform.localPosition = theSaveData.capturePlayerProjectile[i].bulletDirection.getVectorThree();
            e.transform.localRotation = theSaveData.capturePlayerProjectile[i].bulletRotation.getQuaternion();
            e.GetComponent<CaptureBulletBehavior>().rebounds = theSaveData.capturePlayerProjectile[i].rebounds;
            e.GetComponent<CaptureBulletBehavior>().disabled = theSaveData.capturePlayerProjectile[i].disabled;
        }

        for (int i = 0; i < theSaveData.currentPlayerProjectile.Length; i++)
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/Trap") as
                                   GameObject);

            e.transform.localPosition = theSaveData.currentPlayerProjectile[i].bulletDirection.getVectorThree();
            e.transform.localRotation = theSaveData.currentPlayerProjectile[i].bulletRotation.getQuaternion();
            e.GetComponent<PlayerBulletBehavior>().lifeSpan = theSaveData.currentPlayerProjectile[i].lifeSpan;
            e.GetComponent<PlayerBulletBehavior>().disabled = theSaveData.currentPlayerProjectile[i].disabled;
        }
    }
}


