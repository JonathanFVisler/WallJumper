using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class LevelPlayer : MonoBehaviour
{
    [SerializeField]
    string levelName;
    BeatDegree beatDegree = BeatDegree.FAIL;

    [SerializeField]
    GameObject levelSelectManager;

    [SerializeField]
    bool isCollectAllCoins = false;
    [SerializeField]
    bool isOneCoinAtATime = false;
    [SerializeField]
    bool isDestination = false;

    [SerializeField]
    float[] goals;

    [SerializeField]
    bool hasPreRequiredLevel;
    [SerializeField]
    string preRequiredLevel;
    GameObject requiredLevel;

    [SerializeField]
    Color lockedColor = new Color32(171, 171, 171, 255);
    [SerializeField]
    Color unlockedColor = new Color32(255, 255, 255, 255);

    [SerializeField]
    GameObject starOne, starTwo, starThree;

    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        requiredLevel = GameObject.Find(preRequiredLevel + "Button");

        starOne.SetActive(false);
        starTwo.SetActive(false);
        starThree.SetActive(false);

        int temp = PlayerPrefs.GetInt(levelName + "beat degree");
        switch (temp)
        {
            case 1:
                beatDegree = BeatDegree.ONEStar;
                starOne.SetActive(true);
                break;

            case 2:
                beatDegree = BeatDegree.TWOStar;
                starOne.SetActive(true);
                starTwo.SetActive(true);
                break;

            case 3:
                beatDegree = BeatDegree.THREEStar;
                starOne.SetActive(true);
                starTwo.SetActive(true);
                starThree.SetActive(true);
                break;

            default:
                beatDegree = BeatDegree.FAIL; break;
        }


        if (BeatenPreRequiredLevel() || !hasPreRequiredLevel)
        {
            GetComponent<Image>().color = unlockedColor;
        }
        else
        {
            GetComponent<Image>().color = lockedColor;
        }

        
    }

    public void Update()
    {
        if (hasPreRequiredLevel)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, requiredLevel.transform.position);
        }
    }

    public void OnPress()
    {
        if (hasPreRequiredLevel)
        {
            if (!BeatenPreRequiredLevel())
            {
                Debug.Log("preRequiredLevel not beaten");
                return;
            }

        }
        
        levelSelectManager.GetComponent<LevelSelectManager>().UpdateUI(this);
    }

    public float GetHighscore()
    {
        return PlayerPrefs.GetFloat(levelName + "score");
    }

    public BeatDegree GetBeatDegree()
    {
        return beatDegree;
    }

    private bool BeatenPreRequiredLevel()
    {
        if (PlayerPrefs.GetInt(preRequiredLevel + "beat degree") < 1 || 3 < PlayerPrefs.GetInt(preRequiredLevel + "beat degree"))
        {
            return false;
        }

        return true;
    }

    public string GetLevelType()
    {
        if (isCollectAllCoins)
        {
            return "CollectAllCoins";
        }
        else if (isOneCoinAtATime)
        {
            return "OneCoinAtATime";
        }
        else if (isDestination)
        {
            return "Destination";
        }
        else
        {
            return "Type not assined (yes this is a bug)";
        }
    }

    public string GetLevelTypeMessage()
    {
        if (isCollectAllCoins)
        {
            return "Collect the coins in the Level";
        }
        else if (isOneCoinAtATime)
        {
            return "Collect as many coins as possible within the time limit";
        }
        else if (isDestination)
        {
            return "Reach the coin as quickly as possible";
        }
        else
        {
            return "Type not assined (yes this is a bug)";
        }
    }

    public float[] GetGoals()
    {
        return goals;
    }

    public string GetLevelName()
    {
        return levelName;
    }
}
