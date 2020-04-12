using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsePusher : MonoBehaviour
{
    public int beatsPerPulse;
    public int beatsPulsing;
    private int beatOffCounter = 0;
    private int beatOnCounter = 0;
    private Transform m_Transform;
    private CharacterController characterController;
    public GameObject character;
    private FollowerOfTheRhythm tempo;
    private bool pushing = false;
    private Vector3 pushVector;
    public float xDivider;
    public float yDivider;
    public float pushVectorDistance;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = GetComponent<Transform>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        characterController = GameObject.Find("PLAYER").GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!pushing)
        {
            if (tempo.canMoveToRythm())
                beatOffCounter++;
            if (beatOffCounter >= beatsPerPulse)
            {
                pushing = true;
                beatOffCounter = 0;
            }
        }
        else
        {
            if (tempo.canMoveToRythm())
                beatOnCounter++;
            if (beatOnCounter >= beatsPulsing)
            {
                pushing = false;
                beatOnCounter = 0;
            }
        }
        if (pushing)
            pushCharacter();
    }

    void pushCharacter()
    {
        Vector3 pushDirection = character.transform.position - m_Transform.position;
        Vector3 pushVector = pushDirection.normalized;
        pushVector *= pushVectorDistance;
        if (Mathf.Abs(pushDirection.x) < Mathf.Abs(pushVector.x) && Mathf.Abs(pushDirection.y) < Mathf.Abs(pushVector.y))
        {
            characterController.runDeceleration = 0;
            characterController.RunSpeed += (pushVector.x - pushDirection.x) / xDivider;
            characterController.verticalVelocity += (pushVector.y - pushDirection.y) * Time.deltaTime / yDivider;
        }
    }
}
