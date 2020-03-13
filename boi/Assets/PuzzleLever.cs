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
    public Transform doorOne;
    public GameObject doorTwo;

    void Start()
    {
   //     m_Transform = GetComponent<Transform>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (tempo.canMoveToRythm() && isMoving == true)
        {
            doorOne.Translate(new Vector3(0, 1.66f));
            timesMoved++;
            if (timesMoved == 3)
            {
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
