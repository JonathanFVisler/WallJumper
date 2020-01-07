using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    string playerName;
    public TextMeshProUGUI playerNameInputField;

    public bool useTimer = true;
    public float timer;
    public float staticTimer;
    public TextMeshProUGUI timerText;
    public int coinsCollected = 0;
    public TextMeshProUGUI coinText;

    public int mapHeigth;
    public int mapWidth;

    public GameObject coin;

    public Camera mainMenuCamera;
    public GameObject[] sceneUI;
    public TextMeshProUGUI[] highscoreUI;
    public TextMeshProUGUI postGameCoinDisplay;

    public float musicVolumne;
    public float soundEffectVolumne;

    public Slider musicSlider;
    public Slider soundEffectSlider;

    public string deviceType;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        staticTimer = timer;

        int tempScore;
        string tempName;

        for (int i = 0; i < highscoreUI.Length; i++)
        {
            if(i % 2 == 0) 
            {
                tempName = PlayerPrefs.GetString(i.ToString() + "name", "None");
                highscoreUI[i].text = tempName + "  |";
            }
            else if(i % 2 == 1)
            {
                tempScore = PlayerPrefs.GetInt(i.ToString() + "score", 0);
                highscoreUI[i].text = tempScore.ToString();
            }
            
        }

        if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            deviceType = "Desktop";
        }
        else
        {
            deviceType = "Handheld";
        }

        musicVolumne = PlayerPrefs.GetFloat("MusicVolumne", 0);
        musicSlider.value = musicVolumne;
        soundEffectVolumne = PlayerPrefs.GetFloat("SoundEffectVolumne", 0);
        soundEffectSlider.value = soundEffectVolumne;
    }


    // Update is called once per frame
    void Update()
    {
        if (useTimer)
        {
            if (SceneManager.GetSceneByName("SampleScene").isLoaded) 
            {
                timer -= Time.deltaTime;
                int timerDisplay = (int)timer;
                timerText.text = timerDisplay.ToString();

                if (timer <= 0)
                {
                    TimesOut();
                } 
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressedEscape();
        }
    }

    public void CoinPickup()
    {
        timer += 3;
        coinsCollected++;
        coinText.text = ": " + coinsCollected.ToString();
        SpawnCoin();
    }

    public void SpawnCoin()
    {
        float spawnXPos = Random.Range(-(mapWidth-1)/2, (mapWidth - 1) / 2);
        float spawnYPos = Random.Range(-(mapHeigth-1) / 2, (mapHeigth - 1) / 2);

        Instantiate(coin, new Vector3(spawnXPos, spawnYPos, 0), transform.rotation);
    }

    public void PressedPlay()
    {
        Debug.Log("Pressed Play");
        playerName = playerNameInputField.text;
        coinsCollected = 0;
        coinText.text = ": " + coinsCollected.ToString();
        timer = staticTimer;
        SetupLoadedScene(0);
        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("Coin"))
        {
            Destroy(coin);
        }
        mainMenuCamera.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    void SetupLoadedScene(int index)
    {
        for (int i = 0; i < sceneUI.Length; i++)
        {
            if (i == index)
            {
                sceneUI[i].SetActive(true);
            }
            else
            {
                sceneUI[i].SetActive(false);
            }
        }
    }

    public void PressedSettings()
    {
        SetupLoadedScene(3);
    }

    public void PressedEscape()
    {
        TimesOut();
    }

    public void PressedMainMenu()
    {
        SetupLoadedScene(1);
    }

    public void PressedQuit()
    {
        Application.Quit();
    }

    void TimesOut()
    {
        SceneManager.UnloadScene(1);
        mainMenuCamera.gameObject.SetActive(true);
        SetupLoadedScene(2);
        postGameCoinDisplay.text = "You Got " + coinsCollected + " coins";
        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 1)
            {
                if (PlayerPrefs.GetInt(i.ToString() + "score", 0) < coinsCollected)
                {
                    MoveAllDownOnLeaderboard(i);
                    break;
                }
            }
        }
    }

    void MoveAllDownOnLeaderboard(int index)
    {
        string tempName = PlayerPrefs.GetString((index - 1).ToString() + "name", "None");
        int tempScore = PlayerPrefs.GetInt(index.ToString() + "score", 0);

        int tempScore2;
        string tempName2;

        PlayerPrefs.SetString((index - 1).ToString() + "name", playerName);
        PlayerPrefs.SetInt((index).ToString() + "score", coinsCollected);

        highscoreUI[index - 1].text = playerName + "  |";
        highscoreUI[index].text = coinsCollected.ToString();
        
        for (int i = index+1; i < 6; i++)
        {
            if(i % 2 == 0)
            {
                tempName2 = PlayerPrefs.GetString(i.ToString() + "name", "None");
                PlayerPrefs.SetString(i.ToString() + "name", tempName);
                highscoreUI[i].text = tempName + "  |";
                tempName = tempName2;
            }

            if(i % 2 == 1)
            {
                tempScore2 = PlayerPrefs.GetInt((i).ToString() + "score", 0);
                PlayerPrefs.SetInt((i).ToString() + "score", tempScore);
                highscoreUI[i].text = tempScore.ToString();
                tempScore = tempScore2;
            }
        }
    }

    public void SetMusicVolumne()
    {
        musicVolumne = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolumne", musicVolumne);
    }

    public void SetSoundEffectVolumne()
    {
        soundEffectVolumne = soundEffectSlider.value;
        PlayerPrefs.SetFloat("SoundEffectVolumne", soundEffectVolumne);
    }
}
