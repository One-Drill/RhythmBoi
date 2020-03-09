using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autodestroy : MonoBehaviour
{
    public float delay;
    private void Update()
    {
        Destroy(this.gameObject, delay);
    }
}