using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject BustedPrefab;
    private Vector3 BustedPrefabPosition;

    //Velocity
    public float CurrentSpeed = 30;
    float fixedSpeed = 30;
    public float JumpForce = 3000;
    public bool IsSlowed = false;

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

    public bool isControllable = true;
    static uint helper= 1;

    LevelMenu levelMenu;
    SmoothPitchChanger smoothPitchChanger;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        helper = 1;
        animator.SetBool("Dying", false);

        var go = GameObject.Find("LevelMenu");
        if (go != null) levelMenu = go.GetComponent<LevelMenu>();

        go = GameObject.Find("Options");
        if (go != null) smoothPitchChanger = go.GetComponent<SmoothPitchChanger>();
    }

    void Update()
    {
        if (isControllable)
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
        if (isControllable)
        {
            float hor = Input.GetAxis("Horizontal");

			myRigidbody2D.velocity = new Vector2(hor * CurrentSpeed, myRigidbody2D.velocity.y);
           
            animator.SetFloat("Speed", Mathf.Abs(hor));

            if ((hor > 0 && !IsLookingRight) || (hor < 0 && IsLookingRight))
            {
                Flip();
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

    void Flip()
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
        Debug.Log("fhfh");
        if (speed <= fixedSpeed)
        {
            if (smoothPitchChanger != null) smoothPitchChanger.SetPitch(0.7f, 2.0f);
        }
        else
        {
            if (smoothPitchChanger != null) smoothPitchChanger.SetPitch(1.2f, 2.0f);
        }
        fixedSpeed = speed;
        Invoke("SetNormalSpeed", 6);
    }

    public void SetNormalSpeed()
    {

        if (smoothPitchChanger != null) smoothPitchChanger.SetPitch(1.0f, 0.5f);
        fixedSpeed = 30;
    }

    public void ApplyGrannyDamage()
    {
        BustedPrefabPosition = gameObject.transform.position;
        BustedPrefabPosition.y += 60;
        myRigidbody2D.velocity = new Vector2(0, 0);

        animator.SetBool("Dying", true);
        animator.SetFloat("Speed", 0);

        Invoke("Busted", 1);

        GetComponent<LadderScript>().enabled = false;       //Kann immer noch Leitern hochlaufen nach dem fangen!!! Trotz dieser Zeile!
        GetComponent<PlayerController>().enabled = false;
    }


    public void Busted()
    {
        if (helper > 0)
        {
            helper--;
            if (levelMenu != null) levelMenu.EnterLevelFailed();
        }
    }
}
