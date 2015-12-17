using UnityEngine;
using System.Collections;

public class PickupControl : MonoBehaviour
{
    public GameObject Granny;

    void Start()
    {
        Granny = GameObject.Find("Granny");
    }
    void FixedUpdate()
    {
        if (GetComponent<Collider2D>().IsTouching(Granny.GetComponent<Collider2D>()) && Input.GetButton("Fire2") && ItemUse.PickupCooldown == 0)
        {
            if (gameObject.name.Contains("Chocolate"))
            {
                Granny.SendMessage("HoldChocolate", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (gameObject.name.Contains("pillMatrix_0"))
                {
                    Granny.SendMessage("HoldPill", 0, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (gameObject.name.Contains("pillMatrix_1"))
                    {
                        Granny.SendMessage("HoldPill", 1, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        if (gameObject.name.Contains("pillMatrix_2"))
                        {
                            Granny.SendMessage("HoldPill", 2, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}
