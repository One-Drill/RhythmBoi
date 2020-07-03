using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFeathers : MonoBehaviour
{
    public GameObject platformPosition;
    public int positionNumbers;
    public GameObject feather;
    private FollowerOfTheRhythm tempo;
    private static int i = 1000;
    private Transform[] tab;
    public string[] affectedFeather;
    private int featherPattern;
    int j;
    public Transform player;

    public void Feathers()
    {
            CreateFeathers();
    }

    void CreateFeathers()
    {     
       
       i++;
        if (i >= affectedFeather[featherPattern].Length)
        {
            i = 0;
            featherPattern = 0;
            //  featherPattern = Random.Range(1, affectedFeather.Length);
            RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out MythicalPlatform platform))
                {
                     while (featherPattern < affectedFeather.Length)
                    {
                        if (affectedFeather[featherPattern][i].Equals(platform.letter))
                            break;
                       featherPattern++;
                    }                      
                }
            }
        }
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent(out MythicalPlatform platform))
            {

                if (affectedFeather[featherPattern][i].Equals(platform.letter))
                {
                    Instantiate(feather, platform.transform.transform.Find("Firepoint").transform.position, Quaternion.identity);
                }
            }
        }
    }
}