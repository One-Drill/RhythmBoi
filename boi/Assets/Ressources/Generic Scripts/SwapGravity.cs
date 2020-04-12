using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapGravity : MonoBehaviour
{
    public static bool gravitySwapped = false;

    private CharacterController characterController;
    private SpriteRenderer boxRenderer;
    public bool gravityShouldSwap = false;
    private FollowerOfTheRhythm tempo;
    public int numberOfBoxes;
    public float cooldownBeats;
    private float beatCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        boxRenderer = GetComponent<SpriteRenderer>();
        characterController = GameObject.Find("PLAYER").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gravityShouldSwap)
        {
            if (tempo.canMoveToRythm())
                beatCounter--;
        }
        gravityShouldSwap = beatCounter <= 0 && gravityShouldSwap;
        if (gravityShouldSwap)
        { 
            gravitySwap();
            beatCounter = cooldownBeats;
            gravityShouldSwap = false;
        }
        if (gravitySwapped || (gravityShouldSwap && !gravitySwapped))
            boxRenderer.flipY = true;
        else
            boxRenderer.flipY = false;
    }

    public void gravitySwap()
    {
        gravitySwapped = !gravitySwapped;
        for (int i = 0; i < numberOfBoxes; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.GetComponent<Box>().swapped = gravitySwapped ? 1 : -1;
        }
        characterController.verticalVelocity *= -1;
        characterController.Swapped = gravitySwapped ? 1 : -1;
    }
}
