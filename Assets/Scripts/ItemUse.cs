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

    Sprite pill0Sprite;
    Sprite pill1Sprite;
    Sprite pill2Sprite;

    LevelMenu levelMenu;

    void Start()
    {
        pill0Sprite = PillPrefab0.GetComponent<SpriteRenderer>().sprite;
        pill1Sprite = PillPrefab1.GetComponent<SpriteRenderer>().sprite;
        pill2Sprite = PillPrefab2.GetComponent<SpriteRenderer>().sprite;

        var go = GameObject.Find("LevelMenu");
        if (go != null) levelMenu = go.GetComponent<LevelMenu>();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Fire3") && IsHoldingChocolate && UsageCooldown == 0)
        {
            UseChocolate();
        }
        if (Input.GetButton("Fire4") && IsHoldingPill && UsageCooldown == 0)
        {
            UsePill();
        }
        if (PickupCooldown > 0)
        {
            PickupCooldown--;
        }
        if (UsageCooldown > 0)
        {
            UsageCooldown--;
        }
    }

    public void HoldChocolate()
    {
        IsHoldingChocolate = true;
        if (levelMenu != null)
            levelMenu.SetHUDChocolate(true);
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
                Instantiate(HoldingPill0 ? PillPrefab0 : HoldingPill1 ? PillPrefab1 : HoldingPill2 ? PillPrefab2 : null, temppos, Quaternion.identity);
            }

            IsHoldingPill = true;
            switch (pillNumber)
            {
            case 0:
                HoldingPill0 = true;
                HoldingPill1 = false;
                HoldingPill2 = false;
                if (levelMenu != null)
                    levelMenu.SetHUDPill(pill0Sprite);
                    break;
                case 1:
                    HoldingPill0 = false;
                    HoldingPill1 = true;
                    HoldingPill2 = false;
                if (levelMenu != null)
                    levelMenu.SetHUDPill(pill1Sprite);
                    break;
                case 2:
                    HoldingPill0 = false;
                    HoldingPill1 = false;
                    HoldingPill2 = true;
                if (levelMenu != null)
                    levelMenu.SetHUDPill(pill2Sprite);
                    break;
            }
        }
    }

    public void UseChocolate()
    {
        if (levelMenu != null)
            levelMenu.SetHUDChocolate(false);

        UsageCooldown = 20;

        if (GetComponent<PlayerController>().IsSlowed && IsHoldingChocolate)
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
            Instantiate(ChocolatePrefab, temppos, Quaternion.identity);
            slowCollider.GetComponent<KidController>().HasChocolate = true;
        }
    }

    public void UsePill()
    {
        if (levelMenu != null)
            levelMenu.SetHUDPill(null);

        UsageCooldown = 20;

        if (IsHoldingPill)
        {
            if (HoldingPill0)
            {
                GameObject.Find("Rollator").GetComponent<PunchDamage>().damage += 2;
            }
            if (HoldingPill1)
            {
                gameObject.GetComponent<PlayerController>().SetPillSpeed(45);
            }
            if (HoldingPill2)
            {
                gameObject.GetComponent<PlayerController>().SetPillSpeed(15);
            }
            IsHoldingPill = false;
            HoldingPill0 = false;
            HoldingPill1 = false;
            HoldingPill2 = false;
        }
    }
}
