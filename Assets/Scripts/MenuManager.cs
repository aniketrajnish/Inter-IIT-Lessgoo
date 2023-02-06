using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    Camera[] cams;
    float initX1, initY1, initX2, initY2;
    [SerializeField] float freq, amp;
    void Start()
    {
        cams = GameObject.FindObjectsOfType<Camera>();

        initX1 = cams[0].transform.position.x;
        initY1 = cams[0].transform.position.y;

        initX2 = cams[1].transform.position.x;
        initY2 = cams[1].transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float smoothX = amp * Mathf.Sin(Time.unscaledTime * freq);
        float smoothY = -.5f* amp * Mathf.Cos(Time.unscaledTime * freq);

        cams[0].transform.position = new Vector3(smoothX + initX1, smoothY + initY1, -10);
        cams[1].transform.position = new Vector3(-smoothX + initX2, -smoothY + initY2, -10);
    }
    private void OnDisable()
    {
        if (cams[0] != null && cams[1] != null)
        {
            cams[0].transform.position = new Vector3(initX1, initY1, -10);
            cams[1].transform.position = new Vector3(initX2, initY2, -10);
        }
    }
}
