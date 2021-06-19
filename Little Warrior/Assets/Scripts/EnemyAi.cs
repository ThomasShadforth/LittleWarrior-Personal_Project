﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAi : MonoBehaviour
{
    [Header("General AI Properties")]
    public Transform target;
    public float activateDistance = 50f;
    public float speed = 5f;
    public float nextWaypoint = 3f;
    public float jumpNodeHeightReq = .5f;
    public float jumpMod = .3f;
    public float jumpCheckOffset = .1f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    [Header("Custom Behaviour")]
    public bool jumpEnabled;
    public bool FollowEnabled;
    public bool directionLook;

    public bool isGrounded;
    [SerializeField]
    Transform enemyFeet;

    public float overlapCheck = .4f;
    public LayerMask whatIsGround;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .25f);

        
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && TargetInDistance() && FollowEnabled)
        {
            seeker.StartPath(rb.position, target.position, onPathComplete);
        }
    }

    void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(FollowEnabled && TargetInDistance())
        {
            pathFollow();
        }
        

        
    }

    void pathFollow()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        isGrounded = Physics2D.OverlapCircle(enemyFeet.position, overlapCheck, whatIsGround);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (jumpEnabled && isGrounded)
        {
            if(direction.y > jumpNodeHeightReq)
            {
                rb.AddForce(Vector2.up * speed * jumpMod);
            }
        }

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypoint)
        {
            currentWaypoint++;
        }

        
    }

    bool TargetInDistance()
    {
        if(Vector2.Distance(rb.position, target.position) <= activateDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void enemyAngleAdjust(float zRotation)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zRotation);
    }
}
