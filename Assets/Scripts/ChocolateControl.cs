using UnityEngine;
using System.Collections;

public class ChocolateControl : MonoBehaviour
{
    public GameObject Granny;
    
    void FixedUpdate()
    {
        if (GetComponent<Collider2D>().IsTouching(Granny.GetComponent<Collider2D>()) && Input.GetButton("Fire2"))
        {
            Granny.SendMessage("HoldChocolate", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
