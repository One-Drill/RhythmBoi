using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alternance : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteOne;
    public Sprite spriteTwo;
    public Sprite spriteThree;
    public Sprite spriteFour;

    private int objActif;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        objActif = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo.canMoveToRythm())
        {
            if (objActif == 4)
                objActif = 1;
            else
                objActif++;
            if (objActif == 1)
                spriteRenderer.sprite = spriteOne;
            else if (objActif == 2)
                spriteRenderer.sprite = spriteTwo;
            else if (objActif == 3)
                spriteRenderer.sprite = spriteThree;
            else if (objActif == 4)
                spriteRenderer.sprite = spriteFour;
        }


    }
}
