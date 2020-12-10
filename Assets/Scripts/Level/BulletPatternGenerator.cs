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

    private BubbleBulletPattern Linear()
    {
        return Instantiate((BubbleBulletPatternLinear)Resources.Load("Prefabs/BubbleBulletPatternLinear",
                                                                     typeof(BubbleBulletPatternLinear)));
    }
    private BubbleBulletPattern Petal()
    {
        return Instantiate((BubbleBulletPatternLemniscate)Resources.Load("Prefabs/BubbleBulletPatternLemniscate",
                                                                         typeof(BubbleBulletPatternLemniscate)));
    }
    public void Awake()
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                patterns[i][j] = Linear().Init(new double[] { 2.0 }, 1, 0f, 0,
                                               0, 0, 0, 0, 0, 5f, 0);
            }
        }
        //15deg = 0.2617994
        patterns[0][1] = Linear().Init(new double[] { 2.5 }, 3, -0.2617994f, 0f,
                                       0.2617994f, 0, 0, 0, 0, 6f, 0);
        patterns[4][0] = Petal().Init(new double[] { 2.08, 0.0, 4.0, 1.5, 2.0, 0.5 }, 32, 0f, 0, 2 * 0.09817477f, 0, 0, 0, 0, 2.5f, 0);
        
        instance = this;
    }

    // Method used to add a savedPatern. Doesn't work cause Idk how this works. :(
    public void addSavedPattern(BubbleSpirit b, string patternType,
                                double[] parameter, float lifetime,
                                float patternLifetime)
    {
        BubbleBulletPattern pattern;
        switch (patternType)
        {
            case "BubbleBulletPatternLinear":
                pattern = (BubbleBulletPatternLinear)Resources.Load("Prefabs/BubbleBulletPatternLinear",
                                                                     typeof(BubbleBulletPatternLinear));
                break;
            //            case BubbleBulletPattern:
            //                pattern = Linear();break;
            case "BubbleBulletPatternLemniscate":
                pattern = (BubbleBulletPatternLemniscate)Resources.Load("Prefabs/BubbleBulletPatternLemniscate",
                                                                        typeof(BubbleBulletPatternLemniscate));
                break;
            default:
                Debug.Log("unrecognized pattern type (got " +
                          patternType + ", need BubbleBulletPattern*");
                pattern = (BubbleBulletPatternLinear)Resources.Load("Prefabs/BubbleBulletPatternLinear",
                                                                    typeof(BubbleBulletPatternLinear));
                break;
        }
        
        b.pattern = Instantiate(pattern, b.transform);
        b.pattern.parentBubble = b;
        b.pattern.setPatternParameters(parameter);
        b.pattern.lifetime = lifetime;
        b.pattern.patternLifetime = patternLifetime;
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
                b.pattern.enabled = true;
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
                    b.pattern.enabled = true;
                    b.pattern.parentBubble = b;
                }
                
            }
        }
    }

}
