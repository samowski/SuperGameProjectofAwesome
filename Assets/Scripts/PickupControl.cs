using UnityEngine;
using System.Collections;

public class PickupControl : MonoBehaviour
{
    GameObject Granny;

    Collider2D pickupCollider;
    Collider2D grannyCollider;

    ItemUse itemUse;

    void Start()
    {
        Granny = GameObject.Find("Granny");

        pickupCollider = GetComponent<Collider2D>();
        grannyCollider = Granny.GetComponent<Collider2D>();

        itemUse = Granny.GetComponent<ItemUse>();
    }

    void FixedUpdate()
    {
        bool gotPill = false;

        if (pickupCollider.IsTouching(grannyCollider) && Input.GetButton("Fire2"))
        {
            if (gameObject.name.Contains("Chocolate"))
            {
                itemUse.HoldChocolate();
                Destroy(gameObject);
            }
            else if (gameObject.name.Contains("pillMatrix_0"))
            {
                gotPill = itemUse.HoldPill(0);
            }
            else if (gameObject.name.Contains("pillMatrix_1"))
            {
                gotPill = itemUse.HoldPill(1);
            }
            else if (gameObject.name.Contains("pillMatrix_2"))
            {
                gotPill = itemUse.HoldPill(2);
            }

            if (gotPill)
            {
                Destroy(gameObject);
            }
        }
    }
}
