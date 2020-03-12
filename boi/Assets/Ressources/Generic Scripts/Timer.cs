using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float time;
    private bool endLevel;
    // Start is called before the first frame update
    void Start()
    {
        endLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (endLevel == false)
        {
            time += Time.deltaTime;
            // timeInt = Mathf.RoundToInt(time);
            GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}:{1:00}", Mathf.Floor(time / 60), time % 60);
            //GetComponent<Text>().text = Mathf.Round(time).ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            endLevel = true;
        
    }
}
