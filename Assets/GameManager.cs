using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
}
