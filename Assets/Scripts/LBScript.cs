using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LBScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer, finalScore, time, levelIndicator;
    public float timeElapsed;
    float targetTime = 180;
    public bool scoreAssigned;
    public static LBScript instance;
    string finTime;
    void Start()
    {
        timeElapsed = 0;
        instance = this;
        levelIndicator.text = "Level " + PlayerPrefs.GetInt("CurrLevel", 0).ToString();
    }
    void Update()
    {
        if (!GameManager.instance.isPaused)        
            Clock(timer);           
        
        if (!scoreAssigned && GameManager.instance.levelFinished)
        {
            print("XD");
            GameManager.instance.PauseSettings();
            int score = AssignScore(timeElapsed);
            finalScore.text = "Score: " + score.ToString();
            print("Final " + timer.text);
            CanvasManager.cmInstance.FindCanvas("CanvasScore").SetActive(true);
            time.text = "Time Elapsed: " + timer.text;
            timer.gameObject.SetActive(false);
            scoreAssigned = true;
        }
        finTime = timer.text;
    }
    void Clock(TextMeshProUGUI _timer)
    {
        timeElapsed += Time.unscaledDeltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        _timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    public int AssignScore(float _timeElapsed)
    {
        int score = (int)(200 * (1 - (_timeElapsed - targetTime) / targetTime));
        if (score < 50) score = 50;

        return score;
    }   
}