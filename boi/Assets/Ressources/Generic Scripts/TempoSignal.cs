using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TempoSignal : MonoBehaviour
{
    public static TempoSignal Instance { get; private set; }

    private float startingTime;
    public float bpm;
    public float tolerance = 0.2f;
    private AudioSource music;
    public AudioSource snare;
    public float audioOffset = 0.25f;
    private bool hasBeaten;
    

    void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        music = GetComponent<AudioSource>();
        startingTime = Time.time;
        hasBeaten = true;
    }


    public float indicateBeatPlayer()
    {
        float beatDistance = distanceToBeat();

        if (beatDistance < tolerance)
        {
            //float tmp = distanceToBeat == toNextBeat ? -distanceToBeat : distanceToBeat;
            //somme += tmp;
            //n++;
            //print($"this one: {tmp} moy: {somme / n}");
            return (1);
        }
        if (beatDistance > tolerance)
            hasBeaten = false;
        //print(distanceToBeat);
        return (0);
    }
    public float indicateBeat()
    {
        float currentTime = Time.time - startingTime + audioOffset;
        float spb = 60 / bpm;
        float toPreviousBeat = currentTime % spb;

        if (toPreviousBeat < spb / 2)
            return 1;
        else
            hasBeaten = false;
        return 0;
    }

    public float distanceToBeat()
    {
        float currentTime = Time.time - startingTime + audioOffset;
        float spb = 60 / bpm;
        float toPreviousBeat = currentTime % spb;
        float toNextBeat = spb - toPreviousBeat;
        float distanceToBeat = toPreviousBeat < toNextBeat ? toPreviousBeat : toNextBeat;

        return (distanceToBeat);
    }

    void Update()
    {
        if (!music.isPlaying && indicateBeat() == 1 && !hasBeaten)
        {
            music.Play();
            hasBeaten = true;
        }
        if (indicateBeat() == 0)
            hasBeaten = false;

        if (Input.GetKeyDown("p"))
        {
            
            if (bpm == 180f)
            {
                bpm = 180f;
            }
            else
            {
                bpm += 30f;
                music.pitch += 0.25f;
            }

        }
        if (Input.GetKeyDown("o"))
        {
           
            if (bpm == 120f)
                bpm = 120f;

            else
            {
                bpm -= 30f;
                music.pitch -= 0.25f;
            }
        }
    }
}
