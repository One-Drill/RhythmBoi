using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFeathers : MonoBehaviour
{
    public GameObject platformPosition;
    public int positionNumbers;
    public SpriteRenderer spriteRenderer;
    private FollowerOfTheRhythm tenpo;
    private int i;
    //public Transform[] tab;
    private Transform[] tab;

    void Start()
    {
        tab = new Transform[positionNumbers];
        tenpo = GetComponent<FollowerOfTheRhythm>();
        platformPosition = GameObject.Find("BossPlatforms");
       while (i < positionNumbers)
        {
            tab[i] = platformPosition.transform.GetChild(i).transform;
            i++;
        }
        i = 0;
        print("tab[i]");
    }

    // Update is called once per frame
     public void Feathers()
    {
            CreateFeathers(tab[i]);
    }

        void CreateFeathers(Transform position)
        {
            if (++i >= positionNumbers)
                i = 0;
            Instantiate(spriteRenderer, position.transform.position, Quaternion.identity);
        }
    }