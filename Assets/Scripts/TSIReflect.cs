using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSIReflect : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;
    Vector2 prevpos;
    Animator anim;
    ParticleSystem[] parts;
    public bool hasWon, buttonTrigger, gateTrigger;

    [Header("Assign These:")]
    public GameObject sprite;
    public float rotSpeed, speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        parts = GetComponentsInChildren<ParticleSystem>();
        parts[0].Pause();
    }
    void Update()
    {
        movement = Vector2.zero;

        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(-movX, movY);

        rb.MovePosition(rb.position + movement.normalized * speed * Time.deltaTime / Time.timeScale);

        Vector2 movedir = rb.position - prevpos;

        if (movedir.magnitude > 0)
            sprite.transform.rotation = Quaternion.Lerp(sprite.transform.rotation,
                Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movedir.y * 100, movedir.x * 100) - 90),
                rotSpeed * Time.deltaTime / Time.timeScale);

        if (TSIController.tsiInstance.isSlowMo)
        {
            anim.Play("GirlWalk");
            parts[0].Play();
        }
        else
        {
            anim.Play("New State");
            parts[0].Pause();
        }

        anim.speed = 1 / Time.timeScale;

        prevpos = rb.position;
    }  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UI")
        {
            AudioSource source;
            if(collision.gameObject.GetComponent<AudioSource>() != null)
            {
                source = collision.gameObject.GetComponent<AudioSource>();
            }
            else
            {
                source = collision.GetComponentInParent<AudioSource>();
            }
            if (!source.isPlaying)
                StartCoroutine(DisableAudioSource(source));
        }

        if (collision.gameObject.name == "Button")
        {
            collision.gameObject.GetComponent<Animator>().Play("ButtonPress");
            GameObject.Find("Button").GetComponent<AudioSource>().Play();
            buttonTrigger = true;

                
        }
        if (collision.gameObject.tag == "LevelSwitch")
        {
            //play gate animation
            gateTrigger = true;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameObject.Find("Player").GetComponent<TSIController>().buttonTrigger)
        {
            hasWon = true;
        }

        if (GameObject.Find("Player").GetComponent<TSIController>().gateTrigger && hasWon)
        {
            collision.GetComponentInChildren<Animator>().Play("Gate_animation");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Button")
        {
            collision.gameObject.GetComponent<Animator>().Play("New State");
            buttonTrigger = false;
        }
        if (collision.gameObject.tag == "LevelSwitch")
        {
            //play gate close animation
            gateTrigger = false;
        }
    }

    IEnumerator DisableAudioSource(AudioSource source)
    {
        source.Play();
        yield return new WaitForSecondsRealtime(source.clip.length);
        source.enabled = false;
    }
}
