using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    private TextMeshProUGUI[] texts;
    private List<bool> checker;
    [SerializeField] private TextMeshProUGUI[] excludedTexts;
    private List<GameObject> includedTextObjects;
    public static TextManager instance;
    [SerializeField] private float extremeVal = 0.25f;
    [SerializeField] private float angFreq = 3.14f;

    private void Awake()
    {
        instance = this;
        Invoke("OnLoad", .1f);
    }
    public void OnLoad()
    {
        texts = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        checker = new List<bool>();
        for (int i = 0; i < texts.Length; ++i)
        {
            checker.Add(true);
        }
        RemoveExcludedTexts();

        includedTextObjects = new List<GameObject>();
        CreateIncludedList();
    }
 

    private void Update()
    {
        Animate();
    }


    bool compareTMPObjects(TextMeshProUGUI t1, TextMeshProUGUI t2)
    {
        if(string.Equals(t1.name, t2.name))
        {
            return true;
        }

        return false;
    }    


    void RemoveExcludedTexts()
    {
        foreach (TextMeshProUGUI t1 in excludedTexts)
        {
            int i = 0;
            foreach (TextMeshProUGUI t2 in texts)
            {
                if (compareTMPObjects(t1, t2))
                {
                    checker[i] = false;
                }
                i++;
            }
        }
        
    }

    void CreateIncludedList()
    {
        int i = 0;
        foreach (TextMeshProUGUI t in texts)
        {
            if(checker[i])
            {
                includedTextObjects.Add(t.gameObject);
            }
            i++;
        }
    }

    void Animate()
    {
        float scale = 1 + extremeVal * Mathf.Sin(angFreq * Time.time);
        foreach (GameObject includedObject in includedTextObjects)
        {
            includedObject.transform.localScale = scale * transform.localScale;
        }
    }
}
