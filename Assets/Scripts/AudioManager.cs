using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class AudioManager : MonoBehaviour
{
    AudioSource aud;
    public AudioClip[] clips;
    public static AudioManager instance = null;
    public static AudioManager Instance
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
        instance = this;
    }

    void Start()
    {
        aud = GetComponent<AudioSource>();
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
                GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

                if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
                    PlayAud("Click", false);
            }
        }
    }
    void Update()
    {
        PlayBtnAud();
    }
}
