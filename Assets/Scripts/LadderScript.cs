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
        if (Input.GetButton("Jump") && GetComponent< Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myridigdbody.velocity = new Vector3(myridigdbody.velocity.x, ClimbSpeed, myridigdbody.velocity.y);
        }
    } 
}
