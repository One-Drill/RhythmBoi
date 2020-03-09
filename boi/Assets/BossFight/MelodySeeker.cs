using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodySeeker : MonoBehaviour
{
    private TempoSignal tempo;
    public int timeSignature = 4;
    public int harmonicMultiplier;
    public string melodyMarker;
    private bool hasSung;
    private int noteNumber;

    void Start()
    {
        tempo = GetComponent<TempoSignal>();
    }

    void Update()
    {
        if (inSync())
        {
            if (!hasSung)
            {
                noteNumber++;
                if (noteNumber >= melodyMarker.Length)
                    noteNumber = 0;
            }
            hasSung = true;
        }
        else
            hasSung = false;
    }

    public bool inSync()
    {
        float currentTime = tempo.getMusicTime() - tempo.getStartingTime();
        float spb = 60 / (tempo.bpm * harmonicMultiplier);
        float toPreviousBeat = currentTime % spb;

        if (toPreviousBeat < spb / 2)
            return true;
        return false;
    }
    
    public bool indicateKeyNote(int offset)
    {
        if (inSync() && melodyMarker[(noteNumber + offset * harmonicMultiplier * timeSignature) % melodyMarker.Length] != 'X')
        {
            return (true);
        }
        return (false);
    }
}
