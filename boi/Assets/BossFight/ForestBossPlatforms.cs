using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
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
    public GameObject sword;


    // drums
    private Drums drums;


    // feathers
    private FallingFeathers feathers;


   
    // spikes
    int spikePattern;
    private int j;
    int i;
    public bool onBeat;
    public int beats;
    public int bars;
    private int currentPhase;
    private bool phaseEnded;
    [SerializeField] private int[] startPhaseBar;

    void Start()
    {
        j = 0;
        //setup un phase initializer pour debug
        currentPhase = 1;
        noteNumber = 0;
        notesPlayed = 0;
        feathers = GetComponent<FallingFeathers>();
        melody = GetComponent<FollowerOfTheMelody>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        drums = GetComponent<Drums>();
        heightLevel = 0;
        //bars = 0;
        melody.setMusicTime(0);
    }

    void Update()
    {
        //gestion battements et mesures
        onBeat = tempo.canMoveToRythm();
		if (onBeat)
		{
			beats++;

            if (beats == 4)
			{
				beats = 0;
                if (bars >= startPhaseBar[4] )
                    feathers.Feathers();
                if (bars >= startPhaseBar[6])
					drums.DrumPat();
				// gestion de la position des piques sur la plateforme (emplacement temporaire)
				if (bars >= startPhaseBar[1])
				{
					spikePattern++;
					if (spikePattern >= spikeCombination.Length)
						spikePattern = 0;
				}
				bars++;
			}
		}
        // lancement des phases en fonction de l'avancement de la musique /

        //initialisation des lumieres
        if (bars <= startPhaseBar[1])
                Phase1();

        if (bars >= startPhaseBar[1] && bars <= startPhaseBar[2] && heightLevel == 1)
                Phase2();
        //initialisation des piques
        if (bars >= startPhaseBar[2] && bars <= startPhaseBar[3] && heightLevel == 2)
        {
            Phase3();
        }

        if (bars >= startPhaseBar[3] && bars <= startPhaseBar[4] && heightLevel == 3)
        {
            Phase3();

        }
        //initialisation de la dance des plumes
        if (bars >= startPhaseBar[4])
        {
            Phase3();
        }
        //initialisation des tambours
    }

    private void Phase1()
    {
        //reset de la phase en cas de non reussite
        if (bars == startPhaseBar[1] && heightLevel < 1)
        {
            melody.setMusicTime(0);
            bars = startPhaseBar[0];
            noteNumber = 0;
        }
        // cast des elements inclus dans la phase
        platformLogic();
        melodyAnnouncer();
        stepController();
    }

    private void Phase2()
    {
        //reset de la phase en case de non reussite
        if (bars == startPhaseBar[2] && heightLevel < 2)
        {
            //melody.setMusicTime(16)
            melody.setMusicTime(8);
            bars = startPhaseBar[1];
            noteNumber = 0;
        }
        // cast des elements inclus dans la phase
        platformLogic();
        melodyAnnouncer();
        stepController();
    }

    private void Phase3()
    {
        //reset de la phase en case de non reussite
        if (bars == startPhaseBar[3] && heightLevel < 3)
        {
            //melody.setMusicTime(16)
            melody.setMusicTime(16);
            bars = startPhaseBar[2];
            noteNumber = 0;
        }
        // cast des elements inclus dans la phase
        platformLogic();
        melodyAnnouncer();
        stepController();
        // cast des piques
         spikeDance();

    }

    private void Phase4()
    {
        //reset de la phase en case de non reussite
        if (bars == startPhaseBar[3] && heightLevel <= 2)
        {
            //melody.setMusicTime(16)
            melody.setMusicTime(32);
            bars = startPhaseBar[2];
			noteNumber = 0;
		}
        // cast des elements inclus dans la phase
        platformLogic();
        melodyAnnouncer();
        stepController();
        // cast des piques
         spikeDance();
        // cast des épées
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

    // a recoder
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
            j++;
            noteNumber++;
            if (noteNumber >= platformCombination[heightLevel].Length)
            {
                j = 0;
                noteNumber = 0;
            }
        }
    }
    // a recoder
    void melodyAnnouncer()
    {
        if (melody.canMoveToMelody(2))
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.TryGetComponent(out MythicalPlatform platform))
                {
                    if (platformCombination[heightLevel][j].Equals(platform.letter))
                    {
                        platform.lightUp();
                    }

                }
            }
            j++;
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
