using UnityEngine;
using System.Collections;

public class KidController : MonoBehaviour
{
    #region Variables
    public float MaxSpeed = 8;
    private float ViewRange = 105;
    public bool HasChocolate = false;

    public GameObject LArm;
    public GameObject RArm;
    public GameObject LFoot;
    public GameObject RFoot;
    private SimpleCCD AnglesLArm;
    private SimpleCCD AnglesRArm;
    private SimpleCCD AnglesLFoot;
    private SimpleCCD AnglesRFoot;

    [HideInInspector]
    public bool IsLookingRight = true;
    private Rigidbody2D myRigidbody2D;
    private Animator animator;

    public Transform ChasedObject;
    public float distanceToGranny;
    #endregion

    void Start()
    {
        AnglesLArm = LArm.GetComponent<SimpleCCD>();
        AnglesRArm = RArm.GetComponent<SimpleCCD>();
        AnglesLFoot = LFoot.GetComponent<SimpleCCD>();
        AnglesRFoot = RFoot.GetComponent<SimpleCCD>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (HasChocolate)
        {
            animator.SetBool("Grab", false);
            animator.SetBool("HasChocolate", true);
            ChasedObject.GetComponent<PlayerController>().MaxSpeed = 20;
        }
        else
        {
            if (animator.GetBool("Grab") == true)
            {
                ChasedObject.GetComponent<PlayerController>().MaxSpeed = 12;
                MaxSpeed = 4;
                ChasedObject.GetComponent<PlayerController>().IsSlowed = true;
                ChasedObject.GetComponent<ItemUse>().slowCollider = GetComponent<Collider2D>();
            }
            else
            {
                ChasedObject.GetComponent<PlayerController>().MaxSpeed = 20;
                MaxSpeed = 8;
                ChasedObject.GetComponent<PlayerController>().IsSlowed = false;
                ChasedObject.GetComponent<ItemUse>().slowCollider = null;

            }
        }
    }

    void FixedUpdate()
    {
        CalculateDistance();

        WalkController();

        animator.SetFloat("Speed", Mathf.Abs(myRigidbody2D.velocity.x));

        if ((myRigidbody2D.velocity.x > 0 && !IsLookingRight) || (myRigidbody2D.velocity.x < 0 && IsLookingRight))
        {
            Flip();
        }
    }

    void CalculateDistance()
    {
        if ((transform.position.x - ChasedObject.position.x) > 0)
        {
            distanceToGranny = Mathf.Sqrt(Mathf.Pow((transform.position.x - ChasedObject.position.x), 2) + Mathf.Pow((ChasedObject.position.y), 2));
        }
        else
        {
            distanceToGranny = -Mathf.Sqrt(Mathf.Pow((transform.position.x - ChasedObject.position.x), 2) + Mathf.Pow((ChasedObject.position.y), 2));
        }
    }

    void WalkController()
    {
        if (Mathf.Abs(distanceToGranny) < ViewRange && Mathf.Abs(distanceToGranny) > 2 && !HasChocolate)
        {
            Chasing();
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(0, 0);
        }
    }

    void Chasing()
    {
        myRigidbody2D.velocity = new Vector2(distanceToGranny < 0 ? MaxSpeed * 3 : -MaxSpeed * 3, myRigidbody2D.velocity.y);
        if (Mathf.Abs(distanceToGranny) < 5)
        {
            animator.SetBool("Grab", true);
        }
        else
        {
            animator.SetBool("Grab", false);
        }
    }
    void Flip()
    {
        IsLookingRight = !IsLookingRight;

        Vector3 myAngles = transform.localEulerAngles;
        myAngles.y += 180;
        transform.localEulerAngles = myAngles;

        if (!IsLookingRight)
        {
            AnglesLArm.angleLimits[0].min = 210;
            AnglesLArm.angleLimits[0].max = 360;
            AnglesRArm.angleLimits[0].min = 210;
            AnglesRArm.angleLimits[0].max = 360;
            AnglesLFoot.angleLimits[0].min = 0;
            AnglesLFoot.angleLimits[0].max = 90;
            AnglesRFoot.angleLimits[0].min = 0;
            AnglesRFoot.angleLimits[0].max = 90;
        }
        else
        {
            AnglesLArm.angleLimits[0].min = 0;
            AnglesLArm.angleLimits[0].max = 150;
            AnglesRArm.angleLimits[0].min = 0;
            AnglesRArm.angleLimits[0].max = 150;
            AnglesLFoot.angleLimits[0].min = 250;
            AnglesLFoot.angleLimits[0].max = 360;
            AnglesRFoot.angleLimits[0].min = 250;
            AnglesRFoot.angleLimits[0].max = 360;
        }
    }
}
