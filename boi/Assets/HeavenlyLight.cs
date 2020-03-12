using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenlyLight : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;
    private float fullScale;
    public float minScale;
    public float vanishDuration;
    private static float vanishDurationStatic;
    private static float minScaleStatic = 0.1f;
    private Vector3 tmpScale;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        tmpScale = spriteRenderer.transform.localScale;
        vanishDurationStatic = vanishDuration;
        minScaleStatic = minScale;
        fullScale = tmpScale.x;
        tmpScale.x = fullScale;
        spriteRenderer.transform.localScale = tmpScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.enabled)
        {
            if (tmpScale.x < fullScale)
            {
                tmpScale.x += ((fullScale - minScaleStatic) / vanishDurationStatic) * Time.deltaTime;
                spriteRenderer.transform.localScale = tmpScale;
            }
            else
            {
                tmpScale.x = minScaleStatic;
                spriteRenderer.transform.localScale = tmpScale;
                spriteRenderer.enabled = false;
            }
        }
    }

    public void lightUp()
    {
        tmpScale.x = minScaleStatic;
        spriteRenderer.transform.localScale = tmpScale;
        spriteRenderer.enabled = true;
    }
}
