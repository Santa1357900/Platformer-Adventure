using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;

    Vector2 moveInput;

    TouchingDirection touchingDirection;
    damageable damageable;
    damageableSuperEnemy damageableSuperEnemy;
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirection.IsOnWall)
                {
                    if (touchingDirection.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {   //air move
                        return airWalkSpeed;
                    }

                }
                else
                {
                    //idle speed
                    return 0;
                }
            }
            else
            {   
                //Movement Locked
                return 0;
            }


        }
    
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }



    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }
    public bool _IsFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return _IsFacingRight;
        }
        private set
        {   
            if (_IsFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _IsFacingRight = value;
        }
    }

    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } 
    }

    public bool isAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

 

    Rigidbody2D rb;
    Animator animator;
 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        damageable = GetComponent<damageable>();
        damageableSuperEnemy = GetComponent<damageableSuperEnemy>();
    }

 

    private void FixedUpdate()
    {   
        if(!damageable.lockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        if (!damageableSuperEnemy.lockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (isAlive )
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 &&!IsFacingRight)
        {
            //right
            IsFacingRight = true;
        }else if (moveInput.x < 0 && IsFacingRight)
        {
            //left
            IsFacingRight= false;
        }
    }

    public void onRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
           IsRunning = true;
        }else if(context.canceled)
        {
            IsRunning= false;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
      
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void onHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }



}
