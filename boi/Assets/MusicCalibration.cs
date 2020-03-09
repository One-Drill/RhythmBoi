using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCalibration : MonoBehaviour
{
    private AudioSource sound;
    private FollowerOfTheRhythm tempo;

    // Start is called before the first frame update

    void Start()
    {
        sound = GetComponent<AudioSource>();
        tempo = GetComponent<FollowerOfTheRhythm>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo.canMoveToRythm())
        {
            sound.Play();
        }
    }
}
