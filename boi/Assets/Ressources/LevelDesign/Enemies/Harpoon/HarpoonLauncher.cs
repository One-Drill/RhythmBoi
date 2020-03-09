using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonLauncher : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    private FollowerOfTheRhythm tempo;
    private SpriteRenderer sprite;
    private bool isOnbeat;
    private float time2;
    private float time;
    private float waitTime = 0f;
    public int beats = 0;
    public float distancePerFrame = 1f;
    public bool isReturning = false;
    public bool isReturningHalf = false;
    private Vector2 halfDistance= new Vector2(0f, 0f);
    public bool canShoot = true;
    private float spb;

    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        sprite = GetComponent<SpriteRenderer>();
        halfDistance.x = points[0].transform.position.x + (points[1].position.x - points[0].position.x) / 2; ;
        halfDistance.y = points[0].transform.position.y + (points[1].position.y - points[0].position.y) / 2;
        canShoot = true;
    }

    void Update()
    {
       // sprite.transform.Rotate(*)
        
        spb = 60 / tempo.getBpm();
        time += Time.deltaTime / spb * 1.1f;
        time2 += Time.deltaTime / spb * 1.1f;
        isOnbeat = tempo.canMoveToRythm();
        if (canShoot && !isReturning && !isReturningHalf)
        {
            transform.position = Vector2.Lerp(points[0].position,points[1].position, time);
            
        }
        if (isReturning)
        {
            transform.position = Vector2.Lerp(halfDistance, points[0].position, time2);

        }
        if (isReturning && transform.position.y == points[0].position.y)
        {
            waitTime = 0;
            canShoot = true;
            isReturning = false;
            time = 0;       
        }
        if ( isReturningHalf)
        {
            transform.position = Vector2.Lerp(points[1].position , halfDistance, time2);
        }
        if (isReturningHalf && transform.position.y == halfDistance.y)
        {
            if (waitTime == 0)
            {
                waitTime = Time.time + 0.2f;
            }
            if (Time.time >= waitTime)
            {
                time2 = 0;
                isReturningHalf = false;
                isReturning = true;
            }
        }
        if (transform.position.y == points[1].position.y)
        {
            canShoot = false;
            if(beats == 1)
            {
                time2 = 0;
                isReturningHalf = true;
                beats = 0;
            }
            if (isOnbeat)
            {
                beats++;
            }
        }
        
    }
}
