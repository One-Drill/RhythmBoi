using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo.canMoveToRythm())
            music.Play();
    }
}
