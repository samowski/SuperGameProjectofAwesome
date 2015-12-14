using UnityEngine;
using System.Collections;

public class ItemUse : MonoBehaviour
{
    public int holdPillIndex = -1;
    public bool IsHoldingChocolate = false;
    public GameObject ChocolatePrefab;
    public GameObject[] pills;
    public float PickupCooldown = 0;
    public float UsageCooldown = 0;

    public Collider2D slowCollider;

    PlayerController playerController;
    KidController kidController;

    PunchDamage rollatorDamage;

    [SerializeField]
    float currentPickupCooldown = 0.0f;
    [SerializeField]
    float currentUsageCooldown = 0.0f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        kidController = slowCollider.GetComponent<KidController>();

        rollatorDamage = GameObject.Find("Rollator").GetComponent<PunchDamage>();
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (Input.GetButton("Fire3") && (IsHoldingChocolate||isHoldingPill()) && currentUsageCooldown <= 0.0)
        {
            UseItem();
        }
        if (currentPickupCooldown > 0)
        {
            currentPickupCooldown -= delta;
        }
        if(currentUsageCooldown>0)
        {
            currentUsageCooldown -= delta;
        }
    }

    bool isHoldingPill()
    {
        return holdPillIndex > -1;
    }

    public void HoldChocolate()
    {
        IsHoldingChocolate = true;
    }

    public bool HoldPill(int pillNumber)
    {
        if (currentPickupCooldown <= 0.0f && currentPickupCooldown <= 0.0f)
        {
            currentPickupCooldown = PickupCooldown;

            if (isHoldingPill())
            {
                Vector3 temppos = gameObject.transform.position;
                temppos.y += 2;
                Instantiate(pills[holdPillIndex], temppos, Quaternion.identity);
            }

            holdPillIndex = pillNumber;

            return true;
        }
        return false;
    }

    public void UseItem()
    {
        currentUsageCooldown = UsageCooldown;

        if (playerController.IsSlowed)
        {
            IsHoldingChocolate = false;

            Vector3 temppos = slowCollider.transform.position;
            if (kidController.IsLookingRight)
            {
                temppos.x += 4;
            }
            else
            {
                temppos.x -= 4;
            }
            Instantiate(ChocolatePrefab, temppos, Quaternion.identity);
            kidController.HasChocolate = true;
        }
        else if (isHoldingPill())
        {
            rollatorDamage.damage += 2;
            Debug.Log("isholgingpill false");
            holdPillIndex = -1;
        }
    }
}
