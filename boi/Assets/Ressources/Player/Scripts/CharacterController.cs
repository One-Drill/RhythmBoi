using System;
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
    public bool grounded; // Accessed from playerMovement, should find way to make private.
    private bool m_FacingRight = true;
    public float RunSpeed { get; set; }
    public float airSpeedMultiplier = 1.5f;
    private Transform collisions;
    public Transform sprite;
    private float slideSpeed = -1;
    private float beatDuration = 0;
    private bool spaceReleased = false;
    private float airTime = 0;
    public float shortHopDuration;
    public float fastStop;
    private float normalDeceleration;
    private bool canWallJump = false;
    private bool hasWallJumped = false;
    public float coyoteTime;
    private float coyoteTimeCounter = 0;
    public float bounceSpeed;
    public int Swapped { get; set; }
    public float pushBlockSpeed;
    private float baseRunDeceleration;

    private Transform rightCheck;
    private Transform leftCheck;


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
        grounded = false;
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
        //canWallJump = wallJumpCheck();
        MoveHorizontal(horizontalMovement * -Swapped);
        //horizontalMovement = slide(runSpeed * horizontalMovement, down);
        verticalMovement(hop, up, spaceWasReleased, wallJump);
        runDeceleration = baseRunDeceleration;
        playerCollisions.RectifyPosition();
    }

    private void MoveHorizontal(float horizontalMovement)
    {
        /*if (horizontalMovement > 0 && !m_FacingRight)
        {
            flip();
        }
        else if (horizontalMovement < 0 && m_FacingRight)
        {
            flip();
        }*/
        // MIGUEL IN : Setting up animator's param "Speed" with horizontalMovement to enable run animation if it is greater than 0!
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        // MIGUEL OUT
        if (horizontalMovement > 0)
            RunSpeed += runAcceleration * Time.deltaTime;
        else if (horizontalMovement < 0)
            RunSpeed -= runAcceleration * Time.deltaTime;
        if (horizontalMovement == 0 && Mathf.Abs(RunSpeed) <= 1)
            RunSpeed = 0;
        if (horizontalMovement == 0 && Mathf.Abs(RunSpeed) > 1)
            RunSpeed += (RunSpeed > 0 ? -runDeceleration : runDeceleration) * Time.deltaTime;
        RunSpeed = Mathf.Clamp(RunSpeed, -maxRunSpeed, maxRunSpeed);
        if (RunSpeed > 0)
        {
            RunSpeed *= horizontalMovement > 0.001 ? horizontalMovement : 1;
            horizontalMovement = CheckHorizontalRightCollision(RunSpeed, !grounded ? airSpeedMultiplier : 1);
        }
        else if (RunSpeed < 0)
        {
            RunSpeed *= horizontalMovement < -0.001 ? -horizontalMovement : 1;
            horizontalMovement = CheckHorizontalLeftCollision(RunSpeed, !grounded ? airSpeedMultiplier : 1);
        }
        m_Transform.Translate(new Vector3(horizontalMovement, 0));
    }

    private float CheckHorizontalRightCollision(float horizontalMovement, float multiplier)
    {
        return horizontalMovement * Time.deltaTime * multiplier;

        RaycastHit2D hitRight = Physics2D.Raycast(rightCheck.position, Swapped == 1? Vector2.left : Vector2.right);
        if (hitRight.collider == null)
        {
            return horizontalMovement * Time.deltaTime * multiplier;
        }
        float minDistance = multiplier * horizontalMovement * Time.deltaTime;
        minDistance = minDistance < hitRight.distance ? minDistance : hitRight.distance;
        return minDistance;
    }

    private float CheckHorizontalLeftCollision(float horizontalMovement, float multiplier)
    {
        return horizontalMovement * Time.deltaTime * multiplier;

        RaycastHit2D hitLeft = Physics2D.Raycast(leftCheck.position, Swapped == 1 ? Vector2.left : Vector2.right);
        if (hitLeft.collider == null)
        {
            return horizontalMovement * Time.deltaTime * multiplier;
        }
        float minDistance = Mathf.Abs(multiplier * horizontalMovement * Time.deltaTime);
        minDistance = minDistance < hitLeft.distance ? minDistance : hitLeft.distance;
        return - minDistance;
    }

    public void verticalMovement(bool hop, bool up, bool spaceWasReleased, bool wallJump)
    {
        animator.SetBool("Onair", grounded);
        
        if (grounded && hop)
        {
            grounded = false;
            spaceReleased = false;
            verticalVelocity = jumpSpeed;
        }
        if (spaceWasReleased)
            spaceReleased = true;


        RaycastHit2D hitFloor = Physics2D.Raycast(groundCheck.position, Swapped == 1 ? Vector2.up : Vector2.down);
        if (hitFloor.collider == null || hitFloor.distance > Mathf.Abs(verticalVelocity))
        {
            if (grounded)
                coyoteTimeCounter += Time.deltaTime;
        }
        if (coyoteTimeCounter > coyoteTime)
            grounded = false;
        jumpDeceleration = normalDeceleration;
        if (!grounded)
        {
            Jump();
        }
        m_Transform.Translate(new Vector3(0, verticalVelocity * Time.deltaTime));
    }

    void Jump()
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
                grounded = true;
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
        if (hitInfo.tag == "BOUNCE")
        {
            grounded = false;
            verticalVelocity = bounceSpeed;
        }
        if (hitInfo.tag == "GravSwap")
            hitInfo.gameObject.GetComponent<SwapGravity>().gravityShouldSwap = true;

    }

}
