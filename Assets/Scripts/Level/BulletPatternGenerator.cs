using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternGenerator : Singleton<BulletPatternGenerator>
{
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
                    Instantiate(Resources.Load("Prefabs/BubbleBulletPatternLinear"))
                    as GameObject;
                BubbleBulletPattern pattern = g.GetComponent<BubbleBulletPattern>();
                pattern.Init(new float[] { 2.0f }, 1, 0f, 0, 0, 0, 0, 0, 3f, 0);
                patterns[i][j] = pattern;
            }
        }
    }

    public const float patternChance = 1.0f;
    public void addToBubble(BubbleSpirit b, int unit_count, float difficulty)
    {
        //TODO[FINAL] research grid position, color, number in unit to
        // modulate the params of patterns
        if (Random.value <= patternChance) //chance to not use pattern?
        {
            var bucket = patterns[(int)(difficulty / (100 / 5))];
            var choice = bucket[Random.Range(0, bucket.Length - 1)];
            b.pattern = Instantiate(choice, b.transform);
        }
    }

}
