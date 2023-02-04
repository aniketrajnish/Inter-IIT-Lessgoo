using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboadScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    float timeElapsed;
    float targetTime = 100;    
    void Start()
    {
        
    }    
    void Update()
    {
        Clock(timer);
    }
    void Clock(TextMeshProUGUI _timer)
    {
        timeElapsed += Time.unscaledDeltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        _timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    int AssignScore(float _timeElapsed)
    {
        return (int)(100 * (1 - (_timeElapsed - targetTime) / targetTime));
    }

}
