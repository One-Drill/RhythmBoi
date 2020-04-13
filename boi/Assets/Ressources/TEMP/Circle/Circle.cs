using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Circle : MonoBehaviour 
{
    public int vertexCount;
    public float lineWidth = 0.2f;
    public float radius;
    private LineRenderer lineRenderer;
    private FollowerOfTheRhythm tempo;
    public Vector3[] vertex;
    public GameObject[] swords;
    private Notes[] indexes;
    public GameObject sword;
    private float spb;
    private float t;
    private float waitTime;
    private GameObject poss;
   
    private void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        swords = new GameObject[vertexCount];
        lineRenderer = GetComponent<LineRenderer>();
        vertex = new Vector3[vertexCount];
        indexes = new Notes[vertexCount];
        setupCricle();

        for (int i = 0; i <= vertex.Length -1; i++)
        {
            swords[i] = Instantiate(sword, vertex[i], Quaternion.identity); 
            if (i != vertex.Length - 1)
            {

                LookAt2D(vertex[i + 1], swords[i].transform);
                indexes[i] = swords[i].GetComponent<Notes>();
                indexes[i].startIndex = i;
                indexes[i].swordNb = i;
                indexes[i].endIndex = i + 1;
            }
            else
            {
                LookAt2D(vertex[0], swords[i].transform);
                indexes[i] = swords[i].GetComponent<Notes>();
                indexes[i].startIndex = i;
                indexes[i].swordNb = i;
                indexes[i].endIndex = 0;

            }

        }
    }

    private void LookAt2D(Vector2 coord, Transform sword)
    {
        // LookAt 2D
        Vector3 target = (Vector3)coord;
        // get the angle
        Vector3 norTar = (target - sword.position).normalized;
        float angle = Mathf.Atan2(norTar.y, norTar.x) * Mathf.Rad2Deg;
        // rotate to angle
        Quaternion rotation = new Quaternion(); 
        rotation.eulerAngles = new Vector3(0, 0, angle - 90);
        sword.rotation = rotation;
    }

    private float GetAngle(Vector2 coord, Transform sword)
    {
        Vector3 target = (Vector3)coord;
        Vector3 norTar = (target - sword.position).normalized;
        float angle = Mathf.Atan2(norTar.y, norTar.x) * Mathf.Rad2Deg;
        return angle;
    }

    private void Update()
    {
        spb = 60 / tempo.getBpm();
        t += Time.deltaTime / spb * 1.1f;
        for (int i = 0; i < swords.Length; i++)
        {
            if (swords[i].transform.position.x != vertex[indexes[i].endIndex].x && swords[i].transform.position.y != vertex[indexes[i].endIndex].x)
            {
                Vector2 lookPoint;
                lookPoint.x  = (vertex[indexes[i].endIndex].x + swords[i].transform.position.x) / 2;
                lookPoint.y  = (vertex[indexes[i].endIndex].y + swords[i].transform.position.y) / 2;
                float angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0, 0, 0)), swords[i].transform.rotation);
                float nextangle = Mathf.LerpAngle(angle, maxAngle, Time.time);

                LookAt2D(lookPoint, swords[i ].transform);
                swords[i].transform.position = Vector3.Lerp(vertex[indexes[i].startIndex], vertex[indexes[i].endIndex], t);
            }
            else
            {
                print("Snb: " + i + " S: " + swords[i].transform.position + " E: " + vertex[indexes[i].endIndex]);
                indexes[i].startIndex = indexes[i].endIndex;
                if (indexes[i].endIndex < swords.Length - 1)
                    indexes[i].endIndex++;
                else
                    indexes[i].endIndex = 0;
                print("Snb: " + indexes[i].swordNb + " S: " + swords[i].transform.position + " E: " + vertex[indexes[i].endIndex]);

                t = 0;
            }
        }

    }

    private void setupCricle()
    {
        lineRenderer.widthMultiplier = lineWidth;
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;
        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f) + transform.position;
            vertex[i] = pos;
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = transform.position;
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
