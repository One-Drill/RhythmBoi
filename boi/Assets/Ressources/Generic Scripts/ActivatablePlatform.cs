using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatablePlatform : MonoBehaviour
{
    //possible problem: Running two puzzles at the same time will cause trouble.
    private static int timer = 0;
    private static bool timerIsActive = false;
    private static FollowerOfTheRhythm tempo;
    private static bool isActivatable = true;
    public static bool puzzleComplete = false;

    public int resetTime = 1;
    public int puzzleTimeInBeats = 10;
    public Sprite activated, deactivated;

    private SpriteRenderer spriteRenderer;
    public bool isActivated = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tempo = GetComponent<FollowerOfTheRhythm>();
    }

    void Update()
    {
        if (puzzleComplete)
        {
            isActivated = true;
            spriteRenderer.sprite = activated;
        }
        else
        {
            if (!timerIsActive)
            {
                isActivated = false;
                spriteRenderer.sprite = deactivated;
            }
            else
            {
                if (tempo.canMoveToRythm())
                {
                    timer -= 1;
                }
                if (timer == 0)
                {
                    isActivatable = false;
                    timerIsActive = false;
                }
            }
            if (!isActivatable && tempo.canMoveToRythm())
            {
                timer -= 1;
                if (timer <= -resetTime)
                {
                    isActivatable = true;
                    timer = 0;
                }
            }
        }
    }

    public void isSteppedOn()
    {
        if (isActivatable && !puzzleComplete)
        {
            isActivated = !isActivated;
            if (isActivated)
                spriteRenderer.sprite = activated;
            else
                spriteRenderer.sprite = deactivated;
            if (!timerIsActive)
            {
                timer = puzzleTimeInBeats;
                timerIsActive = true;
            }
        }
    }
}
