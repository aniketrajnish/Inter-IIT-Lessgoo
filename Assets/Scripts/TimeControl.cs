using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    bool yeet;    

    [Header("Assign These:")]
    public float minTimeScale;
    public float yeetTime;
    public static TimeControl tcInstance;
    public float SlowFOV, regularFOV;
    Camera[] cams;
    void Start()
    {
        tcInstance = this;
        cams = Camera.allCameras;
    }

    // Update is called once per frame
    void Update()
    {
        TimeScaleController(TSIController.tsiInstance.isSlowMo);
        print(Time.timeScale);
    }
    void TimeScaleController (bool _isSlowMo)
    {
        float time = (_isSlowMo) ? minTimeScale : 1f;
        float lerpTime = (_isSlowMo) ? 2f : 5f;
        float FOV = (_isSlowMo) ? SlowFOV : regularFOV;

        time = yeet ? 1 : time;
        lerpTime = yeet ? .1f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime * Time.unscaledDeltaTime);
        Time.fixedDeltaTime = Time.timeScale / 100;
        
        foreach (Camera cam in cams)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, FOV, lerpTime * Time.unscaledDeltaTime);

    }
    public IEnumerator Yeet(float time)
    {
        yeet = true;
        yield return new WaitForSecondsRealtime(yeetTime);
        yeet = false;
    }
}
