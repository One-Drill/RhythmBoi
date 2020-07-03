using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    public Rigidbody2D rb;
    public float speed;
    private bool moving = false;

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
    //        moving = true;
        }
     //   else if (moving && tempo.canMoveToRythm())
     //   {
    //        moving = false;
     //       rb.velocity = new Vector2(0, 0);
      //  }
    }
}
