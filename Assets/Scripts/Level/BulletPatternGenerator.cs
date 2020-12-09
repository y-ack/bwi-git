using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternGenerator : MonoBehaviour
{
    public static BulletPatternGenerator instance;
    public BubbleBulletPattern[][] patterns  = new BubbleBulletPattern[][] {
        new BubbleBulletPattern[3],
        new BubbleBulletPattern[3],
        new BubbleBulletPattern[3],
        new BubbleBulletPattern[3],
        new BubbleBulletPattern[3]
    };
    // public List<List<BubbleBulletPattern>> patterns = new List<List<BubbleBulletPattern>> {
    //     new List<BubbleBulletPattern>(),
    //     new List<BubbleBulletPattern>(),
    //     new List<BubbleBulletPattern>(),
    //     new List<BubbleBulletPattern>(),
    //     new List<BubbleBulletPattern>()
    // };

    private BubbleBulletPattern Linear()
    {
        return (BubbleBulletPatternLinear)Resources.Load("Prefabs/BubbleBulletPatternLinear",
                                                         typeof(BubbleBulletPatternLinear));
    }
    private BubbleBulletPattern Petal()
    {
        return (BubbleBulletPatternLemniscate)Resources.Load("Prefabs/BubbleBulletPatternLemniscate",
                                                             typeof(BubbleBulletPatternLemniscate));
    }
    public void Awake()
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                patterns[i][j] = Linear().Init(new double[] { 2.0 }, 1, 0f, 0, 0, 0, 0, 0, 0, 5f, 0);
            }
        }
        patterns[4][0] = Petal().Init(new double[] { 2.08, 0.0, 4.0, 1.5, 2.0, 0.5 }, 32, 0f, 0, 2 * 0.09817477f, 0, 0, 0, 0, 2.5f, 0);
        
        instance = this;
    }

    public void addSavedPattern(BubbleSpirit b, double[] parameter )
    {
        b.pattern = Instantiate(patterns[4][0], b.transform);
        b.pattern.parentBubble = b;
        b.pattern.setPatternParameters(parameter);
    }

    public void addToBubble(BubbleSpirit b, int unit_type, float difficulty)
    {
        const float patternChance = 1.0f;
        const float miniBoss = 1.966f;
        float noneShooter = Random.Range(-1.0f, 2.0f);
        //TODO[FINAL] research grid position, color, number in unit to
        // modulate the params of patterns
        if (unit_type == 1) //boss detection
        {
            if (b.gridPosition == Vector2Int.zero)
            {
                b.pattern = Instantiate(patterns[4][0], b.transform);
                b.pattern.parentBubble = b;
            }
        }
        if (unit_type == 0 && b.gridPosition == Vector2Int.zero)
        {            
            if (noneShooter > miniBoss) //chance to be a mini boss
            {
                Debug.Log("Yo! we've got a miniboss! It's only 1% chance!");
                b.pattern = Instantiate(patterns[4][0], b.transform);
                b.pattern.parentBubble = b;
                return;
            }
        }
        if (unit_type == 0)
        {
            //chance to not use pattern
            if (Random.value <= patternChance)
            {
                
                if (difficulty >= 99f)
                {
                    var bucket = patterns[3];
                    var choice = bucket[Random.Range(0, bucket.Length - 1)];
                    b.pattern = Instantiate(choice, b.transform);
                    b.pattern.parentBubble = b;
                }
                else
                {
                    var bucket = patterns[Mathf.RoundToInt((difficulty) / (100 / 5))];                  
                    var choice = bucket[Random.Range(0, bucket.Length - 1)];
                    b.pattern = Instantiate(choice, b.transform);
                    b.pattern.parentBubble = b;
                }
                
            }
        }
    }

}
