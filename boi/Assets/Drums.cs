using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drums : MonoBehaviour
{
    public string[] affectedDrums;
    private int drumPattern;
    private int i;


    // fonction pour les tambours du BOSS, appelé depuis forestbossplatform
    // itere dans chaque enfant de forestbossplatform, compare sa lettre
    // si elle correspond a celle de la public string  affectedDrums, ses points de vie sont reduit, si la plateforme arrive a 0 elle tombe
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
