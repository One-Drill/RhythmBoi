using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoMovement_Gem : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    public float speed;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tempo.canMoveToRythm())
        {
          speed *= -1;
           rb.velocity = transform.right * speed;
        }
        else
            rb.velocity = transform.right * 0;
    }
}
