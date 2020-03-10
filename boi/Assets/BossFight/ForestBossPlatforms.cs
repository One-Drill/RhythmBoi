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
        platformLogic();
        if (step != 0)
        {
            if (tempo.canMoveToRythm() && !(heightLevel == 0 && Math.Sign(step) == -1) && !(heightLevel >= maxHeight && Math.Sign(step) == 1))
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
        if (heightLevel >= maxHeight)
            heightLevel = maxHeight - 1;
        if (heightLevel < 0)
            heightLevel = 0;
        if (notesPlayed >= platformCombination[heightLevel].Length)
        {
            print(succesNumber);
            if (succesNumber >= platformCombination[heightLevel].Length)
                step = melody.getTimeSignature();
            if (succesNumber < 2)
                step = -melody.getTimeSignature();
            succesNumber = 0;
            notesPlayed = 0;
        }
    }

    void platformLogic()
    {
        if (melody.canMoveToMelody(offset))
        {
            notesPlayed++;
            print($"notes played {notesPlayed}");
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
                        platformSuccess();
                    }
                    else
                    {
                        platform.Failure();
                        platformFailure();
                    }
                }
            }
            noteNumber++;
            if (noteNumber >= platformCombination[heightLevel].Length)
                noteNumber = 0;
        }
    }

    private void platformFailure()
    {
    }

    private void platformSuccess()
    {
        succesNumber += 1;
    }
}
