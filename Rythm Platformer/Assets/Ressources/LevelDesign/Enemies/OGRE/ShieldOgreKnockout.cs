using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOgreKnockout : MonoBehaviour
{
    private GameObject ogreGameObject;
    private ShieldOgre ogreScript;

    // Start is called before the first frame update
    void Start()
    {
        ogreGameObject = this.gameObject.transform.parent.gameObject;
        ogreScript = this.gameObject.transform.parent.gameObject.GetComponent<ShieldOgre>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player" && !ogreScript.knockedDown && !ogreScript.shieldUp)
        {
            ogreScript.knockedDown = true;
            ogreScript.shield.tag = "Untagged";
            ogreScript.damageCollider.tag = "BOUNCE";
            //  ogreGameObject.transform.Rotate(0, 0, -90);
        }

    }
}
