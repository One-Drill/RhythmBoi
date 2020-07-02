using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFeathers : MonoBehaviour
{
    public GameObject platformPosition;
    public int positionNumbers;
    public GameObject feather;
    private FollowerOfTheRhythm tempo;
    private int i;
    private Transform[] tab;
    public string[] affectedFeather;
    private int featherPattern;
    int j;

     public void Feathers()
    {
            CreateFeathers();
    }

    void CreateFeathers()
    {     
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
   //    i++;
    //    if (i >= affectedFeather[featherPattern].Length)
      //     i = 0;
        featherPattern++;
       if (featherPattern >= affectedFeather.Length)
              featherPattern = 0;
    }
}
