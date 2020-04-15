using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLever : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool wasActivated = false;
    private Transform m_Transform;
    private FollowerOfTheRhythm tempo;
    private bool isMoving = false;
    private int timesMoved = 0;

    public Sprite spriteCrankUp;
    public Sprite spriteCrankDown;
    public GameObject doorOne;
    public GameObject doorTwo;
    public GameObject doorThree;

    private float doorScriptOne;
    private float doorScriptTwo;
    private float doorScriptThree;

    void Start()
    {
   //     m_Transform = GetComponent<Transform>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (doorOne)
            doorScriptOne = doorOne.GetComponent<DoorPuzzleLever>().doorStatement;
        if (doorTwo)
            doorScriptTwo = doorTwo.GetComponent<DoorPuzzleLever>().doorStatement;
        if (doorThree)
            doorScriptThree = doorThree.GetComponent<DoorPuzzleLever>().doorStatement;
    }

    void Update()
    {
        
        if (tempo.canMoveToRythm() && isMoving == true)
        {
            if (doorOne)
                doorOne.transform.Translate(new Vector3(0, 2.5f * doorScriptOne));
            if (doorTwo)
                doorTwo.transform.Translate(new Vector3(0, 2.5f * doorScriptTwo));
            if(doorThree)
                doorThree.transform.Translate(new Vector3(0, 2.5f * doorScriptThree));
            timesMoved++;
            if (timesMoved == 2)
            {
                doorScriptOne *= -1;
                doorScriptTwo *= -1;
                doorScriptThree *= -1;
                timesMoved = 0;
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && wasActivated == false)
        {
            wasActivated = true;
            isMoving = true;
            spriteRenderer.sprite = spriteCrankDown;
        }
        else if (other.gameObject.tag == "Player" && wasActivated == true)
        {
            wasActivated = false;
            isMoving = true;
            spriteRenderer.sprite = spriteCrankUp;
        }
    }
}
