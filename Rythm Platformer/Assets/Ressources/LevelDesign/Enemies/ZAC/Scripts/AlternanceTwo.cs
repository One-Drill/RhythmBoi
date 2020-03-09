using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternanceTwo : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D rb;
    public int speed;
    private int frame;
    private bool canMove;
    public Sprite spriteOne;
    public Sprite spriteTwo;
    public Sprite spriteThree;
    public Sprite spriteFour;
    public Sprite spriteFive;
    public Sprite spriteSix;
    public Sprite spriteSeven;
    public Sprite spriteEight;
    public Sprite spriteNine;
    public Sprite spriteTen;
    public Sprite spriteEleven;
    public Sprite spriteTwelve;
    public Sprite spriteThirteen;
    public Sprite spriteFourteen;
    public Sprite spriteFifteen;
    public Sprite spriteSixteen;
    public Sprite spriteSeventeen;
    public Sprite spriteEighteen;
    public Sprite spriteNineteen;
    public Sprite spriteTwenty;
    private int ActifObj;

    // Start is called before the first frame update
    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        ActifObj = 1;
        frame = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (tempo.canMoveToRythm())
            canMove = true;
        if (canMove)
        {
            if (ActifObj == 11)
                ActifObj = 1;
            else if (ActifObj == 10)
            {
                gameObject.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
                // gameObject.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
                // speed *= -1;
                SpriteRenderer.sprite = spriteOne;
                canMove = false;
                frame = 0;
                ActifObj++;    
            }
            else if (ActifObj == 9)
            {
                if (frame >= 1 && frame <= 32)
                   SpriteRenderer.sprite = spriteTwenty;      
                else
                {
                    SpriteRenderer.sprite = spriteNineteen;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 8)
            {
                frame++;
                if (frame >= 1 && frame <= 2)
                {
                    rb.velocity = transform.right * -speed * 2;
                    SpriteRenderer.sprite = spriteEighteen;
                }
                else
                {
                    SpriteRenderer.sprite = spriteOne;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 7)
            {
                frame++;
                if (frame >= 1 && frame <= 4)
                {
                    rb.velocity = transform.right * -speed * 2;
                    SpriteRenderer.sprite = spriteSixteen;
                }
                else
                {
                    SpriteRenderer.sprite = spriteSeventeen;
                    rb.velocity = transform.right * -speed * 2;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 6)
            {
                frame++;
                if (frame >= 1 && frame <= 2)
                {
                    rb.velocity = transform.right * -speed * 2;
                    SpriteRenderer.sprite = spriteFourteen;
                }
                else
                {
                    SpriteRenderer.sprite = spriteFifteen;
                    rb.velocity = transform.right * -speed * 2;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 5)
            {
                frame++;
                // if (frame >= 1 && frame <= 4)
                //   SpriteRenderer.sprite = spriteEleven;
                if (frame >= 1 && frame <= 2)
                {
                    rb.velocity = transform.right * -speed;
                    SpriteRenderer.sprite = spriteTwelve;
                }
                else
                {
                    rb.velocity = transform.right * -speed;
                    SpriteRenderer.sprite = spriteThirteen;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 4)
            {
                frame++;
                if (frame == 1)
                    SpriteRenderer.sprite = spriteEight;
                else if (frame == 2)
                    SpriteRenderer.sprite = spriteNine;
                else if (frame == 3)
                    SpriteRenderer.sprite = spriteTen;
                else
                {
                    SpriteRenderer.sprite = spriteEleven;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 3)
            {
                frame++;
                if (frame >= 1 && frame <= 2)
                {
                    rb.velocity = transform.right * -speed;
                    SpriteRenderer.sprite = spriteSix;
                }
                else
                {
                    SpriteRenderer.sprite = spriteSeven;
                    rb.velocity = transform.right * -speed;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 2)
            {
                frame++;
                if (frame >= 1 && frame <= 2)
                {
                    rb.velocity = transform.right * -speed;
                    SpriteRenderer.sprite = spriteFour;
                }
                else
                {
                    SpriteRenderer.sprite = spriteFive;
                    rb.velocity = transform.right * -speed;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
            else if (ActifObj == 1)
            {
                frame++;
                if (frame >= 1 && frame <= 2)
                {
                    rb.velocity = transform.right * -speed;
                    SpriteRenderer.sprite = spriteTwo;
                }
                else
                {
                    SpriteRenderer.sprite = spriteThree;
                    rb.velocity = transform.right * -speed;
                    canMove = false;
                    frame = 0;
                    ActifObj++;
                }
            }
        }
        else
            rb.velocity = transform.right * 0;
    }
}