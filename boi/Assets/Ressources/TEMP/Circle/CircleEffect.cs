using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleEffect : MonoBehaviour 
{
    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float radius;
    public float endradius;
    private float startRadius;
    private LineRenderer lineRenderer;
    private FollowerOfTheRhythm tempo;
    private bool isResetting = false;
    private float windUpBeats = 3;
    private float growthPerBeat;
    public bool canMove = false;
    public int frames;
    Color blue = new Color(51f/255f, 115f / 255f, 184f / 255f, 1f);


    private void Start()
    {

        growthPerBeat =4f / windUpBeats / 5 ; 
        startRadius = radius;
        lineRenderer = GetComponent<LineRenderer>();
        tempo = GetComponent<FollowerOfTheRhythm>();
        lineRenderer.material.color = blue;
        setupCricle();
    }

    private void Update()
    {
        frames++;
        if (tempo.canMoveToRythm() )
        {

            if (!canMove)
            {
                canMove = true;
                frames = 0;
            }
        }
        if (frames >= 5 && canMove)
        {
            canMove = false;
        }

        if (canMove && radius <= endradius && isResetting == false)
        {
            radius += growthPerBeat;
        }
        if (radius > endradius)
            isResetting = true;
        if (isResetting && radius > startRadius)
        {
            lineRenderer.material.color = Color.red;
            radius -= 0.25f;
        }
        if (isResetting && radius <= startRadius)
        {
            radius = startRadius;
            canMove = false;
            lineRenderer.material.color = blue;
            isResetting = false;

        }

        setupCricle();

    }

    void setupCricle()
    {
        lineRenderer.widthMultiplier = lineWidth;
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;
        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f) + transform.position;
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = Vector3.zero;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;
            theta += deltaTheta;
        }
    }
#endif
}
