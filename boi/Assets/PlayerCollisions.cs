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
    void Start()
    {
        m_Transform = GetComponent<Transform>();
        collisions = m_Transform.Find("Collisions");
        groundCheck = collisions.Find("groundCheck");
        ceilCheck = collisions.Find("ceilCheck");

        rightCheck = collisions.Find("rightCheck");
        leftCheck = collisions.Find("leftCheck");
        head = collisions.Find("head");
        knee = collisions.Find("knee");
    }

    void Update()
    {
        
    }

    public void RectifyPosition()
    {
        Vector3 correct = new Vector3();
        correct.x = Horizontal();
        if (correct.x != 0)
        print(correct.x);
        correct.y = Vertical();
        m_Transform.Translate(correct);
    }

    private float Vertical()
    {
        float heightUp = Mathf.Abs(ceilCheck.position.y - m_Transform.position.y);
        float heightDown = Mathf.Abs(m_Transform.position.y - groundCheck.position.y);
        float verticalCorrection = RayCollisionDetection(Vector2.down, heightDown, m_Transform);
        if (verticalCorrection == 0)
        {
            verticalCorrection -= RayCollisionDetection(Vector2.up, heightUp, m_Transform);
        }
        return verticalCorrection;
    }

    private float Horizontal()
    {
        float rightWidth = Mathf.Abs(transform.position.y - rightCheck.position.y);
        float leftWidth = Mathf.Abs(transform.position.y - leftCheck.position.y);

        float horizontalCorrection = Mathf.Max(RayCollisionDetection(Vector2.right, rightWidth, head), RayCollisionDetection(Vector2.right, rightWidth, knee));
        if (horizontalCorrection == 0)
        {
            horizontalCorrection -= Mathf.Max(RayCollisionDetection(Vector2.left, leftWidth, head), RayCollisionDetection(Vector2.left, leftWidth, knee));
        }
        return horizontalCorrection;
    }

    private float RayCollisionDetection(Vector2 direction, float distance, Transform startingPoint)
    {
        RaycastHit2D hitPoint = Physics2D.Raycast(startingPoint.position, direction);
        if (true || hitPoint.collider != null && hitPoint.distance < distance + 0.1f)
        {
            return distance - hitPoint.distance;
        }
        return 0;
    }
}
