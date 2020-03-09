using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleMovement_Gem : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    public float speed;
    public Rigidbody2D rb;
    private int counter = 0;
    public float canMove;
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if (counter == 3)
        {
            counter = 0;
            speed *= -1;
        }
        if (!moving && tempo.canMoveToRythm())
        {
            canMove++;
            if (canMove % 2 == 0f)
            {
                rb.velocity = transform.right * speed * 4;
                counter++;  
            }
            moving = true;
        }
        else if (moving && tempo.canMoveToRythm())
        {
            moving = false;
            rb.velocity = new Vector2(0, 0);
        }    
    }
}
