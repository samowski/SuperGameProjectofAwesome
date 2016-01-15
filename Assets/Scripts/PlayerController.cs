using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Velocity
	public float CurrentSpeed = 30;
	float fixedSpeed = 30;
	public float JumpForce = 3000;
	public bool IsSlowed = false;

	//GoundDetection
	public Transform GroundCheck;
	public LayerMask WhatIsGround;
	bool isGrounded = false;

	//Body, Animator and LookDirection
	[HideInInspector]
	public bool IsLookingRight = true;

	Rigidbody2D myRigidbody2D;
	Animator animator;

	bool shouldAttack = false;
	bool shouldJump = false;

	public static bool IsAttacking = false;

	public bool IsControllable = true;

	bool isBusted = false;

	LevelMenu levelMenu;
	SmoothPitchChanger smoothPitchChanger;

	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		animator.SetBool("Dying", false);

		var gameObject = GameObject.Find("LevelMenu");

		if (gameObject != null)
		{
			levelMenu = gameObject.GetComponent<LevelMenu>();
		}

		gameObject = GameObject.Find("Options");

		if (gameObject != null)
		{
			smoothPitchChanger = gameObject.GetComponent<SmoothPitchChanger>();
		}
	}

	void Update()
	{
		if (IsControllable)
		{
			if (Input.GetButtonDown("Jump") && isGrounded)
			{
				shouldJump = true;
			}

			if (Input.GetButtonDown("Fire1"))
			{
				shouldAttack = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Finish"))
		{
			levelMenu.EnterLevelFinished();
		}
	}

	void FixedUpdate()
	{
		if (IsControllable)
		{
			float horizontalAxis = Input.GetAxis("Horizontal");

			myRigidbody2D.velocity = new Vector2(horizontalAxis * CurrentSpeed, myRigidbody2D.velocity.y);

			animator.SetFloat("Speed", Mathf.Abs(horizontalAxis));

			if ((horizontalAxis > 0 && !IsLookingRight) || (horizontalAxis < 0 && IsLookingRight))
			{
				flip();
			}
		}

		//GroundDetection of the Groundlayer(WhatIsGround) at the position of GroundCheck with a radius of 0.15
		isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, WhatIsGround);

		animator.SetBool("IsGrounded", isGrounded);

		if (shouldJump)
		{
			myRigidbody2D.AddForce(new Vector2(0, JumpForce));
			shouldJump = false;
		}

		if (shouldAttack)
		{
			animator.SetTrigger("Punch");
			shouldAttack = false;
		}
	}

	void flip()
	{
		IsLookingRight = !IsLookingRight;

		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}

	// called from animation
	void ChangeAttackTrue()
	{
		IsAttacking = true;
	}

	void ChangeAttackFalse()
	{
		IsAttacking = false;
	}
	//

	public void CalculateSlow(int amountOfSlows, GameObject kid)
	{
		CurrentSpeed = fixedSpeed;
		IsSlowed = false;
		JumpForce = 3000;

		if (amountOfSlows > 1)
		{
			CurrentSpeed = 12;
			IsSlowed = true;
			JumpForce = 1500;
		}
		else
		{
			if (amountOfSlows == 1)
			{
				CurrentSpeed = 12;
				IsSlowed = true;
				JumpForce = 3000;
			}
		}
	}

	public void SetPillSpeed(int speed)
	{
		if (speed <= fixedSpeed)
		{
			if (smoothPitchChanger != null)
			{
				smoothPitchChanger.SetPitch(0.7f, 2.0f);
			}
		}
		else
		{
			if (smoothPitchChanger != null)
			{
				smoothPitchChanger.SetPitch(1.2f, 2.0f);
			}
		}

		fixedSpeed = speed;
		Invoke("SetNormalSpeed", 6);
	}

	public void SetNormalSpeed()
	{
		if (smoothPitchChanger != null)
		{
			smoothPitchChanger.SetPitch(1.0f, 0.5f);
		}

		fixedSpeed = 30;
	}

	public void ApplyGrannyDamage()
	{
		myRigidbody2D.velocity = new Vector2(0, 0);

		animator.SetBool("Dying", true);
		animator.SetFloat("Speed", 0);

		Invoke("Busted", 1);

		GetComponent<LadderScript>().enabled = false;
		GetComponent<PlayerController>().enabled = false;
	}

	public void Busted()
	{
		if (!isBusted)
		{
			isBusted = true;

			if (levelMenu != null)
			{
				levelMenu.EnterLevelFailed();
			}
		}
	}
}