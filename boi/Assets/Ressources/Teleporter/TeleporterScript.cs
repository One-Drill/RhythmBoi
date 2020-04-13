using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleporterScript : MonoBehaviour
{
    [SerializeField]
    public Transform[] teleporters;
    public int teleporterNB;
    public Transform player;
    public TMP_Text tpIndex;
    private int tpSelect;
    private bool canTp = false;
    public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        tpIndex.enabled = false;
        tpSelect = teleporterNB;
    }

    // Update is called once per frame
    void Update()
    {
       bool t =  camera.activeInHierarchy;

        if (canTp)
        {
            if (tpSelect < teleporters.Length && Input.GetKeyDown("e"))
            {
                tpSelect++;
            }
            if (tpSelect > 1 && Input.GetKeyDown("q"))
            {
                tpSelect--;
            }
            tpIndex.text = tpSelect.ToString();
            if (Input.GetKeyDown("w") && tpSelect != teleporterNB)
            {
                player.position = teleporters[tpSelect - 1].position;
            }
        }  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canTp = true;
            tpIndex.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canTp = false;
            tpIndex.enabled = false;
            tpSelect = teleporterNB;
        }
    }
}
