﻿using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{
    public float MaxSpeed = 9;
    public float CurrentSpeed;

    public bool ShouldWalkItself = false;
    public bool IsUnconscious = false;
    public bool IsBlocking = false;

    public float StartX;

    public GameObject LArm;
    public GameObject RArm;
    public GameObject LFoot;
    public GameObject RFoot;
    public SimpleCCD AnglesLArm;
    public SimpleCCD AnglesRArm;
    public SimpleCCD AnglesLFoot;
    public SimpleCCD AnglesRFoot;

    [HideInInspector]
    public bool IsLookingRight = true;
    private Rigidbody2D myRigidbody2D;
    private Animator animator;

    public Transform ChasedObject;
    public float distanceToGranny;

    void Start()
    {
        AnglesLArm = LArm.GetComponent<SimpleCCD>();
        AnglesRArm = RArm.GetComponent<SimpleCCD>();
        AnglesLFoot = LFoot.GetComponent<SimpleCCD>();
        AnglesRFoot = RFoot.GetComponent<SimpleCCD>();
        StartX = transform.localPosition.x;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {

        if (!IsUnconscious)
        {
            if (!IsBlocking)
            {
                CalculateDistance();

                WalkController();

                animator.SetFloat("Speed", Mathf.Abs(myRigidbody2D.velocity.x));

                if ((myRigidbody2D.velocity.x > 0 && !IsLookingRight) || (myRigidbody2D.velocity.x < 0 && IsLookingRight))
                {
                    Flip();
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
        CurrentSpeed = myRigidbody2D.velocity.x;
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
        if (Mathf.Abs(distanceToGranny) < 35)
        {
            Chasing();
        }
        else if (ShouldWalkItself)
        {
            if (transform.localPosition.x - StartX > 20 || transform.localPosition.x - StartX < -20)
            {
                Flip();
            }
            WalkInPattern(IsLookingRight);
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(0, 0);
        }
    }

    void Chasing()
    {
        myRigidbody2D.velocity = new Vector2(distanceToGranny < 0 ? MaxSpeed * 3 : -MaxSpeed * 3, myRigidbody2D.velocity.y);
        ShouldWalkItself = false;
        if (Mathf.Abs(distanceToGranny) < 5)
        {
            animator.SetTrigger("Catch");
        }
    }

    void WalkInPattern(bool direction)
    {
        myRigidbody2D.velocity = new Vector2(direction ? MaxSpeed : -MaxSpeed, myRigidbody2D.velocity.y);
    }

    void Flip()
    {
        IsLookingRight = !IsLookingRight;

		/*Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;*/

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

    void SetUnconscious()
    {
        IsUnconscious = true;
    }

    void SetBlocking()
    {
        IsBlocking = true;
    }

    void UnsetBlocking()
    {
        IsBlocking = false;
    }
}