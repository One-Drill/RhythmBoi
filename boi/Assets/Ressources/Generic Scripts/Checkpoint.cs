using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Sprite flag;
    public Sprite respawnFlag;
    private SpriteRenderer checkpointSpriteRenderer;
    public bool checkpointReached;

    void Start()
    {
        checkpointSpriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            checkpointSpriteRenderer.sprite = respawnFlag;
            checkpointReached = true;
        }
    }
}
