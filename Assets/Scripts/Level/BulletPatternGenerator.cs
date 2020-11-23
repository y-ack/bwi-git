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

    public const float patternChance = 1.0f;
    public void addToBubble(BubbleSpirit b, int unit_count, float difficulty)
    {
        //TODO[FINAL] research grid position, color, number in unit to
        // modulate the params of patterns
        if (unit_count > 18) //cheap boss detection todo
        {
            if (b.gridPosition == Vector2Int.zero)
            {
                b.pattern = Instantiate(patterns[4][0], b.transform);
                b.pattern.parentBubble = b;
            }
        } else
        {
            if (Random.value <= patternChance) //chance to not use pattern?
            {
                // difficulty - 0.001f so that difficulty won't never get to exactly 100
                var bucket = patterns[(int)((difficulty - 0.001f) / (100 / 5))];                
                var choice = bucket[Random.Range(0, bucket.Length - 1)];
                b.pattern = Instantiate(choice, b.transform);
                b.pattern.parentBubble = b;
            }
        }
    }

}
