using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FatalHit : MonoBehaviour
{
    public static Vector2 respawnPoint;
    public GameObject panel;
    public CharacterController characterController;
    private float runSpeed;
    private float jumpSpeed;
    private SwapGravity swapGravity;

    void Start()
    {
        respawnPoint = transform.position;
        runSpeed = characterController.maxRunSpeed;
        jumpSpeed = characterController.jumpSpeed;
        swapGravity = GameObject.Find("GravityBox").GetComponent<SwapGravity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FATAL")
        {
            if (SwapGravity.gravitySwapped == true)
                swapGravity.gravitySwap();
            panel.gameObject.SetActive(true);
          //  swapGravity.gravitySwapped = false;
            transform.position = respawnPoint;
            characterController.RunSpeed = 0;
            characterController.verticalVelocity = 0;
            characterController.maxRunSpeed = 0.001f;
            characterController.jumpSpeed = 0f;
            Invoke("Respawn", 1f);
        }
        if (collision.gameObject.tag == "CHECKPOINT")
        {
            respawnPoint = collision.transform.position;
        }
    }

    void Respawn()
    {
        panel.gameObject.SetActive(false);
        characterController.maxRunSpeed = runSpeed;
        characterController.jumpSpeed = jumpSpeed;
    }
}