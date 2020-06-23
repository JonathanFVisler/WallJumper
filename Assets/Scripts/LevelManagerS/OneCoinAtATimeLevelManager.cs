using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class OneCoinAtATimeLevelManager : MonoBehaviour, LevelManagerStrategy
{

    int coinsCollected = 0;
    float timer = 0;
    float playTimer = 0;
    [SerializeField]
    float StartTimer = 60;
    string levelName;
    [SerializeField]
    float levelHeight;
    [SerializeField]
    float levelWidth;
    [SerializeField]
    GameObject coin;
    
    TextMeshProUGUI timeText;
    TextMeshProUGUI coinText;

    bool count = true;

    [SerializeField]
    GameObject levelEndMenu;
    [SerializeField]
    GameObject starOne, starTwo, starThree;
    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    GameObject player;

    [SerializeField]
    bool hasNextLevel;
    [SerializeField]
    string nextLevel;

    [SerializeField]
    AudioSource dingAudioSource;

    private void Start()
    {
        timeText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        timer = StartTimer;
        playTimer = 0;
        coinText.text = "Coins: 0";
        levelName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    private void Update()
    {
        if (count) {
            timer -= Time.deltaTime;
            playTimer += Time.deltaTime;

            timeText.text = "Time:" + timer.ToString("F2");

            if (timer <= 0)
            {
                count = false;
                DisplayLevelEndMenu();
            }
        }
    }

    public BeatDegree BeatLevel()
    {
        int currentBeatDegree = PlayerPrefs.GetInt(levelName + "beat degree");
        if (coinsCollected < Memory.GoalMemory[0])
        {
            return BeatDegree.FAIL;
        }
        if (coinsCollected < Memory.GoalMemory[1])
        {
            return BeatDegree.ONEStar;
        }
        if (coinsCollected < Memory.GoalMemory[2])
        {
            return BeatDegree.TWOStar;
        }
        return BeatDegree.THREEStar;
    }

    public void CoinPickup()
    {
        coinsCollected++;
        coinText.text = "Coins: " + coinsCollected;
        SpawnCoin();
    }

    private void SpawnCoin()
    {
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-levelWidth/2, levelWidth/2), UnityEngine.Random.Range(-levelHeight / 2, levelHeight / 2), 0);
        GameObject coin = Instantiate(this.coin, spawnPos, this.coin.transform.rotation);
        if (coin.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), new Collider2D[2]) != 0)
        {
            Destroy(coin);
            SpawnCoin();
        }

    }

    public float GetHighScore()
    {
        return PlayerPrefs.GetFloat(levelName + "score");
    }

    public float GetScore()
    {
        return coinsCollected;
    }

    public float GetTimeToBeat()
    {
        return playTimer;
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void SaveScore()
    {
        if (coinsCollected > GetHighScore())
        {
            PlayerPrefs.SetFloat(levelName + "score", coinsCollected);
        }
    }

    public void EndLevel()
    {
        SaveScore();
        SaveBeatDegree();
        if (hasNextLevel)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            SceneManager.LoadScene("LevelSelectScene");
        }
    }

    public void FailLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    private void SaveBeatDegree()
    {
        int oldBeatDegree = PlayerPrefs.GetInt(levelName + "beat degree");
        BeatDegree newBeatDegree = BeatLevel();

        if (newBeatDegree == BeatDegree.ONEStar && oldBeatDegree < 2)
        {
            PlayerPrefs.SetInt(levelName + "beat degree", 1);
            Memory.increaseStars(Mathf.Max(0, 1 - oldBeatDegree));
            Debug.Log(Memory.TotalStarMemory);
        }
        else if (newBeatDegree == BeatDegree.TWOStar && oldBeatDegree < 3)
        {
            PlayerPrefs.SetInt(levelName + "beat degree", 2);
            Memory.increaseStars(Mathf.Max(0, 2 - oldBeatDegree));
            Debug.Log(Memory.TotalStarMemory);
        }
        else if (newBeatDegree == BeatDegree.THREEStar)
        {
            PlayerPrefs.SetInt(levelName + "beat degree", 3);
            Memory.increaseStars(Mathf.Max(0, 3 - oldBeatDegree));
            Debug.Log(Memory.TotalStarMemory);
        }
    }

    public void DisplayLevelEndMenu()
    {
        levelEndMenu.SetActive(true);

        SaveBeatDegree();
        SaveScore();

        player.GetComponent<Player>().StopMoving();
        player.GetComponent<BoxCollider2D>().enabled = false;

        scoreText.text = "Score: " + coinsCollected.ToString("F2") + "Sec";

        switch (BeatLevel())
        {
            case BeatDegree.FAIL:
                starOne.SetActive(false);
                starTwo.SetActive(false);
                starThree.SetActive(false);
                break;

            case BeatDegree.ONEStar:
                Invoke("PlayDingAudio", 0.4f);
                starOne.SetActive(true);
                starOne.GetComponent<Animation>().Play();
                starTwo.SetActive(false);
                starThree.SetActive(false);
                PlayDingAudio();
                break;

            case BeatDegree.TWOStar:
                Invoke("PlayDingAudio", 0.4f);
                Invoke("PlayDingAudio", 1.1f);
                starOne.SetActive(true);
                starTwo.SetActive(true);
                starTwo.GetComponent<Animation>().Play();
                starThree.SetActive(false);
                break;

            case BeatDegree.THREEStar:
                Debug.Log("Got 3 stars");
                Invoke("PlayDingAudio", 0.4f);
                Invoke("PlayDingAudio", 1.1f);
                Invoke("PlayDingAudio", 1.8f);
                starOne.SetActive(true);
                starTwo.SetActive(true);
                starThree.SetActive(true);
                starThree.GetComponent<Animation>().Play();
                break;
        }
    }

    public void PlayDingAudio()
    {
        Debug.Log("Ding");
        dingAudioSource.Play();
    }

    public void PlayAgain()
    {
        SaveBeatDegree();
        SaveScore();
        SceneManager.LoadScene(levelName);
    }
}
