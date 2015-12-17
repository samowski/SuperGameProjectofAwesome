using UnityEngine;
using System.Collections;

public class LadderScript : MonoBehaviour
{
    public float ClimbSpeed = 30;

    Rigidbody2D ridigdbody;
    Collider2D grannyCollider;

    void Start()
    {
        ridigdbody = GetComponent<Rigidbody2D>();
        grannyCollider = GetComponent<Collider2D>();
    }
    void FixedUpdate()
    {
        if (grannyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            ridigdbody.velocity = new Vector3(ridigdbody.velocity.x, 1,5f);
            if (Input.GetButton("Jump"))
            {
                ridigdbody.velocity = new Vector3(ridigdbody.velocity.x, ClimbSpeed, ridigdbody.velocity.y);
            }
            else
            {
                if(Input.GetButton("Fire2"))
                {
                    ridigdbody.velocity = new Vector3(ridigdbody.velocity.x, -ClimbSpeed, ridigdbody.velocity.y);
                }
            }
        }
    } 
}
