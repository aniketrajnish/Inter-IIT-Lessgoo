using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    bool yeet;
    private Vector3 originalVignetteScale;
    private Vector3 sloMoVignetteScale;

    [Header("Assign These:")]
    public float minTimeScale;
    public float yeetTime;
    public static TimeControl tcInstance;
    public float SlowFOV, regularFOV;
    Camera[] cams;
    public Transform[] Vignettes;
    public float scaleFactor = 10.0f;    
    void Start()
    {
        tcInstance = this;
        cams = Camera.allCameras;
        originalVignetteScale = Vignettes[0].localScale;
        sloMoVignetteScale = originalVignetteScale;
        originalVignetteScale = scaleFactor * sloMoVignetteScale;
    }

    // Update is called once per frame
    void Update()
    {
        TimeScaleController(TSIController.tsiInstance.isSlowMo);
    }
    void TimeScaleController (bool _isSlowMo)
    {
        float time = (_isSlowMo) ? minTimeScale : 1f;
        float lerpTime = (_isSlowMo) ? 2f : 5f;
        float FOV = (_isSlowMo) ? SlowFOV : regularFOV;
        Vector3 scale = (_isSlowMo) ? sloMoVignetteScale : originalVignetteScale;

        time = yeet ? 1 : time;
        lerpTime = yeet ? .1f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime * Time.unscaledDeltaTime);
        Time.fixedDeltaTime = Time.timeScale / 100;
        
        foreach (Camera cam in cams)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, FOV, lerpTime * Time.unscaledDeltaTime);

        foreach (Transform Vignette in Vignettes)
            Vignette.localScale = Vector3.Lerp(Vignette.localScale, scale, lerpTime * Time.unscaledDeltaTime * 3f);
        print(Time.timeScale);
    }
    public IEnumerator Yeet(float time)
    {
        yeet = true;
        yield return new WaitForSecondsRealtime(yeetTime);
        yeet = false;
    }
}
