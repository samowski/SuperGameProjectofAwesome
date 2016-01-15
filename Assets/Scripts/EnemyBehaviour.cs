using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public float MaxSpeed = 7;
	float viewRange = 50;

	public bool ShouldWalkItself = false;
	public bool IsUnconscious = false;
	public bool IsBlocking = false;

	float startX;

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

	EnemyBehaviour myBehaviour;

	public Transform ChasedObject;
	public Vector2 GrannyPos;
	public float DistanceToGranny;

	void Start()
	{
		if (gameObject.tag == "Police")
		{
			viewRange = 38;
		}

		myBehaviour = GetComponent<EnemyBehaviour>();

		anglesLArm = LArm.GetComponent<SimpleCCD>();
		anglesRArm = RArm.GetComponent<SimpleCCD>();
		anglesLFoot = LFoot.GetComponent<SimpleCCD>();
		anglesRFoot = RFoot.GetComponent<SimpleCCD>();

		startX = transform.localPosition.x;

		myRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		GrannyPos = ChasedObject.position;
		GrannyPos.y += 15;

		if (!IsUnconscious)
		{
			if (!IsBlocking)
			{
				calculateDistance();

				controlWalking();

				animator.SetFloat("Speed", Mathf.Abs(myRigidbody2D.velocity.x));

				if ((myRigidbody2D.velocity.x > 0 && !IsLookingRight) || (myRigidbody2D.velocity.x < 0 && IsLookingRight))
				{
					flip();
				}
			}
			else
			{
				myRigidbody2D.velocity = new Vector2(0, 0);
			}
		}
		else
		{
			myRigidbody2D.velocity = new Vector2(0, 0);
			animator.SetTrigger("Unconscious");
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

	void controlWalking()
	{
		if (Mathf.Abs(DistanceToGranny) < viewRange && Mathf.Abs(transform.position.x - GrannyPos.x) > 3)
		{
			chase();
		}
		else if (ShouldWalkItself)
		{
			if (transform.localPosition.x - startX > 20 || transform.localPosition.x - startX < -20)
			{
				flip();
			}

			walkInPattern(IsLookingRight);
		}
		else
		{
			myRigidbody2D.velocity = new Vector2(0, 0);
		}
	}

	void chase()
	{
		myRigidbody2D.velocity = new Vector2(DistanceToGranny < 0 ? MaxSpeed * 3 : -MaxSpeed * 3, myRigidbody2D.velocity.y);

		ShouldWalkItself = false;

		if (Mathf.Abs(DistanceToGranny) < 5)
		{
			animator.SetTrigger("Catch");

			myRigidbody2D.velocity = new Vector2(0, 0);

			ChasedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

			ChasedObject.GetComponent<PlayerController>().Busted();

			GetComponent<EnemyBehaviour>().enabled = false;

			ChasedObject.GetComponent<PlayerController>().enabled = false;
		}
	}

	void walkInPattern(bool direction)
	{
		myRigidbody2D.velocity = new Vector2(direction ? MaxSpeed : -MaxSpeed, myRigidbody2D.velocity.y);
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
			anglesLArm.angleLimits[0].max = 358;
			anglesRArm.angleLimits[0].min = 210;
			anglesRArm.angleLimits[0].max = 358;
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
			anglesLFoot.angleLimits[0].max = 358;
			anglesRFoot.angleLimits[0].min = 250;
			anglesRFoot.angleLimits[0].max = 358;
		}
	}

	public void SetUnconscious()
	{
		IsUnconscious = true;
		myBehaviour.enabled = false;
	}

	// called from Animation
	void SetBlocking()
	{
		IsBlocking = true;
	}

	void UnsetBlocking()
	{
		IsBlocking = false;
	}
	//
}