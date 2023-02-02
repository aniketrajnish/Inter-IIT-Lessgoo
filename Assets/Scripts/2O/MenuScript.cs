using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Play").GetComponent<Button>().enabled = false;
        GameObject.Find("Quit").GetComponent<Button>().enabled = false;
        GameObject.Find("Reset").GetComponent<Button>().enabled = false;
        GameObject.Find("GirlSprite_0").GetComponent<Animator>().Play("GirlWalk");
        
        GameObject.Find("PlayerSprite_0").GetComponent<Animator>().Play("PlayerWalk");    
        
    }

    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("Level") != 0)
            SceneManager.LoadScene("Level " + PlayerPrefs.GetInt("Level"));
        else
            SceneManager.LoadScene("Level 1");
    }

    public void ResetGame()
    {
        PlayerPrefs.SetInt("Level",1);
    }

    void Update()
    {   
        if(Time.timeSinceLevelLoad > 1f && Time.timeSinceLevelLoad <7.5f)
        {
            GameObject.Find("Player").GetComponent<AudioSource>().enabled = true;
        }
        if (Time.timeSinceLevelLoad>7.5f)
        {
            GameObject.Find("Play").GetComponent<Button>().enabled = true;
            GameObject.Find("Quit").GetComponent<Button>().enabled = true;
            GameObject.Find("Reset").GetComponent<Button>().enabled = true;
            GameObject.Find("GirlSprite_0").GetComponent<Animator>().Play("New State");
            GameObject.Find("PlayerSprite_0").GetComponent<Animator>().Play("New State");
            GameObject.Find("Player").GetComponent<AudioSource>().enabled = false;
        }
    }
}
