using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private Transform groundCheck;
    private Transform ceilCheck;

    private Transform rightCheck;
    private Transform leftCheck;
    private Transform knee;
    private Transform head;

    private Transform collisions;
    private Transform m_Transform;
    private Transform center;
    private CharacterController controller;

    //todo Add swapped on the collisions
    void Start()
    {
        m_Transform = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        collisions = m_Transform.Find("Collisions");
        groundCheck = collisions.Find("groundCheck");
        ceilCheck = collisions.Find("ceilCheck");

        rightCheck = collisions.Find("rightCheck");
        leftCheck = collisions.Find("leftCheck");
        head = collisions.Find("head");
        knee = collisions.Find("knee");
        center = collisions.Find("center");
        center.position = new Vector3((rightCheck.position.x + leftCheck.position.x) / 2, (head.position.y + knee.position.y) / 2);
    }

    void Update()
    {
        
    }

    public void RectifyPosition()
    {
        Horizontal();
        Vertical();
    }

    private void Vertical()
    {
        float heightUp = Mathf.Abs(ceilCheck.position.y - center.position.y);
        float heightDown = Mathf.Abs(center.position.y - groundCheck.position.y);
        float verticalCorrection = RayCollisionDetection(Vector2.down, heightDown, center);
        if (verticalCorrection == 0)
        {
            verticalCorrection -= RayCollisionDetection(Vector2.up, heightUp, center);
        }
        m_Transform.Translate(new Vector3(0, verticalCorrection));
    }

    private void Horizontal()
    {
        float rightWidth = Mathf.Abs(center.position.x - rightCheck.position.x);
        float leftWidth = Mathf.Abs(center.position.x - leftCheck.position.x);

        float horizontalCorrection = - Mathf.Max(RayCollisionDetection(Vector2.right, rightWidth, head), RayCollisionDetection(Vector2.right, rightWidth, knee));
        horizontalCorrection += Mathf.Max(RayCollisionDetection(Vector2.left, leftWidth, head), RayCollisionDetection(Vector2.left, leftWidth, knee));
        if (horizontalCorrection != 0)
        {
            controller.RunSpeed = 0;
        }
        m_Transform.Translate(new Vector3(horizontalCorrection, 0));

    }

    private float RayCollisionDetection(Vector2 direction, float distance, Transform startingPoint)
    {
        RaycastHit2D hitPoint = Physics2D.Raycast(startingPoint.position, direction);
        if (hitPoint.collider != null && hitPoint.distance < distance + 0.01f)
        {
            return distance - hitPoint.distance;
        }
        return 0;
    }

    public bool ShouldSnapToCeiling(float verticalVelocity, out float distanceToSnap)
    {
        Vector3 centeredCeilCheck = new Vector3(center.position.x, ceilCheck.position.y);

        RaycastHit2D hitCeil = Physics2D.Raycast(centeredCeilCheck, controller.Swapped == 1 ? Vector2.down : Vector2.up);
        if (hitCeil.collider != null && hitCeil.distance < Mathf.Abs(verticalVelocity * Time.deltaTime) + 0.001f)
        {
            distanceToSnap = hitCeil.distance;
            return true;
        }
        distanceToSnap = 0;
        return false;
    }

    public bool ShouldSnapToGround(float verticalVelocity, out float distanceToSnap, out RaycastHit2D hitGround)
    {
        Vector3 centeredGroundCheck = new Vector3(center.position.x, groundCheck.position.y);

        hitGround = Physics2D.Raycast(centeredGroundCheck, controller.Swapped == 1 ? Vector2.up : Vector2.down);
        if (hitGround.collider != null && hitGround.distance < Mathf.Abs(verticalVelocity * Time.deltaTime) + 0.001f)
        {
            distanceToSnap = hitGround.distance;
            return true;
        }
        distanceToSnap = 0;
        return false;
    }

    public bool ShouldSnapHorizontaly(float runSpeed, out float distanceToSnap)
    {

        if (runSpeed > 0)
        {
            RaycastHit2D upperRay = Physics2D.Raycast(new Vector3(rightCheck.position.x, head.position.y), Vector2.right, runSpeed);
            RaycastHit2D lowerRay = Physics2D.Raycast(new Vector3(rightCheck.position.x, knee.position.y), Vector2.right, runSpeed);
            
            distanceToSnap = Mathf.Max(upperRay.distance, lowerRay.distance);
            
            return upperRay.collider != null || lowerRay.collider != null;          //return true if any of the rays collide
        }
        else
        {
            runSpeed = -runSpeed;
            RaycastHit2D upperRay = Physics2D.Raycast(new Vector3(leftCheck.position.x, head.position.y), Vector2.left, runSpeed);
            RaycastHit2D lowerRay = Physics2D.Raycast(new Vector3(leftCheck.position.x, knee.position.y), Vector2.left, runSpeed);
            
            distanceToSnap = - Mathf.Max(upperRay.distance, lowerRay.distance);
         
            return upperRay.collider != null || lowerRay.collider != null;          //return true if any of the rays collide
        }
    }
}
