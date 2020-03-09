using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZacAlternance : MonoBehaviour
{
    private FollowerOfTheRhythm tempo;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteOne;
    public Sprite spriteTwo;
    public Sprite spriteThree;
    public Sprite spriteFour;
    public Sprite spriteFive;
    public Sprite spriteSix;
    public Sprite spriteSeven;
    public Sprite spriteEight;
    public Sprite spriteNine;

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
            if (objActif == 9)
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
            else if (objActif == 5)
                spriteRenderer.sprite = spriteFive;
            else if (objActif == 6)
                spriteRenderer.sprite = spriteSix;
            else if (objActif == 7)
                spriteRenderer.sprite = spriteSeven;
            else if (objActif == 8)
                spriteRenderer.sprite = spriteEight;
            else if (objActif == 9)
                spriteRenderer.sprite = spriteNine;
        }


    }
}
