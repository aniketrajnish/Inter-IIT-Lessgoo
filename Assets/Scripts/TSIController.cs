using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSIController : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector] public Vector2 movement;
    Vector2 prevpos;
    public Animator anim, bloodAnim;
    ParticleSystem[] parts;
    bool isAudPlaying;
    [HideInInspector] public bool once, hasWon, dead, isSlowMo, buttonTrigger, gateTrigger, endTrigger;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] TSIReflect reflect;

    [Header("Assign These:")]
    public GameObject sprite, bloodSplash;
    public float rotSpeed, speed;
    public static TSIController tsiInstance;
    //bool hasCollided;

    void Awake()
    {
        tsiInstance = this;
        rb = GetComponent<Rigidbody2D>();
        parts = GetComponentsInChildren<ParticleSystem>();
        parts[0].Stop();
    }
    void Update()
    {
        anim.speed = 1 / Time.timeScale;
        bloodAnim.speed = 1 / Time.timeScale;

        if (!dead)
            Move();
        else
            parts[0].Stop();
    }
    void Move()
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
            {
                aud.time = Random.Range(0, aud.clip.length - 1);
                //AudioManager.amInstance.PlayAud("Footsteps", true);
            }
            isAudPlaying = true;

        }
        else
        {
            anim.Play("New State");
            parts[0].Pause();
            aud.enabled = false;
            //AudioManager.amInstance.GetComponent<AudioSource>().Stop();
            isAudPlaying = false;
        }

        prevpos = rb.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UI")
        {
            AudioSource source;
            if (collision.gameObject.GetComponent<AudioSource>() != null)
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
            if (!hasWon)
            {
                AudioSource aud = GameObject.Find("Button").GetComponent<AudioSource>();
                if (aud.isActiveAndEnabled)
                    aud.Play();
            }
            buttonTrigger = true;


            if (reflect.buttonTrigger)
               hasWon = true;
            

        }
        if (collision.gameObject.tag == "LevelSwitch")
        {
            //play gate animation
            gateTrigger = true;
            if (hasWon)
            {
                Animator anim =  collision.GetComponentInChildren<Animator>();
                anim.Play("Gate_animation");
                anim.speed = 1 / Time.timeScale;
                AudioManager.instance.PlayAud("Door Open", false);
                if (!once)
                {
                    SpriteRenderer sr = GameObject.Find("Bhangarh").GetComponent<SpriteRenderer>();
                    StartCoroutine(FadeIn(sr, fadeTime));
                    once = true;
                    AudioManager.instance.PlayAud("bhangarhfort", false);
                }
                Collider2D col = collision.GetComponentsInChildren<BoxCollider2D>()[1];
                col.isTrigger = true;
            }
        }
        if(collision.gameObject.name == "EndCollider")
        {
            endTrigger = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Button")
        {
            if (reflect.buttonTrigger)
                hasWon = true;
        }

        
        if (collision.gameObject.name == "EndCollider")
        {
            if (reflect.endTrigger)            
                Debug.Log("switch");            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Button")
        {
            if(!hasWon)
            {
                collision.gameObject.GetComponent<Animator>().Play("New State");
            }
            buttonTrigger = false;
        }

        if (collision.gameObject.tag == "LevelSwitch")
        {
            //play gate close animation
            gateTrigger = false;
        }
        if (collision.gameObject.name == "EndCollider")
        {
            endTrigger = false;
        }
    }
    IEnumerator DisableAudioSource(AudioSource source)
    {
        if (source.isActiveAndEnabled)
            source.Play();

        yield return new WaitForSecondsRealtime(source.clip.length);
        source.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
            StartCoroutine(GameManager.instance.Death(this.gameObject));

    }
    
    IEnumerator FadeIn(SpriteRenderer sr, float t)
    {
        for (float i = 0; i < 1; i += .1f/t)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, i);
            yield return new WaitForSecondsRealtime(.1f);
        }
    }
}
