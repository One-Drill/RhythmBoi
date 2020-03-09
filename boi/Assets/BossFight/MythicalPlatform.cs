using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythicalPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public char letter;
    private SpriteRenderer spriteRenderer;
    public bool colorChanged;
    private Color originalColor;
    private float i;
    private float lightTime = 0.1f;

    void Awake()
    {
        i = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (colorChanged)
            i += Time.deltaTime;
        if (i >= lightTime)
        {
            i = 0;
            colorChanged = false;
            spriteRenderer.color = originalColor;
        }
    }
    public void Success()
    {
        //spriteRenderer.color = Color.green;
       // colorChanged = true;
    }

    public void Failure()
    {
        //colorChanged = true;
        //spriteRenderer.color = Color.red;
    }
}
