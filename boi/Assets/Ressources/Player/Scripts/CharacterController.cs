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
        verticalMovement(hop, up, spaceWasReleased, wallJump);
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

        if (RunSpeed > 0)
        {
            //RunSpeed *= horizontalMovement > 0.001 ? horizontalMovement : 1;
            horizontalMovement = CheckHorizontalRightCollision(RunSpeed, !Grounded ? airSpeedMultiplier : 1);
        }
        else if (RunSpeed < 0)
        {
            //RunSpeed *= horizontalMovement < -0.001 ? -horizontalMovement : 1;
            horizontalMovement = CheckHorizontalLeftCollision(RunSpeed, !Grounded ? airSpeedMultiplier : 1);
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
        if (hitInfo.tag == "BOUNCE")
        {
            Grounded = false;
            verticalVelocity = bounceSpeed;
        }
        if (hitInfo.tag == "GravSwap")
            hitInfo.gameObject.GetComponent<SwapGravity>().gravityShouldSwap = true;

    }

}
