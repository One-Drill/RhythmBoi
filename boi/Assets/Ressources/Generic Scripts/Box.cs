using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Transform upRight;
    public Transform upLeft;
    public Transform downRight;
    public Transform downLeft;
    private Transform m_Transform;
    public int swapped;
    private float verticalVelocity;
    public float maxFallSpeed;
    public float fallAcceleration;
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (swapped == 1)
            m_Transform.eulerAngles = new Vector3(0, 0, 180);
        else
            m_Transform.eulerAngles = new Vector3(0, 0, 0);
        moveVertical();
    }

    void moveVertical()
    {
        RaycastHit2D hitFloorRight = Physics2D.Raycast(downRight.position, swapped == 1 ? Vector2.up : Vector2.down);
        RaycastHit2D hitFloorLeft = Physics2D.Raycast(downLeft.position, swapped == 1 ? Vector2.up : Vector2.down);
        if (hitFloorRight.collider == null || hitFloorRight.distance > Mathf.Abs(verticalVelocity) + 0.1f
            && hitFloorLeft.collider == null || hitFloorLeft.distance > Mathf.Abs(verticalVelocity) + 0.1f)
            grounded = false;
        if (!grounded)
        {
            verticalVelocity += fallAcceleration * Time.deltaTime;
            if (verticalVelocity >= maxFallSpeed)
                verticalVelocity = maxFallSpeed;
            if (hitFloorRight.collider != null && hitFloorRight.distance < Mathf.Abs(verticalVelocity * Time.deltaTime) + 0.1f)
            {
                m_Transform.Translate(new Vector3(0, -hitFloorRight.distance + 0.02f));
                verticalVelocity = 0;
                grounded = true;
            }
            else if (hitFloorLeft.collider != null && hitFloorLeft.distance < Mathf.Abs(verticalVelocity * Time.deltaTime) + 0.1f)
            {
                m_Transform.Translate(new Vector3(0, -hitFloorLeft.distance + 0.02f));
                verticalVelocity = 0;
                grounded = true;
            }
            else
                m_Transform.Translate(new Vector3(0, -verticalVelocity * Time.deltaTime, 0));
        }
    }

    public void moveLeft(float distance)
    {
      //  m_Transform.localScale += new Vector3(0, -0.001f, 0);
        RaycastHit2D upLeftCheck = Physics2D.Raycast(upLeft.position, swapped == 1 ? Vector2.right : Vector2.left);
        RaycastHit2D hitFloorLeft = Physics2D.Raycast(downLeft.position, swapped == 1 ? Vector2.right : Vector2.left);
        if (upLeftCheck.collider == null && hitFloorLeft.collider == null)
            m_Transform.Translate(-distance, 0, 0);
        else
        {
            float minDistance = Mathf.Abs(distance);
            if (upLeftCheck.collider != null)
                minDistance = minDistance < upLeftCheck.distance ? minDistance : upLeftCheck.distance;
            if (hitFloorLeft.collider != null)
                minDistance = minDistance < hitFloorLeft.distance ? minDistance : hitFloorLeft.distance;
            m_Transform.Translate(-minDistance, 0, 0);
        }
      //  m_Transform.localScale += new Vector3(0, 0.001f, 0);
    }

    public void moveRight(float distance)
    {
       // m_Transform.localScale += new Vector3(0, -0.001f, 0);
        RaycastHit2D upRightCheck = Physics2D.Raycast(upRight.position, swapped == 1 ? Vector2.left : Vector2.right);
        RaycastHit2D hitFloorRight = Physics2D.Raycast(downRight.position, swapped == 1 ? Vector2.left : Vector2.right);
        if (upRightCheck.collider == null && hitFloorRight.collider == null)
            m_Transform.Translate(distance, 0, 0);
        else
        {
            float minDistance = Mathf.Abs(distance);
            if (upRightCheck.collider != null)
                minDistance = minDistance < upRightCheck.distance ? minDistance : upRightCheck.distance;
            if (hitFloorRight.collider != null)
                minDistance = minDistance < hitFloorRight.distance ? minDistance : hitFloorRight.distance;
            m_Transform.Translate(minDistance, 0, 0);
        }
     //   m_Transform.localScale += new Vector3(0, 0.001f, 0);
    }
}
