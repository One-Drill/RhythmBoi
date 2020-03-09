using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
  
    public float bpm;
    private int frame;
    public GameObject impactEffect;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteOne;
    public Sprite spriteTwo;
    public Sprite spriteThree;
    public Sprite spriteFour;
    public Sprite spriteFive;
    private FollowerOfTheRhythm tempo;
    private bool canMove;

    private bool moving = false;

    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        frame = 0;
        
    }

    void FixedUpdate()
    {
        /* if (tempo.canMoveToRythm())
             canMove = true;
         if (canMove)
         {
             frame++;
             if (frame == 1)
             {
                 rb.velocity = transform.right * -speed * 4;
                 spriteRenderer.sprite = spriteTwo;
             }
             if (frame == 2)
             {
                 rb.velocity = transform.right * -speed * 4;
                 spriteRenderer.sprite = spriteThree;
             }
             if (frame == 3)
             {
                 rb.velocity = transform.right * -speed * 3;
                 spriteRenderer.sprite = spriteFour;
             }
             if (frame == 4)
             {
                 rb.velocity = transform.right * -speed * 3;
                 spriteRenderer.sprite = spriteFive;
             }
             if (frame == 5)
             {
                 rb.velocity = transform.right * -speed * 2;
                 spriteRenderer.sprite = spriteFour;
             }
             if (frame == 6)
             {
                 rb.velocity = transform.right * -speed * 2;
                 spriteRenderer.sprite = spriteThree;
             }
             if (frame == 7)
             {
                 rb.velocity = transform.right * -speed * 1;
                 spriteRenderer.sprite = spriteTwo;
             }
             if (frame == 8)
             {
                 rb.velocity = transform.right * -speed * 1;
                 spriteRenderer.sprite = spriteTwo;
             }
             if (frame == 9)
             {
                 spriteRenderer.sprite = spriteOne;

                 frame = 0;
                 canMove = false;
             }
         }
         else
             rb.velocity = transform.right * 0;*/
        if (!moving && tempo.canMoveToRythm())
        {
            rb.velocity = transform.right * -speed;
            spriteRenderer.sprite = spriteTwo;
            moving = true;
        }
        else if (moving && tempo.canMoveToRythm())
        {
            moving = false;
            rb.velocity = new Vector2(0, 0);
            spriteRenderer.sprite = spriteOne;
        }

    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        if (hitInfo.tag == "WALL" || hitInfo.tag == "FATAL")
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
