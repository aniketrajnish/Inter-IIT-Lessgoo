using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboadScript : MonoBehaviour
{
    TextMeshProUGUI timer;
    float timeElapsed;
    void Start()
    {
        timer = GetComponentInChildren<TextMeshProUGUI>();
    }    
    void Update()
    {
        timeElapsed += Time.unscaledDeltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
