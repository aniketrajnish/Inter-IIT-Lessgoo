using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ReflectEnvironment : MonoBehaviour
{
    public GameObject Environment;
    GameObject reflectedEnvironment;
    // Start is called before the first frame update
    void Start()
    {
        reflectedEnvironment = Instantiate(Environment);
        reflectedEnvironment.transform.position = Vector3.Scale(reflectedEnvironment.transform.position, new Vector3(-1, 1, 1));
        reflectedEnvironment.transform.localScale = Vector3.Scale(reflectedEnvironment.transform.localScale, new Vector3(-1, 1, 1));
        foreach (SpriteRenderer renderer in reflectedEnvironment.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = false;
        }
        foreach (ShadowCaster2D shadow in reflectedEnvironment.GetComponentsInChildren<ShadowCaster2D>())
        {
            shadow.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
