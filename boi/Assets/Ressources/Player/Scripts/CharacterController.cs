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
    public Transform leftDownCheck;
    public Transform leftUpCheck;
    public Transform rightDownCheck;
    public Transform rightUpCheck;
    public Transform leftMiddleCheck;
    public Transform rightMiddleCheck;
    public Transform groundCheck;
    public Transform headCheck;
    public float jumpSpeed;
    private FollowerOfTheRhythm tempo;
    public float verticalVelocity;
    public float jumpDeceleration;
    public float runAcceleration;
    public float runDeceleration;
    public float maxRunSpeed;
    public float maxFallSpeed;
    public float fallAcceleration;
    public bool grounded; // Accessed from playerMovement, should find way to make private.
    private bool m_FacingRight = true;
    public float runSpeed = 10f;
    public float airSpeedMultiplier = 1.5f;
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
    public float BounceSpeed;
    public int swapped = -1;
    public float pushBlockSpeed;
    private float baseRunDeceleration;

    // private bool falplat;

    //ABDUL START
    void Start()
    {
        baseRunDeceleration = runDeceleration;
        runSpeed = runSpeed / 120f * tempo.getBpm();
        jumpDeceleration = jumpDeceleration / 120f * tempo.getBpm();
        fallAcceleration = fallAcceleration / 120f * tempo.getBpm();
        maxFallSpeed = maxFallSpeed / 120f * tempo.getBpm();
        airSpeedMultiplier = airSpeedMultiplier / tempo.getBpm() * 120f;
        // RunSpeed adaptation to bpm
    }
    //ABDUL END

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
        if (swapped == 1)
            m_Transform.eulerAngles = new Vector3(0, 0, 180);
        else
            m_Transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void Move(float horizontalMovement, bool hop, bool up, bool down, bool spaceWasReleased, bool wallJump)
    {
        animator.SetFloat("Speed", horizontalMovement);
        canWallJump = wallJumpCheck();
        moveHorizontal(horizontalMovement * -swapped);
        //horizontalMovement = slide(runSpeed * horizontalMovement, down);
        verticalMovement(hop, up, spaceWasReleased, wallJump);
        runDeceleration = baseRunDeceleration;
    }

    public void moveHorizontal(float horizontalMovement)
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
            runSpeed += runAcceleration * Time.deltaTime;
        else if (horizontalMovement < 0)
            runSpeed -= runAcceleration * Time.deltaTime;
        if (horizontalMovement == 0 && Mathf.Abs(runSpeed) <= 1)
            runSpeed = 0;
        if (horizontalMovement == 0 && Mathf.Abs(runSpeed) > 1)
            runSpeed += (runSpeed > 0 ? -runDeceleration : runDeceleration) * Time.deltaTime;
        runSpeed = Mathf.Clamp(runSpeed, -maxRunSpeed, maxRunSpeed);
        if (runSpeed > 0)
        {
            runSpeed *= horizontalMovement > 0.001 ? horizontalMovement : 1;
            horizontalMovement = checkHorizontalRightCollision(runSpeed, !grounded ? airSpeedMultiplier : 1);
        }
        else if (runSpeed < 0)
        {
            runSpeed *= horizontalMovement < -0.001 ? -horizontalMovement : 1;
            horizontalMovement = checkHorizontalLeftCollision(runSpeed, !grounded ? airSpeedMultiplier : 1);
        }
        m_Transform.Translate(new Vector3(horizontalMovement, 0));
    }

    private float checkHorizontalRightCollision(float horizontalMovement, float multiplier)
    {
        RaycastHit2D upRight = Physics2D.Raycast(rightUpCheck.position, swapped == 1? Vector2.left : Vector2.right);
        RaycastHit2D middleRight = Physics2D.Raycast(rightMiddleCheck.position, swapped == 1? Vector2.left : Vector2.right);
        RaycastHit2D downRight = Physics2D.Raycast(rightDownCheck.position, swapped == 1? Vector2.left : Vector2.right);
        if (upRight.collider == null && middleRight.collider == null && downRight.collider == null)
        {
            return horizontalMovement * Time.deltaTime * multiplier;
        }
        float minDistance = multiplier * horizontalMovement * Time.deltaTime;
        if (upRight.collider != null)
            minDistance = minDistance < upRight.distance ? minDistance : upRight.distance;
        if (middleRight.collider != null)
        {
            minDistance = minDistance < middleRight.distance ? minDistance : middleRight.distance;
            if (middleRight.distance <= minDistance && middleRight.transform.gameObject.TryGetComponent<Box>(out Box box))
            {
                box.moveRight(pushBlockSpeed);
            }
        }
        if (downRight.collider != null)
            minDistance = minDistance < downRight.distance ? minDistance : downRight.distance;
        return minDistance;
    }

    private float checkHorizontalLeftCollision(float horizontalMovement, float multiplier)
    {
        RaycastHit2D upLeft = Physics2D.Raycast(leftUpCheck.position, swapped == 1 ? Vector2.right : Vector2.left);
        RaycastHit2D middleLeft = Physics2D.Raycast(leftMiddleCheck.position, swapped == 1 ? Vector2.right : Vector2.left);
        RaycastHit2D downLeft = Physics2D.Raycast(leftDownCheck.position, swapped == 1 ? Vector2.right : Vector2.left);
        if (upLeft.collider == null && middleLeft.collider == null && downLeft.collider == null)
        {
            return horizontalMovement * Time.deltaTime * multiplier;
        }
        float minDistance = Mathf.Abs(multiplier * horizontalMovement * Time.deltaTime);
        if (upLeft.collider != null)
            minDistance = minDistance < upLeft.distance ? minDistance : upLeft.distance;
        if (middleLeft.collider != null)
        { 
            minDistance = minDistance < middleLeft.distance ? minDistance : middleLeft.distance;
            if (middleLeft.distance <= minDistance && middleLeft.transform.gameObject.TryGetComponent<Box>(out Box box))
            {
                box.moveLeft(pushBlockSpeed);
            }
        }
        if (downLeft.collider != null)
            minDistance = minDistance < downLeft.distance ? minDistance : downLeft.distance;
        return minDistance * -1;
    }

    private bool wallJumpCheck()
    {
        RaycastHit2D middleRight = Physics2D.Raycast(rightMiddleCheck.position, swapped == 1? Vector2.left : Vector2.right);
        RaycastHit2D middleLeft = Physics2D.Raycast(leftMiddleCheck.position, swapped == 1? Vector2.right : Vector2.left);
        if (middleRight.collider == null && middleLeft.collider == null)
            return false;
        if (middleRight.distance > 0.7f && middleLeft.distance > 0.7f)
            return false;
        if (hasWallJumped || grounded)
            return false;
        return true;
    }
        private void flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void verticalMovement(bool hop, bool up, bool spaceWasReleased, bool wallJump)
    {
        // MIGUEL IN : stealing grounded param to know when to activate JUMP anim
        animator.SetBool("Onair", grounded);
        // MIGUEL OUT
        if (grounded && hop)
        {
            grounded = false;
            spaceReleased = false;
            verticalVelocity = jumpSpeed;
        }
        if (canWallJump && wallJump)
        {
            grounded = false;
            spaceReleased = false;
            verticalVelocity = jumpSpeed;
            hasWallJumped = true;
            canWallJump = false;
        }
        if (spaceWasReleased)
            spaceReleased = true;


        RaycastHit2D hitFloor = Physics2D.Raycast(groundCheck.position, swapped == 1 ? Vector2.up : Vector2.down);
        if (hitFloor.collider == null || hitFloor.distance > Mathf.Abs(verticalVelocity) + 0.1f)
        {
            grounded = false;
            //if (grounded)
            //    coyoteTimeCounter += Time.deltaTime;
        }
        //if (coyoteTimeCounter >= coyoteTime)
          //  grounded = false;
        jumpDeceleration = normalDeceleration;
        if (!grounded)
        {
            //if (canWallJump)
             //   wallGrabJump(hitFloor);
            //else
                jump(hitFloor);
        }
        m_Transform.Translate(new Vector3(0, verticalVelocity));
    }

    public float slide(float horizontalMovement, bool down)
    {
        if (down && beatDuration <= 0 && grounded)
        {
            slideSpeed = horizontalMovement * 2;
            beatDuration = 50 / tempo.getBpm();
            sprite.Translate(new Vector3(0, -0.1f, 0));
            sprite.Rotate(new Vector3(0, 0, -60));
        }
        if (beatDuration > 0)
        {
            beatDuration -= Time.deltaTime;
            if (beatDuration <= 0)
            {
                slideSpeed = -1;
                sprite.Rotate(new Vector3(0, 0, 60));
                sprite.Translate(new Vector3(0, 0.1f, 0));
            }
        }
        return slideSpeed == -1 ? horizontalMovement : slideSpeed;

    }

    void wallGrabJump(RaycastHit2D hitFloor)
    {
        airTime += Time.deltaTime;
        if (verticalVelocity > 0)
        {
            if (airTime >= shortHopDuration && spaceReleased)
                jumpDeceleration = fastStop;
            verticalVelocity -= jumpDeceleration * Time.deltaTime * 1.2f;
            RaycastHit2D hitCeil = Physics2D.Raycast(headCheck.position, Vector2.up);
            if (hitCeil.collider != null && hitCeil.distance < Mathf.Abs(verticalVelocity) + 0.1f)
            {
                m_Transform.Translate(new Vector3(0, hitCeil.distance));
                verticalVelocity = 0;
            }
        }
        if (verticalVelocity <= 0)
        {
            verticalVelocity -= fallAcceleration * Time.deltaTime / 5;
            if (verticalVelocity < -maxFallSpeed / 2)
                verticalVelocity = maxFallSpeed;
            hitFloor = Physics2D.Raycast(groundCheck.position, Vector2.down);
            if (hitFloor.collider != null && hitFloor.distance < Mathf.Abs(verticalVelocity) + 0.1f)
            {
                m_Transform.Translate(new Vector3(0, -hitFloor.distance));
                airTime = 0;
                verticalVelocity = 0;
                grounded = true;
                hasWallJumped = false;
                coyoteTimeCounter = 0;
                //WENDRUL START
                if (hitFloor.transform.gameObject.TryGetComponent<ActivatablePlatform>(out ActivatablePlatform platform))
                {
                    platform.isSteppedOn();
                }
                //WENDRUL OUT
                // ABDOUL IN
                //   if (Physics2D.Raycast(groundCheck.tag, Vector2.down) == "FallingPlatform")
                //       falplat = true;
                //ABDOUL OUT
            }
        }

    }

    void jump(RaycastHit2D hitFloor)
    {
        airTime += Time.deltaTime;
        if (verticalVelocity > 0)
        {
            if (airTime >= shortHopDuration && spaceReleased)
                jumpDeceleration = fastStop;
            verticalVelocity -= jumpDeceleration * Time.deltaTime;
            RaycastHit2D hitCeil = Physics2D.Raycast(headCheck.position, swapped == 1? Vector2.down : Vector2.up);
            if (hitCeil.collider != null && hitCeil.distance < Mathf.Abs(verticalVelocity) + 0.1f)
            {
                m_Transform.Translate(new Vector3(0, hitCeil.distance));
                verticalVelocity = 0;
                //if (hitCeil.transform.gameObject.TryGetComponent<SwapGravity>(out SwapGravity gravSwapper))
                 //   gravSwapper.gravityShouldSwap = true;
             // else if (hitCeil.transform.gameObject.TryGetComponent<TimedGravSwap>(out TimedGravSwap timedGravSwapper))
              //      timedGravSwapper.gravityShouldSwap = true;
            }
        }
        if (verticalVelocity <= 0)
        {
            verticalVelocity -= fallAcceleration * Time.deltaTime;
            if (verticalVelocity < -maxFallSpeed)
                verticalVelocity = maxFallSpeed;
            if (hitFloor.collider != null && hitFloor.distance < Mathf.Abs(verticalVelocity) + 0.1f)
            {
                m_Transform.Translate(new Vector3(0, -hitFloor.distance));
                airTime = 0;
                verticalVelocity = 0;
                coyoteTimeCounter = 0;
                grounded = true;
                hasWallJumped = false;
                //WENDRUL START
                if (hitFloor.transform.gameObject.TryGetComponent<ActivatablePlatform>(out ActivatablePlatform platform))
                {
                    platform.isSteppedOn();
                }
                //WENDRUL OUT
                // ABDOUL IN
                //   if (Physics2D.Raycast(groundCheck.tag, Vector2.down) == "FallingPlatform")
                //       falplat = true;
                //ABDOUL OUT
            }
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "BOUNCE")
        {
            grounded = false;
            verticalVelocity = BounceSpeed;
        }
        if (hitInfo.tag == "GravSwap")
            hitInfo.gameObject.GetComponent<SwapGravity>().gravityShouldSwap = true;

    }

}
