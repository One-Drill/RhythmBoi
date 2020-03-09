using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOgre : MonoBehaviour
{
    // IMPORTANT!! DO NOT CHANGE FIRST CHILD OF THIS OBJECT, IT NEEDS TO BE THE BODY THAT DEALS DAMAGE.
    public bool knockedDown = false;
    public int shieldBeats;
    private int getUpCounter = 0;
    private int shieldBeatCounter = 0;
    public int BeatsKnockedDown;
    public GameObject damageCollider;
    public GameObject shield;
    private FollowerOfTheRhythm tempo;
    private SpriteRenderer shieldRenderer;
    public bool shieldUp = false;
    private SpriteRenderer ogreRenderer;
    public Sprite shieldDownSprite;
    public Sprite shieldUpSprite;


    // Start is called before the first frame update
    void Start()
    {
        ogreRenderer = GetComponent<SpriteRenderer>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        damageCollider =  this.gameObject.transform.GetChild(0).gameObject;
        shield = this.gameObject.transform.GetChild(1).gameObject;
        shieldRenderer = shield.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockedDown)
        {
           // shield.tag = "Untagged";
            this.gameObject.transform.eulerAngles = new Vector3(0, 0, -90);
            shieldRenderer.enabled = false;
         //   damageCollider.tag = "BOUNCE";
            if (tempo.canMoveToRythm())
                getUpCounter++;
            if (getUpCounter == BeatsKnockedDown)
            {
                shieldRenderer.enabled = true;
                knockedDown = false;
                changeShieldPosition();
                damageCollider.tag = "FATAL";
                shield.tag = "FATAL";
                getUpCounter = 0;
                this.gameObject.transform.Rotate(0, 0, 90);
            }
        }
        else
        {
            if (tempo.canMoveToRythm())
                shieldBeatCounter++;
            if (shieldBeatCounter >= shieldBeats)
            {
                shieldBeatCounter = 0;
                changeShieldPosition();
            }
        }
    }
    void changeShieldPosition()
    {
        if (shieldUp)
        {
            ogreRenderer.sprite = shieldDownSprite;
            shield.transform.Rotate(0, 0, 90);
            shield.transform.Translate(-1.65f, -1.9f, 0);
            shieldUp = false;
        }
        else
        {
            ogreRenderer.sprite = shieldUpSprite;
            shield.transform.Translate(1.65f, 1.9f, 0);
            shield.transform.Rotate(0, 0, -90);
            shieldUp = true;
        }
    }
 }
