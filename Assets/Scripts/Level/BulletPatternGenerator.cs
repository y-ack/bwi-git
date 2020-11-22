using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternGenerator : MonoBehaviour
{
    public static BubbleBulletPattern[][] patterns = new BubbleBulletPattern[][] {
        new BubbleBulletPattern[] {
        },
        new BubbleBulletPattern[] {
        },
        new BubbleBulletPattern[] {
        },
        new BubbleBulletPattern[] {
        },
        new BubbleBulletPattern[] {
        }
    };

    void Start()
    { //generate presets?
    }


    public const float patternChance = 1.0f;
    public static void addToBubble(BubbleSpirit b, int unit_count, float difficulty)
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
