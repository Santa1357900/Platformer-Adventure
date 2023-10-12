using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flyingSpeed = 3f;
    public detectionZone spinZone;
    public float waypointReachedDistance = 0.1f;
    public Collider2D deathCollider;
    public List<Transform> waypoint;

    Animator animator;
    Rigidbody2D rb;
    damageable damageable;

    int waypointNum = 0;

    Transform nextWayponit;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<damageable>();
    }

    private void Start()
    {
        nextWayponit = waypoint[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }


    void Update()
    {
        HasTarget = spinZone.detectionCollider.Count > 0;
    }


    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (canMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        //fly to waypoint
        Vector2 directionToWaypoint = (nextWayponit.position - transform.position).normalized;

        //check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWayponit.position, transform.position);

        rb.velocity = directionToWaypoint * flyingSpeed;
        UpdateDirection();

        //see if we need to switch waypoint
        if(distance <= waypointReachedDistance)
        {
            waypointNum++;

            if(waypointNum >= waypoint.Count)
            {
                waypointNum = 0;
            }

            nextWayponit = waypoint[waypointNum];
        }
    }

    public void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if(transform.localScale.x > 0)
        {
            //facing right
            if(rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            //facing left
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }


    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }

}
