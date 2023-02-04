using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSIController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;
    Vector2 prevpos;
    [HideInInspector] public bool isSlowMo;
    Animator anim;
    ParticleSystem[] parts;   
    bool isAudPlaying;

    [Header("Assign These:")]
    public GameObject sprite;
    public float rotSpeed, speed;
    public static TSIController tsiInstance;

    void Start()
    {
        tsiInstance = this;

        rb = GetComponent<Rigidbody2D>();
        parts = GetComponentsInChildren<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        movement = Vector2.zero;

        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(movX, movY);

        rb.MovePosition(rb.position + movement.normalized * speed * Time.deltaTime / Time.timeScale);        

        Vector2 movedir = rb.position - prevpos;

        if (movedir.magnitude > 0)
            sprite.transform.rotation = Quaternion.Lerp(sprite.transform.rotation,
                Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movedir.y * 100, movedir.x * 100) - 90),
                rotSpeed * Time.deltaTime / Time.timeScale);

        isSlowMo = (movX != 0 || movY != 0) ? true : false;

        AudioSource aud = GetComponent<AudioSource>();

        if (isSlowMo)
        {
            anim.Play("PlayerWalk");
            parts[0].Play();
            aud.enabled = true;
            if (!isAudPlaying)
                aud.time = Random.Range(0, aud.clip.length-1);
            isAudPlaying = true;
        }
        else
        {
            anim.Play("New State");
            parts[0].Pause();
            aud.enabled = false;
            isAudPlaying = false;
        }

        anim.speed = 1 / Time.timeScale;

        prevpos = rb.position;
    }
}
