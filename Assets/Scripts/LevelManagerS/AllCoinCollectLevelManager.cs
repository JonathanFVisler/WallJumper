using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AllCoinCollectLevelManager : MonoBehaviour, LevelManagerStrategy
{
    [SerializeField]
    int maxScore;
    int coinsCollected = 0;
    float playTimer = 0;
    [SerializeField]
    float StartTimer = 60;
    string levelName;
    [SerializeField]
    float levelHeight;
    [SerializeField]
    float levelWidth;

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
    bool hasNextLevel;
    [SerializeField]
    string nextLevel;

    [SerializeField]
    AudioSource dingAudioSource;

    private void Awake()
    {
        levelName = SceneManager.GetActiveScene().name;
        timeText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        coinText.text = "Coins: 0/" + maxScore;
        playTimer = 0;
    }

    private void Update(){
        if (count)
        {
            playTimer += Time.deltaTime;
            timeText.text = "Time: " + playTimer.ToString("F2");
        }
    }

    public BeatDegree BeatLevel(){  
        if(playTimer < Memory.GoalMemory[2]){
            return BeatDegree.THREEStar;
        }
        else if (playTimer < Memory.GoalMemory[1])
        {
            return BeatDegree.TWOStar;
        }
        else if (playTimer < Memory.GoalMemory[0])
        {
            return BeatDegree.ONEStar;
        }
        return BeatDegree.FAIL;
    }

    public float GetTimeToBeat(){
        return playTimer;
    }

    public float GetScore(){
        return playTimer;
    }

    public float GetHighScore(){
        return PlayerPrefs.GetFloat(levelName + "score");
    }

    public void SaveScore()
    {
        if(playTimer < GetHighScore())
        {
            PlayerPrefs.SetFloat(levelName + "score", playTimer);
        }
        else if (GetHighScore() == 0)
        {
            PlayerPrefs.SetFloat(levelName + "score", playTimer);
        }
    }

    public void SaveBeatDegree()
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

    public void PlayLevel()
    {
        SceneManager.LoadScene(levelName);
        
    }

    public void CoinPickup(){
        coinsCollected++;
        coinText.text = "Coins: " + coinsCollected + "/" + maxScore;
        if(coinsCollected >= maxScore)
        {
            DisplayLevelEndMenu();
        }
        //SpawnCoin();
    }

    private void SpawnCoin(){   }

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

    public void DisplayLevelEndMenu()
    {
        levelEndMenu.SetActive(true);

        count = false;

        SaveBeatDegree();
        SaveScore();

        GameObject.Find("Player").GetComponent<Player>().StopMoving();
        GameObject.Find("Player").GetComponent<BoxCollider2D>().enabled = false;

        scoreText.text = "Score: " + playTimer.ToString("F2") + "Sec";

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