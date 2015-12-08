using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //Velocity
    public float MaxSpeed = 4;
    public float JumpForce = 550;

    //GoundDetection
    public Transform GroundCheck;
    public LayerMask WhatIsGround;
    private bool isGrounded = false;

    //Body, Animator and LookDirection
    [HideInInspector]
    public bool IsLookingRight = true;
    private Rigidbody2D myRigidbody2D;
    private Animator animator;

    private bool shouldAttack = false;
    private bool shouldJump = false;

    public static bool IsAttacking = false;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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

    void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");

        myRigidbody2D.velocity = new Vector2(hor * MaxSpeed, myRigidbody2D.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(hor));

        //GroundDetection of the Groundlayer(WhatIsGround) at the position of GroundCheck with a radius of 0.15
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, WhatIsGround);

        animator.SetBool("IsGrounded", isGrounded);

        if ((hor > 0 && !IsLookingRight) || (hor < 0 && IsLookingRight))
        {
            Flip();
        }

        if (shouldJump)
        {
            myRigidbody2D.AddForce(new Vector2(0, JumpForce));
            shouldJump = false;
        }

        if (shouldAttack)
        {
            animator.SetTrigger("Punch");
            shouldAttack = false;
            //in order to create bullets etc: GameObject bullet = (GameObject) Instantiate(bulletPrefab, spawnPoint.position, Quaternion,identity);
            //bullet.GetComponentyRigidbody2D>().AddForce(Vector3.right * bulletSpeed); -- bulletSpeed is a new float-variable
        }
    }

    void Flip()
    {
        IsLookingRight = !IsLookingRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }
	
	void ChangeAttackTrue()
	{
		IsAttacking = true;
	}
	void ChangeAttackFalse()
	{
    Debug.Log("Ist hier drin");
		IsAttacking = false;
	}
}
