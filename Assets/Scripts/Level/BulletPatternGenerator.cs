using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternGenerator : MonoBehaviour
{
    public static BubbleBullet bulletPrefab1;
    
    public static BulletPatternGenerator instance;
    public PatternInfo[][] patterns  = new PatternInfo[][] {
        new PatternInfo[3],
        new PatternInfo[3],
        new PatternInfo[3],
        new PatternInfo[3],
        new PatternInfo[3]
    };

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
        bulletPrefab1 = (BubbleBullet)Resources.Load("Prefabs/BubbleBulletPrefab",
                                                     typeof(BubbleBullet));
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                var a = new PatternInfo
                {
                    bcnt = 1,
                    θ = 0,
                    σ = 0,
                    Δθ = 0,
                    ω = 0,
                    a = 0,
                    at = 0,
                    d = 0,
                    cd = 5f,
                    ΔΣΘ = 0,
                    bulletPrefab = bulletPrefab1,
                    v = new double[] { 2.0 },
                    patternType = PatternType.Linear,
                };
                patterns[i][j] = a;//Linear().Init(new double[] { 2.0 }, 1, 0f, 0,
                                   //              0, 0, 0, 0, 0, 5f, 0);
            }
        }
        //15deg = 0.2617994
        //        patterns[0][1] = Linear().Init(new double[] { 2.5 }, 3, -0.2617994f, 0f,
        //                               0.2617994f, 0, 0, 0, 0, 6f, 0);
        //patterns[4][0] = Petal().Init(new double[] { 2.08, 0.0, 4.0, 1.5, 2.0, 0.5 }, 32, 0f, 0, 2 * 0.09817477f, 0, 0, 0, 0, 2.5f, 0);
        
        instance = this;
    }

    public BubbleBulletPattern instantiatePatternInfo(PatternInfo pattern,
                                                      BubbleSpirit parent)
    {
        BubbleBulletPattern newPattern;
        switch (pattern.patternType)
        {
            case PatternType.Linear:
                newPattern = Instantiate(Linear(), parent.transform); break;
            case PatternType.Petal:
                newPattern = Instantiate(Petal(), parent.transform); break;
            default:
                Debug.Log("maybe bad pattern type ...");
                newPattern = Instantiate(Linear(), parent.transform); break;
        }
        newPattern.Init(pattern);
        newPattern.arg.bulletPrefab = bulletPrefab1;
        newPattern.parentBubble = parent;
        return newPattern;
    }
    // Method used to add a savedPatern. Doesn't work cause Idk how this works. :(
    public void addSavedPattern(BubbleSpirit b,
                                PatternInfo info)
    {
        var pattern = instantiatePatternInfo(info, b);
        pattern.setPatternInfo(info);
        b.pattern = pattern;
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
                b.pattern = instantiatePatternInfo(patterns[4][0], b);
            }
        }
        if (unit_type == 0 && b.gridPosition == Vector2Int.zero)
        {            
            if (noneShooter > miniBoss) //chance to be a mini boss
            {
                Debug.Log("Yo! we've got a miniboss! It's only 1% chance!");
                b.pattern = instantiatePatternInfo(patterns[4][0], b);
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
                    b.pattern = instantiatePatternInfo(choice, b);
                }
                else
                {
                    var bucket = patterns[Mathf.RoundToInt((difficulty) / (100 / 5))];                  
                    var choice = bucket[Random.Range(0, bucket.Length - 1)];
                    b.pattern = instantiatePatternInfo(choice, b);
                }
            }
        }
    }
}
