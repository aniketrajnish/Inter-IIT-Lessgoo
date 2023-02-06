using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    bool yeet;
    private Vector3 originalVignetteScale;
    private Vector3 sloMoVignetteScale;
    AudioSource aud;
    float maxVol;

    [Header("Assign These:")]
    public float minTimeScale;
    public float yeetTime;
    public static TimeControl tcInstance;
    public float SlowFOV, regularFOV;
    Camera[] cams;
    public Transform[] Vignettes;
    public float scaleFactor;
    public AudioClip[] clips;
    void Start()
    {
        PlayAud();
        tcInstance = this;
        cams = Camera.allCameras;

        originalVignetteScale = Vignettes[0].localScale;
        sloMoVignetteScale = originalVignetteScale;
        originalVignetteScale = scaleFactor * sloMoVignetteScale;
        aud.volume = (float)System.Math.Pow(Time.timeScale, 1) * maxVol;
    }

    void PlayAud()
    {
        aud = GetComponent<AudioSource>();
        int clipIndex = Random.Range(0, clips.Length);
        aud.clip = clips[clipIndex];

        if (clipIndex == 0)
            maxVol = .01f;
        else
            maxVol = .05f;       

        aud.Play();        
    }

    // Update is called once per frame
    void Update()
    {
        if(TSIController.tsiInstance != null)
            TimeScaleController(TSIController.tsiInstance.isSlowMo);
    }
    void TimeScaleController (bool _isSlowMo)
    {
        float time = (_isSlowMo) ? minTimeScale : 1f;
        float lerpTime = (_isSlowMo) ? 20f : 50f;
        float FOV = (_isSlowMo) ? SlowFOV : regularFOV;
        Vector3 scale = (_isSlowMo) ? sloMoVignetteScale : originalVignetteScale;

        time = yeet ? 1 : time;
        lerpTime = yeet ? .1f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime * Time.unscaledDeltaTime);
        Time.fixedDeltaTime = Time.timeScale / 100;
        
        foreach (Camera cam in cams)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, FOV, lerpTime * Time.unscaledDeltaTime * .2f);

        foreach (Transform Vignette in Vignettes)
            Vignette.localScale = Vector3.Lerp(Vignette.localScale, scale, lerpTime * Time.unscaledDeltaTime * .6f);

        aud.volume = (float)System.Math.Pow(Time.timeScale, 1) * maxVol;
        //print(Time.timeScale);
    }
    public IEnumerator Yeet(float time)
    {
        yeet = true;
        yield return new WaitForSecondsRealtime(yeetTime);
        yeet = false;
    }
}
