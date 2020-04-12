﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // HI, giving animator here to get anims on point
    public Animator animator;
    //
    private Transform m_Transform;
    private BoxCollider2D m_BoxCollider;
    public Transform groundCheck;
    public Transform headCheck;
    public float jumpSpeed;
    private FollowerOfTheRhythm tempo;
    private PlayerCollisions playerCollisions;
    public float verticalVelocity;
    public float jumpDeceleration;
    public float runAcceleration;
    public float runDeceleration;
    public float maxRunSpeed;
    public float maxFallSpeed;
    public float fallAcceleration;
    public bool Grounded { get; private set; } // Accessed from playerMovement, should find way to make private.
    public float RunSpeed { get; set; }
    public float airSpeedMultiplier = 1.5f;
    private Transform collisions;
    public Transform sprite;
    private bool spaceReleased = false;
    private float airTime = 0;
    public float shortHopDuration;
    public float fastStop;
    private float normalDeceleration;
    public float coyoteTime;
    private float coyoteTimeCounter = 0;
    public float bounceSpeed;
    public int Swapped { get; set; }
    public float pushBlockSpeed;
    private float baseRunDeceleration;

    private Transform rightCheck;
    private Transform leftCheck;
    private float joystick;

    void Start()
    {
        Swapped = -1;
        playerCollisions = GetComponent<PlayerCollisions>();
        baseRunDeceleration = runDeceleration;
        RunSpeed = RunSpeed / 120f * tempo.getBpm();
        jumpDeceleration = jumpDeceleration / 120f * tempo.getBpm();
        fallAcceleration = fallAcceleration / 120f * tempo.getBpm();
        maxFallSpeed = maxFallSpeed / 120f * tempo.getBpm();
        airSpeedMultiplier = airSpeedMultiplier / tempo.getBpm() * 120f;
        
        collisions = m_Transform.Find("Collisions");
        rightCheck = collisions.Find("rightCheck");
        leftCheck = collisions.Find("leftCheck");
        // RunSpeed adaptation to bpm
    }

    void Awake()
    {
        normalDeceleration = jumpDeceleration;
        Grounded = false;
        verticalVelocity = 0;
        tempo = GetComponent<FollowerOfTheRhythm>();
        m_Transform = GetComponent<Transform>();
        m_BoxCollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        if (Swapped == 1)
            m_Transform.eulerAngles = new Vector3(0, 0, 180);
        else
            m_Transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void Move(float horizontalMovement, bool hop, bool up, bool down, bool spaceWasReleased, bool wallJump)
    {
        animator.SetFloat("Speed", horizontalMovement);
        MoveHorizontal(horizontalMovement * -Swapped);
        VerticalMovement(hop, up, spaceWasReleased, wallJump);
        runDeceleration = baseRunDeceleration;
        playerCollisions.RectifyPosition();
    }

    private bool AimForSpeed(float speed, float acceleration)
    {
        if (RunSpeed > speed)
        {
            RunSpeed -= acceleration * Time.deltaTime;
            if (RunSpeed <= speed)
            {
                RunSpeed = speed;
                return true;
            }
        }
        else
        {
            RunSpeed += acceleration * Time.deltaTime;
            if (RunSpeed > speed)
            {
                RunSpeed = speed;
                return true;
            }
        }
        return false;
    }

    private void MoveHorizontal(float horizontalMovement)
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));

        if (horizontalMovement == 0)
        {
            AimForSpeed(Mathf.Lerp(-maxRunSpeed, maxRunSpeed, (horizontalMovement / 2) + 0.5f), runDeceleration);
        }
        else
        {
            AimForSpeed(Mathf.Lerp(-maxRunSpeed, maxRunSpeed, (horizontalMovement / 2) + 0.5f), runAcceleration);
        }

        if (playerCollisions.ShouldSnapHorizontaly(RunSpeed * Time.deltaTime, out float distanceToSnap))
        {
            m_Transform.Translate(new Vector3(distanceToSnap, 0));
            RunSpeed = 0;
        }
        else
        {
            m_Transform.Translate(new Vector3(RunSpeed * Time.deltaTime, 0));
        }
    }

    public void VerticalMovement(bool hop, bool up, bool spaceWasReleased, bool wallJump)
    {
        animator.SetBool("Onair", Grounded);
        
        if (Grounded && hop)
        {
            Grounded = false;
            spaceReleased = false;
            verticalVelocity = jumpSpeed;
        }
        if (spaceWasReleased)
            spaceReleased = true;


        RaycastHit2D hitFloor = Physics2D.Raycast(groundCheck.position, Swapped == 1 ? Vector2.up : Vector2.down);
        if (hitFloor.collider == null || hitFloor.distance > Mathf.Abs(verticalVelocity))
        {
            if (Grounded)
                coyoteTimeCounter += Time.deltaTime;
        }
        if (coyoteTimeCounter > coyoteTime)
            Grounded = false;
        jumpDeceleration = normalDeceleration;
        if (!Grounded)
        {
            Jump();
        }
        m_Transform.Translate(new Vector3(0, verticalVelocity * Time.deltaTime));
    }

    private void Jump()
    {
        airTime += Time.deltaTime;
        if (verticalVelocity > 0)
        {
            if (airTime >= shortHopDuration && spaceReleased)
            {
                jumpDeceleration = fastStop;
            }
            verticalVelocity -= jumpDeceleration * Time.deltaTime;
            if (playerCollisions.ShouldSnapToCeiling(verticalVelocity, out float distanceToSnap))
            {
                m_Transform.Translate(new Vector3(0, distanceToSnap));
                verticalVelocity = 0;
            }
        }
        if (verticalVelocity <= 0)
        {
            verticalVelocity -= fallAcceleration * Time.deltaTime;
            if (verticalVelocity < -maxFallSpeed)
            {
                verticalVelocity = -maxFallSpeed;
            }
            if (playerCollisions.ShouldSnapToGround(verticalVelocity, out float distanceToSnap, out RaycastHit2D hitGround))
            {
                m_Transform.Translate(Vector2.down * distanceToSnap);
                verticalVelocity = 0;
                Grounded = true;
                OnLanding(hitGround);
            }
        }
    }

    public void OnLanding(RaycastHit2D hitGround)
    {
        coyoteTimeCounter = 0;
        airTime = 0;
        if (hitGround.transform.gameObject.TryGetComponent<ActivatablePlatform>(out ActivatablePlatform platform))
        {
            platform.isSteppedOn();
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("BOUNCE"))
        {
            Grounded = false;
            verticalVelocity = bounceSpeed;
        }
        if (hitInfo.CompareTag("GravSwap"))
            hitInfo.gameObject.GetComponent<SwapGravity>().gravityShouldSwap = true;

    }

}
