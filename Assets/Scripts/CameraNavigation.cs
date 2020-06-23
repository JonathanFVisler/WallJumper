using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNavigation : MonoBehaviour
{
    float jumpDist;
    int timesMoved;
    [SerializeField]
    int maxMoveTimes;

    [SerializeField]
    GameObject upButton;
    [SerializeField]
    GameObject downButton;

    [SerializeField]
    GameObject background;

    private void Start()
    {
        jumpDist = GetComponent<Camera>().orthographicSize * 1.3f;
        downButton.SetActive(false);

        int preTimesMoved = PlayerPrefs.GetInt("TimesMovedCamera");
        for (int i = 0; i < preTimesMoved; i++)
        {
            NavigateUp();
        }
    }

    public void NavigateUp()
    {
        if(timesMoved < maxMoveTimes)
        {
            Vector3 moveDist = new Vector3(0, jumpDist, 0);
            timesMoved++;
            Navigate(moveDist);
            downButton.SetActive(true);
            if(timesMoved == maxMoveTimes)
            {
                upButton.SetActive(false);
            }
        }
    }

    public void NavigateDown()
    {
        if(timesMoved > 0)
        {
            Vector3 moveDist = new Vector3(0, -jumpDist, 0);
            timesMoved--;
            Navigate(moveDist);
            upButton.SetActive(true);
            if (timesMoved == 0)
            {
                downButton.SetActive(false);
            }
        }
    }

    private void Navigate(Vector3 moveDist)
    {
        transform.Translate(moveDist);
        background.transform.Translate(moveDist, Space.World);
    }

    public int GetTimesMoved() { return timesMoved; }
}
