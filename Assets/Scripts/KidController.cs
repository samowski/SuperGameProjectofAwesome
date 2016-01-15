using UnityEngine;

public class KidController : MonoBehaviour
{
	public float MaxSpeed = 11;
	public float ViewRange = 105;
	public bool HasChocolate = false;

	public static int AmountOfKids = 0;
	public static int AmountOfSlows = 0;
	public static int AmountOfRunthroughs = 0;

	public GameObject LArm;
	public GameObject RArm;
	public GameObject LFoot;
	public GameObject RFoot;

	SimpleCCD anglesLArm;
	SimpleCCD anglesRArm;
	SimpleCCD anglesLFoot;
	SimpleCCD anglesRFoot;

	[HideInInspector]
	public bool IsLookingRight = true;

	Rigidbody2D myRigidbody2D;
	Animator animator;

	PlayerController chasedController;

	public Transform ChasedObject;
	public Vector2 GrannyPos;
	public float DistanceToGranny;

	void Start()
	{
		AmountOfKids++;

		anglesLArm = LArm.GetComponent<SimpleCCD>();
		anglesRArm = RArm.GetComponent<SimpleCCD>();
		anglesLFoot = LFoot.GetComponent<SimpleCCD>();
		anglesRFoot = RFoot.GetComponent<SimpleCCD>();

		myRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		chasedController = ChasedObject.GetComponent<PlayerController>();
	}

	void FixedUpdate()
	{
		GrannyPos = ChasedObject.position;
		GrannyPos.y += 8.2f; 

		AmountOfRunthroughs++;

		//HoldsChocolate -> Stop
		if (HasChocolate)
		{
			animator.SetBool("Grab", false);
			animator.SetBool("HasChocolate", true);
			animator.SetFloat("Speed", 0);
			myRigidbody2D.velocity = new Vector2(0, 0);
		}
		else
		{
			//Walking
			controlGrabbing();
			calculateDistance();
			controlWalking();

			animator.SetFloat("Speed", Mathf.Abs(myRigidbody2D.velocity.x));
			if ((myRigidbody2D.velocity.x > 0 && !IsLookingRight) || (myRigidbody2D.velocity.x < 0 && IsLookingRight))
			{
				flip();
			}
		}
		//Slowing
		chasedController.CalculateSlow(AmountOfSlows, gameObject);

		//Reset per TimeIntervall
		if (AmountOfRunthroughs == AmountOfKids)
		{
			AmountOfSlows = 0;
			AmountOfRunthroughs = 0;
		}
	}

	void controlWalking()
	{
		if (Mathf.Abs(DistanceToGranny) < ViewRange && Mathf.Abs(transform.position.x - GrannyPos.x) > 2 && !HasChocolate)
		{
			myRigidbody2D.velocity = new Vector2(DistanceToGranny < 0 ? MaxSpeed * 3 : -MaxSpeed * 3, myRigidbody2D.velocity.y);
		}
		else
		{
			myRigidbody2D.velocity = new Vector2(0, 0);
		}
	}

	void controlGrabbing()
	{
		if (Mathf.Abs(DistanceToGranny) < 5)
		{
			animator.SetBool("Grab", true);

			MaxSpeed = 4;

			AmountOfSlows++;

			ChasedObject.GetComponent<ItemUse>().SlowCollider = GetComponent<Collider2D>();
		}
		else
		{
			animator.SetBool("Grab", false);

			MaxSpeed = 11;
		}
	}

	void calculateDistance()
	{
		if ((transform.position.x - GrannyPos.x) > 0)
		{
			DistanceToGranny = Mathf.Sqrt(Mathf.Pow((transform.position.x - GrannyPos.x), 2) + Mathf.Pow((transform.position.y - GrannyPos.y), 2));
		}
		else
		{
			DistanceToGranny = -Mathf.Sqrt(Mathf.Pow((transform.position.x - GrannyPos.x), 2) + Mathf.Pow((transform.position.y - GrannyPos.y), 2));
		}
	}

	void flip()
	{
		IsLookingRight = !IsLookingRight;

		Vector3 myAngles = transform.localEulerAngles;
		myAngles.y += 180;
		transform.localEulerAngles = myAngles;

		if (!IsLookingRight)
		{
			anglesLArm.angleLimits[0].min = 210;
			anglesLArm.angleLimits[0].max = 360;
			anglesRArm.angleLimits[0].min = 210;
			anglesRArm.angleLimits[0].max = 360;
			anglesLFoot.angleLimits[0].min = 1;
			anglesLFoot.angleLimits[0].max = 90;
			anglesRFoot.angleLimits[0].min = 1;
			anglesRFoot.angleLimits[0].max = 90;
		}
		else
		{
			anglesLArm.angleLimits[0].min = 1;
			anglesLArm.angleLimits[0].max = 150;
			anglesRArm.angleLimits[0].min = 1;
			anglesRArm.angleLimits[0].max = 150;
			anglesLFoot.angleLimits[0].min = 250;
			anglesLFoot.angleLimits[0].max = 360;
			anglesRFoot.angleLimits[0].min = 250;
			anglesRFoot.angleLimits[0].max = 360;
		}
	}
}