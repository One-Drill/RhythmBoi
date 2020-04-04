using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBossPlatforms : MonoBehaviour
{
    public string[] platformCombination;
    public string[] spikeCombination;
    private int noteNumber;
    private FollowerOfTheMelody melody;
    private FollowerOfTheRhythm tempo;
    public Transform player;
    public int offset;
    private int succesNumber;
    private int step;
    public float stepUpDist;
    private static int heightLevel;
    public int maxHeight;
    private int notesPlayed;


    // spikes
    int spikePattern;
    int i;
    private static bool onBeat;
    private static int beats;
    private static int bars;
    private int currentPhase;
    private bool phaseEnded;
    [SerializeField] private int[] startPhaseBar;

    void Start()
    {
        currentPhase = 1;
        noteNumber = 0;
        notesPlayed = 0;
        melody = GetComponent<FollowerOfTheMelody>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        heightLevel = 0;
        beats = 0;
        bars = 0;
    }

    void Update()
    {
        
        // tant que la musique est lancée
            if (bars < startPhaseBar[1])
                Phase1();
            if (bars >= startPhaseBar[1] && bars < startPhaseBar[2])
                Phase2();
            if (bars >= startPhaseBar[2] && bars < startPhaseBar[3])
            //Phase3();


            //if (currentPhase == 2)
            //{
            //    spikeDance();
            //}

            // permet de quantifier les battements et les mesures de la musique
            //if (beats >= 4)
            // {

            //        beats = 0;
            //    //spikes
            //         if (bars >= startPhaseBar[1])
            //         {
            //            spikePattern++;
            //            if (spikePattern >= spikeCombination.Length)
            //            spikePattern = 0;
            //         }
            //      //  print(bars);
            //        bars++;
            //    }
    if (beats == 4)
            {
                print(bars);
                
                bars++;
                beats = 0;
            }
            if (tempo.canMoveToRythm())
            {
                beats++;
            }
    }
    private void Phase1()
    {
        platformLogic();
        melodyAnnouncer();
        stepController();

        //print(bars);
        //reset la phase if no succes on melodyannoncer
        if (bars + 1 == startPhaseBar[1] && heightLevel < 1)
        {
            melody.setMusicTime(0);
            bars = startPhaseBar[0];
        }
    }

    private void Phase2()
    {
        platformLogic();
        melodyAnnouncer();
        stepController();
        

        //reset phase
        if (bars + 1 == startPhaseBar[3] && heightLevel < 2)
        {
            melody.setMusicTime((bars * melody.getTimeSignature()) * (1 / tempo.getBpm()) + Time.deltaTime);
            bars = startPhaseBar[1];
        }
    }

    void stepController()
    {
        if (step != 0)
        {
            if (onBeat && !(heightLevel == 0 && Math.Sign(step) == -1) && !(heightLevel >= maxHeight && Math.Sign(step) == 1))
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
            if (platformCombination[heightLevel][noteNumber].Equals('X'))
                platformSuccess();
            else
            {
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
            }
            noteNumber++;
            if (noteNumber >= platformCombination[heightLevel].Length)
                noteNumber = 0;
        }
    }

    void melodyAnnouncer()
    {
        if (melody.canMoveToMelody(2))
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.TryGetComponent(out MythicalPlatform platform))
                {
                    if (platformCombination[heightLevel][noteNumber].Equals(platform.letter))
                    {
                        platform.lightUp();
                    }
                }
            }
            noteNumber++;
            if (noteNumber >= platformCombination[heightLevel].Length)
                noteNumber = 0;
        }
    }

    void spikeDance()
    {

        i = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent(out MythicalPlatform platform))
            {
                if (spikeCombination[spikePattern][i].Equals(platform.letter))
                {
                    platform.spikes.gameObject.SetActive(true);
                }
                else
                    platform.spikes.gameObject.SetActive(false);
                i++;
            }
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
