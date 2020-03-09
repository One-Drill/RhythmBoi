using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController character;
    public int bufferFrame;

    private float horizontalMove;
    private bool hop;
    private bool up;
    private bool down;
    private FollowerOfTheRhythm tempo;
    private bool spaceIsReleased;
    private bool spaceWasReleased;
    private bool wallJump;
    private int wallJumpBufferCounter = 0;
    private int jumpBufferCounter = 0;

    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        hop = false;
        wallJump = false;
    }

    void Update()
    {
        jumpBufferCounter++;
        wallJumpBufferCounter++;
        if (jumpBufferCounter >= bufferFrame)
            hop = false;
        if (wallJumpBufferCounter >= bufferFrame)
            wallJump = false;
        up = false;
        down = false;
        if (Input.GetKeyDown("space") || Input.GetButtonDown("ps4x"))
        {
            hop = true;
            jumpBufferCounter = 0;
        }
        spaceWasReleased = false;
      /*  if (!character.grounded && Input.GetKeyDown("space"))
        {
            wallJump = true;
            wallJumpBufferCounter = 0;
        }*/
        if (Input.GetKeyUp("space") || Input.GetButtonUp("ps4x"))
            spaceWasReleased = true;
        if (Input.GetKeyDown("w"))
            up = true;
        if (Input.GetKeyDown("s"))
            down = true;
        if (Input.GetKeyDown("r"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        horizontalMove = Input.GetAxisRaw("Horizontal");
        if (horizontalMove != 0 && !character.grounded)
            horizontalMove = horizontalMove < 0 ? -1 : 1;
        character.Move(horizontalMove, hop, up, down, spaceWasReleased, wallJump);
    }
}
