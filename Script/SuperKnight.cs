using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(damageableSuperEnemy))]
public class SuperKnight : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float maxWalkSpeed = 3f;
    public float walkStopRate = 1f;
    public detectionZone attackZone;
    public detectionZone cliffDetectionZone;

    Rigidbody2D rb;
    Animator animator;
    damageableSuperEnemy damageable;
    public enum WalkAbleDirection { Right, Left }

    public WalkAbleDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    TouchingDirection touchingDirection;

    public WalkAbleDirection walkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkAbleDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkAbleDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.AttackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.AttackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<damageableSuperEnemy>();
    }

    void Update()
    {
        HasTarget = attackZone.detectionCollider.Count > 0;

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        if (touchingDirection.IsGrounded && touchingDirection.IsOnWall)
        {
            FlipDirection();
        }
        if (!damageable.lockVelocity)
        {
            if (canMove)

                rb.velocity = new Vector2(
                         Mathf.Clamp(rb.velocity.x + (walkSpeed * walkDirectionVector.x * Time.fixedDeltaTime), -maxWalkSpeed, maxWalkSpeed),
                         rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }


    private void FlipDirection()
    {
        if (walkDirection == WalkAbleDirection.Right)
        {
            walkDirection = WalkAbleDirection.Left;
        }
        else if (walkDirection == WalkAbleDirection.Left)
        {
            walkDirection = WalkAbleDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }
    public void onHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
    }

}
