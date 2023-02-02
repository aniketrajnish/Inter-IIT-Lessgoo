using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    public GameObject drag;// Start is called before the first frame update
    public GameObject canvas;// Start is called before the first frame update
    public GameObject creds;// Start is called before the first frame update
    void Start()
    {
        StartCoroutine(dosec());
        drag.SetActive(false);
    }

    IEnumerator dosec()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Find("GirlSprite").SetActive(false);
        GameObject.Find("DevilWalk").SetActive(false);
        yield return new WaitForSeconds(.5f);
        drag.SetActive(true);
        yield return new WaitForSeconds(3f);
        canvas.SetActive(true);
        
        yield return new WaitForSeconds(.3f);
        GameObject.Find("SadMusic").GetComponent<AudioSource>().Play();
        GameObject.Find("SadMusic").GetComponent<AudioSource>().time=47f;
        yield return new WaitForSeconds(1.2f);
        creds.SetActive(true);
    }
    void Update()
    {
       
    }
}
