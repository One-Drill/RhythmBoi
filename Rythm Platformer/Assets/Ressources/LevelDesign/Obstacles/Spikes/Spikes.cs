using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private float tempsReel;
    public FollowerOfTheRhythm tempo;
    private int frame;
    public int direction;
    private bool canMove;
    public int beatsPerSwitch = 1;
    private int beatCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        tempsReel = 60f / tempo.getBpm();
        frame = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tempo.canMoveToRythm())
            beatCounter++;
        if (beatCounter >= beatsPerSwitch)
            canMove = true;
        if (canMove)
        {
            frame++;
            if (frame == 1)
                transform.Translate(0, 1f * transform.parent.localScale.y * direction, 0);
            else if (frame == 2)
                transform.Translate(0, 1f * transform.parent.localScale.y * direction, 0);
            else if (frame == 3)
                transform.Translate(0, 2f * transform.parent.localScale.y * direction, 0);
            else if (frame == 4)
                transform.Translate(0, 2f * transform.parent.localScale.y * direction, 0);
            else if (frame == 5)
            {
                frame = 0;
                tempsReel = 60f / tempo.getBpm();
                direction *= -1;
                canMove = false;
                beatCounter = 0;
                //transform.Translate(0, -1f * transform.parent.localScale.y * direction, 0);               
            }
        }
    }
}