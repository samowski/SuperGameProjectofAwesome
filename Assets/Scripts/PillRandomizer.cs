using UnityEngine;
using System;

public class PillRandomizer : MonoBehaviour
{
    public int EffectNumber=0;

    public static int[] PillNumbers = new int[3];
    public static int PillNumber1;
    public static int PillNumber2;
    public static int PillNumber3;

    void Start()
    {
        PillNumbers[0] = (int)(UnityEngine.Random.value * 15);
        PillNumbers[1] = (int)(UnityEngine.Random.value * 15);
        PillNumbers[2] = (int)(UnityEngine.Random.value * 15);
    }

    void LateUpdate()
    {        
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pills/pillMatrix_" + PillNumbers[EffectNumber]); ;
    }
}
