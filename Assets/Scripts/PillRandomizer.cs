using UnityEngine;
using System;

public class PillRandomizer : MonoBehaviour
{
    public int EffectNumber = 0;

    // problem on second load
    static bool isRandomized = false;
    static int[] PillNumbers;

	SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (!isRandomized)
        {
            PillNumbers = new int[3] {
                (int)(UnityEngine.Random.value * 39),
                (int)(UnityEngine.Random.value * 39),
                (int)(UnityEngine.Random.value * 39)
            };
            isRandomized = true;
        }
    }

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Pills/pillMatrix_" + PillNumbers[EffectNumber]);
	}
}