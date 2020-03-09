using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TempoSignal : MonoBehaviour
{
    public static TempoSignal Instance { get; private set; }

    private float startingTime;
    private float currentTime;
    public float bpm;
    public float tolerance = 0.2f;
    private AudioSource music;
    public AudioSource snare;
    public float audioOffset = 0.25f;
    private bool hasBeaten = false;
    private bool hasBeaten2 = false;
    private float n;
    private float somme;
    

    void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        music = GetComponent<AudioSource>();
        music.Play();
        startingTime = music.time + audioOffset;
        currentTime = startingTime;
        n = 0;
        somme = 0;
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
        float currentTime = music.time - startingTime;
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
        float currentTime = music.time - startingTime;
        float spb = 60 / bpm;
        float toPreviousBeat = currentTime % spb;
        float toNextBeat = spb - toPreviousBeat;
        float distanceToBeat = toPreviousBeat < toNextBeat ? toPreviousBeat : toNextBeat;

        return (distanceToBeat);
    }

    void Update()
    {
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
