using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool levelFinished;
    private void Start()
    {
        instance = this;
    }
    public void Resume()
    {
        Time.timeScale = 1;
        CanvasManager.cmInstance.FindCanvas("CanvasGame").SetActive(false);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        CanvasManager.cmInstance.FindCanvas("CanvasGame").SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Play()
    {
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
}
