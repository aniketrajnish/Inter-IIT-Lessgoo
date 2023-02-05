using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LBDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] leaderboardTextArray;
    private List<KeyValuePair<string, int>> scores = new List<KeyValuePair<string, int>>();

    private void Start()
    {
        leaderboardTextArray = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i =0; i < leaderboardTextArray.Length; i++)
        {
            leaderboardTextArray[i].text = (i + 1) + "Lorem Ispum";
        }        

        for (int i = 0; i < PlayerPrefs.GetInt("scoreCount"); i++)
        {
            scores.Add(new KeyValuePair<string, int>(PlayerPrefs.GetString("scoreName_" + i), PlayerPrefs.GetInt("scoreValue_" + i)));
        }

        scores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
        for (int i = 0; i < scores.Count; i++)
        {
            if (i < leaderboardTextArray.Length)
            {
                leaderboardTextArray[i].text = (i + 1) + ". " + scores[i].Key + " - " + scores[i].Value;
            }
        }

        for (int i = scores.Count; i < leaderboardTextArray.Length; i++)
        {
            leaderboardTextArray[i].text = (i + 1) + ". " + "Lorem ipsum" + " - " + "0";
        }
    }


}
