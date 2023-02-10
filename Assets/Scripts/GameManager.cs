using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool levelFinished;
    public bool isPaused;
    private void Awake()
    {
        instance = this;
        PauseSettings();
        Time.timeScale = 1;
        GetComponentInChildren<Light2D>().gameObject.SetActive(false);
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
    public void ShowLeaderboard()
    {
        PauseSettings();
        
        
        foreach (GameObject canvas in CanvasManager.cmInstance.canvases)
            canvas.SetActive(false);

        GameObject lb = CanvasManager.cmInstance.FindCanvas("Leaderboard");

        lb.SetActive(true);
        lb.transform.GetChild(0).gameObject.SetActive(false);
        lb.transform.GetChild(1).gameObject.SetActive(true);
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
        int level = SceneManager.GetActiveScene().buildIndex;
        TMP_InputField pn = FindObjectOfType<TMP_InputField>();

        int scoreCount = PlayerPrefs.GetInt("scoreCount_" + level, 0);
        PlayerPrefs.SetInt("scoreCount_" + level, scoreCount + 1);

        PlayerPrefs.SetString("scoreName_" + level + "_" + scoreCount, pn.text);
        PlayerPrefs.SetInt("scoreValue_" + level + "_" + scoreCount, LBScript.instance.AssignScore(LBScript.instance.timeElapsed));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && 
            !CanvasManager.cmInstance.FindCanvas("CanvasMenu").activeSelf && 
            GameObject.Find("CanvasLB") == null)
        {
            PauseSettings();
            CanvasManager.cmInstance.FindCanvas("CanvasGame").SetActive(true);
        }
        print(Time.timeScale);
    }
    void PlaySettings()
    {
        Invoke("OL", .1f);
        isPaused = false;
        Time.timeScale = 1;
        GameObject.FindObjectOfType<TSIController>().GetComponent<TSIController>().enabled = true;
        GameObject.FindObjectOfType<TSIReflect>().GetComponent<TSIReflect>().enabled = true;
    }
    public void PauseSettings()
    {
        Invoke("OL", .1f);
        TextManager.instance.OnLoad();
        Time.timeScale = 0;       
        isPaused = true;
        if (GameObject.FindObjectOfType<TSIController>() != null)
            GameObject.FindObjectOfType<TSIController>().GetComponent<TSIController>().enabled = false;
        if (GameObject.FindObjectOfType<TSIReflect>() != null)
            GameObject.FindObjectOfType<TSIReflect>().GetComponent<TSIReflect>().enabled = false;
    }
    void OL()
    {
        TextManager.instance.OnLoad();
    }
    public IEnumerator Death(GameObject go)
    {
        TSIController tsiC;
        TSIReflect tsiR;
        if (go.GetComponent<TSIController>() != null)
        {
            tsiC = go.GetComponent<TSIController>();
            tsiC.dead = true;
            tsiC.movement = Vector3.zero;
            tsiC.bloodSplash.SetActive(true);
            tsiC.bloodSplash.transform.parent = null;
            tsiC.sprite.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (go.GetComponent<TSIReflect>() != null)
        {
            tsiR = go.GetComponent<TSIReflect>();
            tsiR.dead = true;
            tsiR.movement = Vector3.zero;
            tsiR.bloodSplash.SetActive(true);
            tsiR.bloodSplash.transform.parent = null;
            tsiR.sprite.GetComponent<SpriteRenderer>().enabled = false;
        }

        Animator anim = go.GetComponentInChildren<Animator>();
        anim.Play("New State");
        anim.speed = 1 / Time.timeScale;

        yield return new WaitForSecondsRealtime(.75f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }   
}
