using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    public Rigidbody2D rb;
    public float speed;
    private int moving = 1;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteOne;
    public Sprite spriteTwo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "SPIKES")
           Destroy(gameObject);
    }

    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
    }

    void FixedUpdate()
    {
        if (tempo.canMoveToRythm())
        {
            rb.velocity = transform.right * -speed;
            speed = speed * -1;
            moving *= -1;
            if (moving == 1)
                spriteRenderer.sprite = spriteOne;
            else
                spriteRenderer.sprite = spriteTwo;
        }
    }
}
