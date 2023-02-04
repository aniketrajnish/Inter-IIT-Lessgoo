using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class AudioManager : MonoBehaviour
{
    AudioSource aud;
    public AudioClip[] clips;
    public static AudioManager amInstance; 
    void Start()
    {
        aud = GetComponent<AudioSource>();
        amInstance = this;
    }
    public void PlayAud(string _clipName, bool _loop)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == _clipName)
                aud.clip = clip;
        }
        aud.loop = _loop;
        aud.Play();
    }
    void PlayBtnAud()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

                    if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
                        PlayAud("Click", false);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        PlayBtnAud();
    }
}
