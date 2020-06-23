using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarDisplay : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI starText;

    void Start()
    {
        starText.text = ": " + Memory.TotalStarMemory.ToString();
    }
}
