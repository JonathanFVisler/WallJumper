using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    TextMeshProUGUI typeText;
    [SerializeField]
    TextMeshProUGUI highscoreText;
    [SerializeField]
    TextMeshProUGUI beatDegreeText;
    [SerializeField]
    GameObject playButton;


    [SerializeField]
    GameObject[] menuUI;
    [SerializeField]
    GameObject loadingUI;
    [SerializeField]
    GameObject settingsUI;

    [SerializeField]
    GameObject[] stars = new GameObject[3];

    AdController adController;

    string levelName;
    float[] goals;

    static string selectedLevel;

    public void Start()
    {
        Memory.init();
        adController = GetComponent<AdController>();
        adController.IncreaseCounter();

        levelText.gameObject.SetActive(false);
        typeText.gameObject.SetActive(false);
        highscoreText.gameObject.SetActive(false);
        beatDegreeText.gameObject.SetActive(false);
        playButton.SetActive(false);
        stars[0].SetActive(false);
        stars[1].SetActive(false);
        stars[2].SetActive(false);
    }

    public void UpdateUI(string levelName, BeatDegree beatDegree, float highscore, float[] goals, string typeMessage)
    {
        levelText.gameObject.SetActive(true);
        typeText.gameObject.SetActive(true);
        highscoreText.gameObject.SetActive(true);
        beatDegreeText.gameObject.SetActive(true);
        playButton.SetActive(true);

        this.goals = goals;
        this.levelName = levelName;

        levelText.text = levelName;
        highscoreText.text = "Highscore: " + highscore.ToString("F2");

        string tempText;
        tempText = "1 Star: " + goals[0].ToString();
        for (int i = 1; i < goals.Length; i++)
        {
            tempText = tempText + "\n" + (i+1).ToString() + " Stars: " + goals[i].ToString();
        }
        beatDegreeText.text = tempText;
        selectedLevel = levelName;

        switch (beatDegree)
        {
            case BeatDegree.FAIL:
                ShowStars(-1);
                break;

            case BeatDegree.ONEStar:
                ShowStars(0);
                break;

            case BeatDegree.TWOStar:
                ShowStars(1);
                break;

            case BeatDegree.THREEStar:
                ShowStars(2);
                break;

        }

        typeText.text = "Goal: " + typeMessage;
    }

    public void UpdateUI(LevelPlayer levelPlayer)
    {
        levelText.gameObject.SetActive(true);
        typeText.gameObject.SetActive(true);
        highscoreText.gameObject.SetActive(true);
        beatDegreeText.gameObject.SetActive(true);
        playButton.SetActive(true);

        this.goals = levelPlayer.GetGoals();
        this.levelName = levelPlayer.GetLevelName();

        levelText.text = levelName;
        highscoreText.text = "Highscore: " + levelPlayer.GetHighscore().ToString("F2");

        switch (levelPlayer.GetLevelType())
        {
            case "CollectAllCoins":
                highscoreText.text = highscoreText.text + " sec";
                break;
            case "OneCoinAtATime":
                highscoreText.text = highscoreText.text + " coins";
                break;
            case "Destination":
                highscoreText.text = highscoreText.text + " sec";
                break;
        }

        string tempText;
        tempText = "1 Star: " + goals[0].ToString();
        for (int i = 1; i < goals.Length; i++)
        {
            tempText = tempText + "\n" + (i + 1).ToString() + " Stars: " + goals[i].ToString();
        }
        beatDegreeText.text = tempText;
        selectedLevel = levelName;

        switch (levelPlayer.GetBeatDegree())
        {
            case BeatDegree.FAIL:
                ShowStars(-1);
                break;

            case BeatDegree.ONEStar:
                ShowStars(0);
                break;

            case BeatDegree.TWOStar:
                ShowStars(1);
                break;

            case BeatDegree.THREEStar:
                ShowStars(2);
                break;

        }
        typeText.text = "Goal: " + levelPlayer.GetLevelTypeMessage();
    }

    private void ShowStars(int number)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if(i <= number)
            {
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
    }

    public void PlayLevel()
    {
        foreach (GameObject item in menuUI)
        {
            item.SetActive(false);
        }
        loadingUI.SetActive(true);
        //PlayerPrefs.SetInt("TimesMovedCamera", Camera.main.GetComponent<CameraNavigation>().GetTimesMoved());
        SaveGoalInfo();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            yield return null;
        }
    }

    private void SaveGoalInfo()
    {
        Memory.GoalMemory[0] = goals[0];
        Memory.GoalMemory[1] = goals[1];
        Memory.GoalMemory[2] = goals[2];
    }
    
    public void PressedBack()
    {
        foreach (GameObject item in menuUI)
        {
            item.SetActive(true);
        }
        settingsUI.SetActive(false);
    }

    public void PressedSettings()
    {
        foreach (GameObject item in menuUI)
        {
            item.SetActive(false);
        }
        settingsUI.SetActive(true);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LevelSelectScene");
    }
}
