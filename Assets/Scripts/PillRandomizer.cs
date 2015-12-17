using UnityEngine;
using System;

public class PillRandomizer : MonoBehaviour
{
    public int EffectNumber = 0;

    // problem on second load
    public static int[] PillNumbers;

	SpriteRenderer spriteRenderer;

    void Awake()
    {
        PillNumbers = new int[3] { (int)(UnityEngine.Random.value * 39), (int)(UnityEngine.Random.value * 39), (int)(UnityEngine.Random.value * 39) };
    }

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

    void LateUpdate()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Pills/pillMatrix_" + PillNumbers[EffectNumber]); ;
    }
}