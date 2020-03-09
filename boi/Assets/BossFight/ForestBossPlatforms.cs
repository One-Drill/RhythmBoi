using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBossPlatforms : MonoBehaviour
{
    public string[] platformCombination;
    private int noteNumber;
    private FollowerOfTheMelody melody;
    private FollowerOfTheRhythm tempo;
    public Transform player;
    public int offset;
    private int succesNumber;
    private int step;
    public float stepUpDist;
    private int heightLevel;
    public int maxHeight;
    private int notesPlayed;

    void Start()
    {
        noteNumber = 0;
        notesPlayed = 0;
        melody = GetComponent<FollowerOfTheMelody>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        heightLevel = 0;
    }

    void Update()
    {
        print(step);
        platformLogic();
        if (step != 0)
        {
            succesNumber = 0;
            if (tempo.canMoveToRythm() )//&& !(heightLevel == 0 && Math.Sign(step) == -1) && !(heightLevel >= maxHeight && Math.Sign(step) == 1))
            {
                RaycastHit2D hit = Physics2D.Raycast(player.GetComponent<CharacterController>().groundCheck.position, Vector2.down);
                if (hit.distance < stepUpDist)
                {
                    player.Translate(new Vector3(0, (stepUpDist - hit.distance) * Math.Sign(step)));
                }
                foreach (Transform child in transform)
                {
                    child.transform.Translate(new Vector3(0, stepUpDist * Math.Sign(step)));
                }
                step -= Math.Sign(step);
                if (step == 1 && heightLevel < maxHeight)
                    heightLevel++;
                if (step == -1 && heightLevel > 0)
                    heightLevel--;
            }
        }
    }

    void platformLogic()
    {
        if (melody.canMoveToMelody(offset))
        {
            notesPlayed++;
            foreach (Transform child in transform)
            {
                if (child.gameObject.TryGetComponent(out MythicalPlatform platform))
                {
                    if (platformCombination[heightLevel][noteNumber].Equals(platform.letter))
                    {
                        platform.transform.GetComponent<SpriteRenderer>().color = Color.black;
                        platform.colorChanged = true;
                    }
                }
            }
            RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out MythicalPlatform platform))
                {
                    if (platformCombination[heightLevel][noteNumber].Equals(platform.letter))
                    {
                        platform.Success();
                        Success();
                    }
                    else
                    {
                        platform.Failure();
                        Failure();
                    }
                }
            }
            noteNumber++;
            if (noteNumber >= platformCombination[heightLevel].Length)
                noteNumber = 0;
        }
    }

    private void Failure()
    {
        //succesNumber -= 1;
        succesNumber = 0;
    }

    private void Success()
    {
        succesNumber += 1;
        if (notesPlayed >= platformCombination[heightLevel].Length)
        {
            if (succesNumber >= platformCombination[heightLevel].Length)
                step = melody.getTimeSignature();
            else
                step = -melody.getTimeSignature();
            notesPlayed = 0;
        }
    }
}
