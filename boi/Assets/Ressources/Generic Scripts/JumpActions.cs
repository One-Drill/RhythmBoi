using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpActions : MonoBehaviour
{
    public void jump(Transform m_Transform, Transform headCheck, Transform groundCheck, bool grounded, float verticalVelocity, float jumpDeceleration, float fallAcceleration, float maxFallSpeed)
    {
        if (!grounded)
        {
            if (verticalVelocity > 0)
            {
                verticalVelocity -= jumpDeceleration * Time.deltaTime;
                RaycastHit2D hitCeil = Physics2D.Raycast(headCheck.position, Vector2.up);
                if (hitCeil.collider != null && hitCeil.distance < Mathf.Abs(verticalVelocity) + 0.1f)
                {
                    m_Transform.Translate(new Vector3(0, hitCeil.distance));
                    verticalVelocity = 0;
                }
            }
            if (verticalVelocity <= 0)
            {
                verticalVelocity -= fallAcceleration * Time.deltaTime;
                if (verticalVelocity < -maxFallSpeed)
                    verticalVelocity = maxFallSpeed;
                RaycastHit2D hitFloor = Physics2D.Raycast(groundCheck.position, Vector2.down);
                if (hitFloor.collider != null && hitFloor.distance < Mathf.Abs(verticalVelocity) + 0.1f)
                {
                    m_Transform.Translate(new Vector3(0, -hitFloor.distance));
                    grounded = true;
                    verticalVelocity = 0;
                }
            }
        }
    }
}
