using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    public GameObject pauseMenu;
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void ResumeButon()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        GameObject.Find("SadMusic").GetComponent<AudioSource>().volume = .2f;
        GameObject.Find("Player").GetComponent<AudioSource>().Play();
        GameObject.Find("Reflection").GetComponent<AudioSource>().Play();
    }

    public void MenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        GameObject.Find("Player").GetComponent<AudioSource>().Play();
        GameObject.Find("Reflection").GetComponent<AudioSource>().Play();
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject.Find("Player").GetComponent<AudioSource>().Play();
        GameObject.Find("Reflection").GetComponent<AudioSource>().Play();
    }
    void Update()
    {
        if(isPaused==false)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                GameObject.Find("SadMusic").GetComponent<AudioSource>().volume = 1f;
                GameObject.Find("Player").GetComponent<AudioSource>().Stop();
                GameObject.Find("Reflection").GetComponent<AudioSource>().Stop();
            }
        }
    }
}
