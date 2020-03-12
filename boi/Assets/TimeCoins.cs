using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCoins : MonoBehaviour
{
    
    public float timeBeforeVanishingInFrame;
    public Transform hiddenPlace;
    private float resetTimer;
    public static bool isCharacterDead = false;
    public Transform initialPosition;
    public GameObject thisCoin;
    public GameObject camera;
    private bool firstEntrance = false;

    void Start()
    {
        resetTimer = timeBeforeVanishingInFrame;
        transform.position = initialPosition.transform.position;
    }

    void Update()
    {
        if (!camera.activeInHierarchy)
           firstEntrance = true;
        if (isCharacterDead == true || (camera.activeInHierarchy && firstEntrance == true))
        {
            firstEntrance = false;
            timeBeforeVanishingInFrame = resetTimer;
            transform.position = initialPosition.transform.position;    
        }
        if (isCharacterDead == false)
            timeBeforeVanishingInFrame--;
        if (timeBeforeVanishingInFrame <= 0)
        {
            transform.position = hiddenPlace.transform.position;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(thisCoin);
        }
    }

        //   public void characterDead()
        //   {
        //       isCharacterDead = true;
        //  }
    }
