using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BackgroundHelper : MonoBehaviour
{
    public float speed = 0;
    float pos = 0;
    private RawImage image;
    public FollowerOfTheRhythm tempo;
    private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        speed *= 20;
        tempo = GetComponent<FollowerOfTheRhythm>();
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo.canMoveToRythm())
            canMove = true;
        if (canMove)
        {
            pos += speed;

            if (pos > 1.0F)
                pos -= 1.0F;

            image.uvRect = new Rect(pos, 0, 1, 1);
            canMove = false;
        }
    }
}
