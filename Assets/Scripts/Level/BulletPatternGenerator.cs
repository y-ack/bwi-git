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

    public void Awake()
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                var g =
                    (BubbleBulletPatternLinear)Resources.Load("Prefabs/BubbleBulletPatternLinear",
                                                              typeof(BubbleBulletPatternLinear));
                g.Init(new double[] { 2.0 }, 1, 0f, 0, 0, 0, 0, 0, 0, 3f, 0);
                patterns[i][j] = (BubbleBulletPattern)g;
            }
        }
        var g2 =
            (BubbleBulletPatternLemniscate)Resources.Load("Prefabs/BubbleBulletPatternLemniscate",
                                                          typeof(BubbleBulletPatternLemniscate));
        g2.Init(new double[] { 2.08, 0, 4, 1.5, 2, 0.5 }, 48, 0f, 0, float.NaN, 0, 0, 0, 0, 2f, 0);
        patterns[4][0] = (BubbleBulletPattern)g2;
        
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
                var bucket = patterns[(int)(difficulty / (100 / 5))];
                var choice = bucket[Random.Range(0, bucket.Length - 1)];
                b.pattern = Instantiate(choice, b.transform);
                b.pattern.parentBubble = b;
            }
        }
    }

}
