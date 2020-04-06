using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SwitchDoor : MonoBehaviour
{
    public static int whichLever = 0;
    private bool activated = false;
    private Transform m_Transform;
    private FollowerOfTheRhythm tempo;
    private bool isMoving = false;
    private int timesMoved = 0;
    public GameObject player;
    private Vector3 temp;
    private float runSpeed;
    private float jumpSpeed;
    private bool playerHasMoved = false;
    private SpriteRenderer playerSprite;
    public CharacterController characterController;

    public int whichDoor;

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        player = GameObject.Find("PLAYER");
        GameObject spritesprite = player.transform.GetChild(1).gameObject;
        playerSprite = spritesprite.GetComponent<SpriteRenderer>();
        runSpeed = characterController.runSpeed;
        jumpSpeed = characterController.jumpSpeed;

    }

    void Update()
    {
        if (whichDoor == whichLever && activated == false)
        {
            activated = true;
            isMoving = true;
            temp = new Vector3(player.transform.position.x, player.transform.position.y);
        }
        if (isMoving)
        {
            StartCoroutine(beforeCameraWait());
        }
    }

    IEnumerator beforeCameraWait()
    {
        if (!playerHasMoved)
        {
            characterController.runSpeed = 0;
            characterController.jumpSpeed = 0;
            yield return new WaitForSeconds(0.5f);
            player.transform.position = new Vector3(100, 10);
            playerSprite.enabled = false;           
            yield return new WaitForSeconds(1);
            playerHasMoved = true;
        }

        if (tempo.canMoveToRythm())
        {
            m_Transform.Translate(new Vector3(0, 1.66f));
            timesMoved++;
            if (timesMoved == 3)
            {
                isMoving = false;
                yield return new WaitForSeconds(1);
                player.transform.position = temp;
                playerSprite.enabled = true;
                characterController.runSpeed = runSpeed;
                characterController.jumpSpeed = jumpSpeed;

            }
        }

    }
    void incrementSwitch()
    {
        whichLever++;
    }
}
