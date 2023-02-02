using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReflectPosition : MonoBehaviour
{
    Vector2 movement;
    public float speed;
    public GameObject sprite;
    public GameObject bloodSplash;
    public float rotSpeed = 3;
/*    private float angleTo = 90;
    private float angle = 90;*/
    Rigidbody2D rb;
    Vector2 prevpos;

    public bool once;
    bool flag;
    public bool hasWon;
    public bool refInside;
    public bool gameEnd;
    bool dead;
    /*public GameObject playerEnd;
    public GameObject refEnd;*/
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            movement = Vector2.zero;
            if (Input.GetAxis("Horizontal") != 0)
            {
                movement.x = -Input.GetAxis("Horizontal");
                //movement.y = 0;
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                movement.y = Input.GetAxis("Vertical");
                //movement.x = 0;
            }

            AnimatorEnable();

            //if (movement.x < 0)
            //    angleTo = 90;
            //if (movement.y < 0)
            //    angleTo = 180;
            //if (movement.x > 0)
            //    angleTo = -90;
            //if (movement.y > 0)
            //    angleTo = 0;

            //angle = Mathf.Lerp(angle, angleTo, Time.deltaTime * rotSpeed);
            //sprite.transform.eulerAngles = new Vector3(0, 0, angle);

            Vector2 movedir = rb.position - prevpos;
            if (movedir.magnitude * 100 > 1)
                sprite.transform.rotation = Quaternion.Lerp(sprite.transform.rotation, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movedir.y * 100, movedir.x * 100) - 90), rotSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + movement.normalized * speed * Time.fixedDeltaTime);
        prevpos = rb.position;
    }
    IEnumerator Death()
    {
        GameObject.Find("CanvasGame").GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void AnimatorEnable()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GetComponentInChildren<Animator>().Play("GirlWalk");
            GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            GetComponentInChildren<Animator>().Play("New State");
            GetComponent<AudioSource>().enabled = false;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Button")
        {
            if (once == false && GameObject.Find("Player").GetComponent<PlayerController>().once == false)
                collision.gameObject.GetComponent<Animator>().Play("New State");
            hasWon = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Button")
        {
            collision.gameObject.GetComponent<Animator>().Play("ButtonPress");
            GameObject.Find("Button").GetComponent<AudioSource>().Play();
            if (once == false)
            {                
                hasWon = true;
               
                if (GameObject.Find("Player").GetComponent<PlayerController>().hasWon==true)
                {
                    if (GameObject.Find("PGHolder") != null && GameObject.Find("RGHolder") != null)
                    {
                        GameObject.Find("RGHolder").SetActive(false);
                        GameObject.Find("PGHolder").SetActive(false);
                    }
                    /*refEnd.SetActive(true);
                    playerEnd.SetActive(true);*/
                    once = true;
                }

                
            }

            
        }

        if (collision.gameObject.name == "RefInside")
        {
            if (SceneManager.GetActiveScene().name != "Level 10")
                refInside = true;
             else if (SceneManager.GetActiveScene().name == "Level 10")
                gameEnd = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Death"))
        {
            bloodSplash.SetActive(true);
            dead = true;
            GetComponentInChildren<Animator>().Play("New State");
            StartCoroutine(Death());
            movement = Vector3.zero;
        }
    }
}
