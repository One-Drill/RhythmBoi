using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drums : MonoBehaviour
{
    public string[] affectedDrums;
    private int drumPattern;
    private int i;

    
    // Start is called before the first frame update
    // Update is called once per frame
   public void DrumPat()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent(out MythicalPlatform platform))
            {
                  if (affectedDrums[drumPattern][i].Equals(platform.letter))
                {
                    platform.life--;
                    if (platform.life == 0)
                        platform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
            }

        }
        drumPattern++;
    }
}
