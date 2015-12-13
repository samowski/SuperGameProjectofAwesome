using UnityEngine;
using System.Collections;

public class ItemUse : MonoBehaviour
{
    public bool IsHoldingPill = false;
    public bool HoldingPill0 = false;
    public bool HoldingPill1 = false;
    public bool HoldingPill2 = false;
    public bool IsHoldingChocolate = false;
    public GameObject ChocolatePrefab;
    public GameObject PillPrefab0;
    public GameObject PillPrefab1;
    public GameObject PillPrefab2;
    public static uint PickupCooldown = 0;
    public static uint UsageCooldown = 0;

    public Collider2D slowCollider;

    void FixedUpdate()
    {
        if (Input.GetButton("Fire3") && (IsHoldingChocolate||IsHoldingPill) && UsageCooldown==0)
        {
            UseItem();
        }
        if (PickupCooldown > 0)
        {
            PickupCooldown--;
        }
        if(UsageCooldown>0)
        {
            UsageCooldown--;
        }
    }

    public void HoldChocolate()
    {
        IsHoldingChocolate = true;
    }

    public void HoldPill(int pillNumber)
    {
        if (PickupCooldown == 0)
        {
            PickupCooldown = 20;
            if (IsHoldingPill)
            {
                Vector3 temppos = gameObject.transform.position;
                temppos.y += 2;
                GameObject oldPill = (GameObject)Instantiate(HoldingPill0 ? PillPrefab0 : HoldingPill1 ? PillPrefab1 : HoldingPill2 ? PillPrefab2 : null, temppos, Quaternion.identity);
            }

            IsHoldingPill = true;
            switch (pillNumber)
            {
                case 0:
                    HoldingPill0 = true;
                    HoldingPill1 = false;
                    HoldingPill2 = false;
                    break;
                case 1:
                    HoldingPill0 = false;
                    HoldingPill1 = true;
                    HoldingPill2 = false;
                    break;
                case 2:
                    HoldingPill0 = false;
                    HoldingPill1 = false;
                    HoldingPill2 = true;
                    break;
            }
        }
    }

    public void UseItem()
    {
        UsageCooldown = 20;

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
            GameObject.Find("Rollator").GetComponent<PunchDamage>().damage += 2;
            Debug.Log("isholgingpill false");
            IsHoldingPill = false;
            HoldingPill0 = false;
            HoldingPill1 = false;
            HoldingPill2 = false;
        }
    }
}
