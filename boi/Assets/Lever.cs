using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool wasActivated = false;
    private SpriteRenderer spriteRenderer;
    public Sprite spriteCrankDown;
    public bool destroyDoor = false;
    public GameObject doorOne;
    public GameObject doorTwo;
    //  public GameObject player;
    public CharacterController characterController;

    void Start()
    {
     //   player = GameObject.Find("PLAYER");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && wasActivated == false)
        {
            characterController.RunSpeed = 0;
            characterController.jumpSpeed = 0;
            wasActivated = true;
            SwitchDoor.whichLever++;
            spriteRenderer.sprite = spriteCrankDown;
            if (destroyDoor == true)
            {
                if (doorOne)
                    Destroy(doorOne);
                if (doorTwo)
                    Destroy(doorTwo);
            }
        }
        
    }
}
