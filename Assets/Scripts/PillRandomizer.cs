using UnityEngine;
using System;

public class PillRandomizer : MonoBehaviour
{
    public int EffectNumber = 0;

    public static int[] PillNumbers = new int[3] { (int)(UnityEngine.Random.value * 39), (int)(UnityEngine.Random.value * 39), (int)(UnityEngine.Random.value * 39) };

	SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

    void LateUpdate()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Pills/pillMatrix_" + PillNumbers[EffectNumber]); ;
    }
}