using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool levelFinished;
    public bool isPaused;
    private void Start()
    {
        instance = this;
        PauseSettings();
    }
    public void Resume()
    {
        PlaySettings();
        CanvasManager.cmInstance.FindCanvas("CanvasGame").SetActive(false);
    }
    public void Restart()
    {
        PauseSettings();
        CanvasManager.cmInstance.FindCanvas("CanvasGame").SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Play()
    {
        PlaySettings();
        CanvasManager.cmInstance.FindCanvas("Leaderboard").SetActive(true);
        CanvasManager.cmInstance.FindCanvas("CanvasMenu").SetActive(false);
    }
    public void Quit()
    {
        Invoke("ActuallyQuit", .25f);
    }
    public void ActuallyQuit()
    {
        Application.Quit();
    }
    public void SaveScore()
    {
        TMP_InputField pn = FindObjectOfType<TMP_InputField>();

        int scoreCount = PlayerPrefs.GetInt("scoreCount", 0);
        PlayerPrefs.SetInt("scoreCount", scoreCount + 1);

        PlayerPrefs.SetString("scoreName_" + scoreCount, pn.text);
        PlayerPrefs.SetInt("scoreValue_" + scoreCount, LBScript.instance.AssignScore(LBScript.instance.timeElapsed));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseSettings();
            CanvasManager.cmInstance.FindCanvas("CanvasGame").SetActive(true);
        }
    }
    void PlaySettings()
    {
        isPaused = false;
        Time.timeScale = 1;
        GameObject.FindObjectOfType<TSIController>().GetComponent<TSIController>().enabled = true;
        GameObject.FindObjectOfType<TSIReflect>().GetComponent<TSIReflect>().enabled = true;
    }
    public void PauseSettings()
    {
        Time.timeScale = 0;       
        isPaused = true;
        if (GameObject.FindObjectOfType<TSIController>() != null)
            GameObject.FindObjectOfType<TSIController>().GetComponent<TSIController>().enabled = false;
        if (GameObject.FindObjectOfType<TSIReflect>() != null)
            GameObject.FindObjectOfType<TSIReflect>().GetComponent<TSIReflect>().enabled = false;
    }
}
