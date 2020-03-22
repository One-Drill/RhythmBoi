using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerOfTheMelody : MonoBehaviour
{
    private TempoSignal tempo;
    private MelodySeeker melody;
    private bool hasSung;

    void Start()
    {
        hasSung = true;
        tempo = TempoSignal.Instance;
        melody = tempo.melody;
    }

    // Update is called once per frame
    void Update()
    {
        if (!melody.inSync() && hasSung)
        {
            hasSung = false;
        }
    }

    public bool canMoveToMelody(int offset)
    {
        if (melody.indicateKeyNote(offset) && !hasSung)
        {
            hasSung = true;
            return (true);
        }
        return (false);
    }

    public int getTimeSignature()
    {
        return (melody.timeSignature);
    }

    public bool indicateKeyNote()
    {
        return (melody.indicateKeyNote(0));
    }

    public void setMusicTime(float time)
    {
        tempo.setMusicTime(time);
    }
}
