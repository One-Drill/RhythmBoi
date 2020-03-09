using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver : MonoBehaviour
{
    public int puzzlePlatformNumber;
    private int n = 0;
    public GameObject puzzle;
    private bool isPuzzleDone = true;
    private bool puzzleComplete = false;
    public GameObject doorOne;
    public GameObject doorTwo;

    void Start()
    {
        puzzle = GameObject.Find("FirstPuzzle");
    }

    void Update()
    {
        if (!puzzleComplete)
        {
            while (n < puzzlePlatformNumber)
            {
                if (puzzle.transform.GetChild(n).gameObject.GetComponent<ActivatablePlatform>().isActivated == false)
                    isPuzzleDone = false;
                n++;
            }
            if (isPuzzleDone == true)
                puzzleIsDone();
            isPuzzleDone = true;
            n = 0;
        }
    }

    void puzzleIsDone()
    {
        puzzleComplete = true;
        ActivatablePlatform.puzzleComplete = true;
        Destroy(doorOne);
        Destroy(doorTwo);
        /*
        n = 0;
        while (n < puzzlePlatformNumber)
        {
            puzzle.transform.GetChild(n).gameObject.GetComponent<ActivatablePlatform>().puzzleComplete = true;
            n++;
        }*/

    }
}