using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LBDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] leaderboardTextArray;
    private List<KeyValuePair<string, int>> scores = new List<KeyValuePair<string, int>>();
    public TextMeshProUGUI[] btns;
    private void Start()
    {
        SetLB(0);
    }
    public void SetLB(int level)
    {
        print(level);
        leaderboardTextArray = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < leaderboardTextArray.Length; i++)
        {
            leaderboardTextArray[i].text = (i + 1) + "Lorem Ispum";
        }

        for (int i = 0; i < PlayerPrefs.GetInt("scoreCount_" + level); i++)
        {
            scores.Add(new KeyValuePair<string, int>(PlayerPrefs.GetString("scoreName_" + level + "_" + i), PlayerPrefs.GetInt("scoreValue_" + level + "_" + i)));
        }

        // Sort the scores list in descending order based on the score values
        scores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        // Display the scores on the leaderboard
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
                
        foreach (TextMeshProUGUI btn in btns)
        {
            Color color = btn.color;
            color.a = .58f;
            btn.color = color;
        }


        if (btns[level] != null)
        {
            Color color = btns[level].color;
            color.a = 1;
            btns[level].color = color;
        }
    }    
}
