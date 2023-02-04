using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BGSoundScript : MonoBehaviour
{

    private void Start()
    {
        //GetComponent<AudioSource>().time = 10f;
    }
    private void Update()
    {
        GetComponent<AudioSource>().volume = (float)System.Math.Pow(Time.timeScale, .7f) * .05f;
    }
    /*float time;
    
	void Start () 
    {
        InvokeRepeating("PlayMusic", Random.Range(20f, 40f), Random.Range(110f, 130f));
    }

    //Play Global
    private static BGSoundScript instance = null;
    public static BGSoundScript Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            GetComponent<AudioSource>().volume = .2f;     
            GetComponent<AudioSource>().time = 9.5f;    
        }
        else
            GetComponent<AudioSource>().volume = 9.5f;

        if (SceneManager.GetActiveScene().name == "Level " + PlayerPrefs.GetInt("Level") || SceneManager.GetActiveScene().name=="Level 1")
            GetComponent<AudioSource>().Stop();
    }

    void PlayMusic()
    {
        GetComponent<AudioSource>().Play();
        Debug.Log("Music");
        int rand = Random.Range(0, 3);

        if (rand == 2)
            time = 24f;
        if (rand == 1)
            time = 0f;
        if (rand == 0)
            time = 47f;

        GetComponent<AudioSource>().time = time;
        Debug.Log(rand);
    }

    private void Update()
    {
        
    }*/
}
