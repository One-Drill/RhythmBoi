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
            //print("alol"); 
            horizontalCorrection += Mathf.Max(RayCollisionDetection(Vector2.left, leftWidth, head), RayCollisionDetection(Vector2.left, leftWidth, knee));
        if (horizontalCorrection != 0)
        {
            controller.SetRunSpeed(0);
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
}
