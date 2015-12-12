using UnityEngine;
using System.Collections;

public class LadderScript : MonoBehaviour
{
    private Rigidbody2D myridigdbody;
    public float ClimbSpeed = 30;

    void Start()
    {
        GetComponent<Collider2D>().IsTouchingLayers();
        myridigdbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myridigdbody.velocity = new Vector3(myridigdbody.velocity.x, 0.79f);
            if (Input.GetButton("Jump"))
            {
                myridigdbody.velocity = new Vector3(myridigdbody.velocity.x, ClimbSpeed, myridigdbody.velocity.y);
            }
            else
            {
                if(Input.GetButton("Fire2"))
                {
                    myridigdbody.velocity = new Vector3(myridigdbody.velocity.x, -ClimbSpeed, myridigdbody.velocity.y);
                }
            }
        }
    } 
}
