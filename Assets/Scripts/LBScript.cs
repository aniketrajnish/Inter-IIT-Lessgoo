using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LBScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer, finalScore, time;
    public float timeElapsed;
    float targetTime = 100;
    public bool scoreAssigned;
    public static LBScript instance;
    void Start()
    {
        timeElapsed = 0;
        instance = this;
    }
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            Clock(timer);
            //print(timeElapsed);
            if (!scoreAssigned && GameManager.instance.levelFinished)
            {
                GameManager.instance.PauseSettings();
                int score = AssignScore(timeElapsed);
                finalScore.text = "Score: " + score.ToString();
                CanvasManager.cmInstance.FindCanvas("CanvasScore").SetActive(true);
                time.text = "Time Elapsed: " + timer.text;
                timer.gameObject.SetActive(false);
                scoreAssigned = true;
            }
        }
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