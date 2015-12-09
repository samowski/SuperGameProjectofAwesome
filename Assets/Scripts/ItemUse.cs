using UnityEngine;
using System.Collections;

public class ItemUse : MonoBehaviour
{
    public bool IsHoldingPill = false;
    public bool IsHoldingChocolate = false;
    public GameObject ChocolatePrefab;
    public Transform spawnpoint;
    public Vector3 temp;

    void FixedUpdate()
    {
        if (Input.GetButton("Fire3") && (IsHoldingChocolate||IsHoldingPill))
        {
            UseItem();
        }
    }

    public void HoldChocolate()
    {
        IsHoldingChocolate = true;
    }

    public void UseItem()
    {
        if (IsHoldingChocolate && GameObject.Find("Granny").GetComponent<PlayerController>().IsSlowed)
        {
            CalculateSpawnpoint();
            IsHoldingChocolate = false;

            GameObject chocolate1 = (GameObject)Instantiate(ChocolatePrefab, temp, Quaternion.identity);
            GameObject.Find("Boy").GetComponent<KidController>().HasChocolate = true;
        }
        else if (IsHoldingPill)
        {

        }
    }

    void CalculateSpawnpoint()
    {
        temp = spawnpoint.position;
        temp.x += (float)1.5;
        temp.y += -1;
    }
}
