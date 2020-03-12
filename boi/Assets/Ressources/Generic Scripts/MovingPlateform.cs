using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public GameObject player;
    public bool isOnPlat;

    private void Update()
    {
        MovePlat();
    }
                               //Fonctionnement
    // j'initialise un raycast qui check le tag de la plateforme et la distance avec le player, si celui-ci correspond au tag "MovingPlatform" et que le player est colle a la plateforme
    // il set la plateformeen tant que parent du Player , si il saute de la plateforme le parent est set a null, Player redevient donc independant.
                               //Utilisation
    //associer ce script a un script de deplacement en appelant la fonction Move ci-dessous a chaque deplacement au lieu de le mettre dans une boucle update(comme ci-dessus) serait judicieux, Miguel out.
    private void MovePlat()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.down);

            if (hit.collider != null && hit.collider.tag == "MovingPlatform")
            {
                if (hit.distance < 1 && !isOnPlat)
                {
                    player.transform.SetParent(transform);
                    isOnPlat = true;

                }
                if (hit.distance > 1 && isOnPlat)
                {
                    player.transform.SetParent(null);
                    isOnPlat = false;
                }
            }
            else
            {
                if (isOnPlat)
                {
                    player.transform.SetParent(null);
                    isOnPlat = false;
                }
            }
        }
}
