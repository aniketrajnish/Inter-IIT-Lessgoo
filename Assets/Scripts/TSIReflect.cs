using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSIReflect : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector] public Vector2 movement;
    Vector2 prevpos;
    public Animator anim, bloodAnim;
    ParticleSystem[] parts;
    [HideInInspector] public bool once, hasWon, dead, gateTrigger, buttonTrigger;
    [SerializeField] private float fadeTime = 1f;

    [Header("Assign These:")]
    public GameObject sprite, bloodSplash;
    public float rotSpeed, speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator[] anims = GetComponentsInChildren<Animator>();
        parts = GetComponentsInChildren<ParticleSystem>();
        parts[0].Pause();
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
            GameObject.Find("Button").GetComponent<AudioSource>().Play();
            buttonTrigger = true;
            if (GameObject.Find("Player").GetComponent<TSIController>().buttonTrigger)
            {
                hasWon = true;
            }

        }
        if (collision.gameObject.tag == "LevelSwitch")
        {
            //play gate animation
            gateTrigger = true;
            if (hasWon)
            {
                collision.GetComponentInChildren<Animator>().Play("Gate_animation");
                AudioManager.Instance.PlayAud("Door Open", false);
                if (!once)
                {
                    SpriteRenderer sr = GameObject.Find("Trespass").GetComponent<SpriteRenderer>();
                    StartCoroutine(FadeIn(sr, fadeTime));
                    once = true;
                    AudioManager.instance.PlayAud("trespassatownrisk", false);
                }
                Collider2D col = collision.GetComponentsInChildren<BoxCollider2D>()[1];
                Debug.Log(col.gameObject.name);
                col.isTrigger = true;
            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Button")
        {
            if (GameObject.Find("Player").GetComponent<TSIController>().buttonTrigger)
            {
                hasWon = true;
            }
        }
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Button")
        {
            if (!hasWon)
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

    }
    IEnumerator DisableAudioSource(AudioSource source)
    {
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
        for (float i = 0; i < 1; i += .1f / t)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, i);
            yield return new WaitForSecondsRealtime(.1f);
        }
    }
}
