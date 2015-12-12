using UnityEngine;
using System.Collections;

public class ItemUse : MonoBehaviour
{
    public bool IsHoldingPill = false;
    public bool IsHoldingChocolate = false;
    public GameObject ChocolatePrefab;

    public Collider2D slowCollider;

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
        if (GetComponent<PlayerController>().IsSlowed)
        {
            IsHoldingChocolate = false;

            Vector3 temppos = slowCollider.transform.position;
            if (slowCollider.GetComponent<KidController>().IsLookingRight)
            {
                temppos.x += 4;
            }
            else
            {
                temppos.x -= 4;
            }
            GameObject chocolate1 = (GameObject)Instantiate(ChocolatePrefab, temppos, Quaternion.identity);
            slowCollider.GetComponent<KidController>().HasChocolate = true;
        }
        else if (IsHoldingPill)
        {

        }
    }
}
