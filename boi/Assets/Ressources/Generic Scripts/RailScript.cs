using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailScript : MonoBehaviour
{
    [SerializeField]
    public Transform[] points;
    public bool canReturn;
    private Vector2 currentSpoint;
    private Vector2 currentEpoint;
    private float mpb = 0;
    public float waitPercent;
    public float goPercent;
    private int  i = 1;
    private FollowerOfTheRhythm tempo;
    private bool coroutineAllowed = true;
    private bool returning = false;
    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
    }

    // Update is called once per frame
    void Update()
    {
        mpb = 60f / tempo.getBpm();
        if(coroutineAllowed)
        {
            currentSpoint = transform.position;
            currentEpoint = points[i].position;
            StartCoroutine(MoveObjectOnBeat(currentSpoint, currentEpoint));

        }
    }

    private IEnumerator MoveObjectOnBeat(Vector2 start, Vector2 end)
    {
        float waitTime = 0f;
        coroutineAllowed = false;
        float time = 0f;
        while(transform.position.x != end.x || transform.position.y != end.y)
        {
            time += Time.deltaTime / mpb * goPercent;
            transform.position = Vector3.Lerp(start, end, time);
            yield return null;
        }
        waitTime = Time.time + mpb * waitPercent;
        while (Time.time < waitTime)
        {
            yield return null;
        }
        if (i == points.Length - 1)
        {
            if (canReturn)
                returning = true;
            else
            {
                i = 0;
                transform.position = points[0].position;
            }

        }
        if (i <= 0)
            returning = false;
        if (i < points.Length - 1 && !returning)
            i++;
        else if (returning && i > 0 && canReturn)
        {
            i--;
        }
        coroutineAllowed =  true;
    }

}
