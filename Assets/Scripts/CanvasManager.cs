using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    GameObject[] canvases;
    public static CanvasManager cmInstance;
    void Start()
    {
        cmInstance = this;
        
        GetAllCanvases(canvases);

        foreach (GameObject canvas in canvases)
            canvas.SetActive(false);

        //canvases[1].SetActive(true);
    }
    void GetAllCanvases(GameObject[] _canvases)
    {
        canvases = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            canvases[i] = transform.GetChild(i).gameObject;
        }
    }
    GameObject FindCanvas(string _cName)
    {       
        foreach (GameObject canvas in canvases)
        {
            if (canvas.name == _cName)
            {
                return canvas;
            }                
        }
        GameObject go = new GameObject();
        return go;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            FindCanvas("CanvasGame").SetActive(true);
        }
    }
}
