using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;
    public float speed;
    public GameObject sprite;
    public float rotSpeed = 3;
    //private float angleTo = -90;
    //private float angle = -90;
    Vector2 prevpos;
    Rigidbody2D rb;
    public GameObject bloodSplash;
    public float splashScale = 1.0f;
    public bool once;
    public bool onceOnly;
    bool flag;
    public bool hasWon;
    public bool playerInside;
    int sceneNo;
    bool dead;
    public bool gameEnd;
    

    /*public GameObject playerEnd;
    public GameObject refEnd;*/

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(SceneManager.GetActiveScene().name.Length==7)
            sceneNo = SceneManager.GetActiveScene().name[6]-48;
        if (SceneManager.GetActiveScene().name.Length == 8)
            sceneNo = 10+SceneManager.GetActiveScene().name[7] - 48;
        PlayerPrefs.SetInt("Level", sceneNo);
    }

    
    void Update()
    {
        if (!dead)
        {
            movement = Vector2.zero;
            if (Input.GetAxis("Horizontal") != 0)
            {
                movement.x = Input.GetAxis("Horizontal");
                //movement.y = 0;
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                movement.y = Input.GetAxis("Vertical");
                //movement.x = 0;
            }

            AnimatorEnable();

            //if (movement.x<0)
            //    angleTo = 90;
            //if (movement.y<0)
            //    angleTo = 180;
            //if (movement.x>0)
            //    angleTo = -90;
            //if (movement.y>0)
            //    angleTo = 0;

            //angle = Mathf.Lerp(angle, angleTo, Time.deltaTime * rotSpeed);
            Vector2 movedir = rb.position - prevpos;
            if (movedir.magnitude * 100 > 1)
                sprite.transform.rotation = Quaternion.Lerp(sprite.transform.rotation, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movedir.y * 100, movedir.x * 100) - 90), rotSpeed * Time.deltaTime);

            //Debug.DrawRay(rb.position, (rb.position - prevpos)*100);
            if (GameObject.Find("Reflection").GetComponent<ReflectPosition>().refInside == true && GameObject.Find("Player").GetComponent<PlayerController>().playerInside == true)
                StartCoroutine(ChangeScene());

            if (GameObject.Find("Reflection").GetComponent<ReflectPosition>().gameEnd == true && GameObject.Find("Player").GetComponent<PlayerController>().gameEnd == true && onceOnly == false)
            {
                StartCoroutine(EndGame());
                onceOnly = true;
            }

            /*Debug.Log(GameObject.Find("Reflection").GetComponent<ReflectPosition>().refInside + "ref");
            Debug.Log(GameObject.Find("Player").GetComponent<PlayerController>().playerInside + "pl");*/
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized  * speed * Time.fixedDeltaTime);
        prevpos = rb.position;
    }

    public void AnimatorEnable()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GetComponentInChildren<Animator>().Play("PlayerWalk");
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
        if (collision.gameObject.name=="Button")
        {   
            if(once==false && GameObject.Find("Reflection").GetComponent<ReflectPosition>().once == false)
                collision.gameObject.GetComponent<Animator>().Play("New State");
            hasWon = false;
        }
    }

    IEnumerator EndGame()
    {
        GameObject.Find("LightOff").GetComponent<AudioSource>().Play();
        GameObject.Find("SadMusic").GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(.7f);
        GameObject.Find("PlayerLight").SetActive(false);
        GameObject.Find("RefLight").SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator ChangeScene()
    {
        GameObject.Find("CanvasGame").GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator Death()
    {
        GameObject.Find("CanvasGame").GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

                if (GameObject.Find("Reflection").GetComponent<ReflectPosition>().hasWon == true)
                {
                    /* refEnd.SetActive(true);
                     playerEnd.SetActive(true);*/
                    if (GameObject.Find("PGHolder") != null && GameObject.Find("RGHolder") != null)
                    {
                        GameObject.Find("RGHolder").SetActive(false);
                        GameObject.Find("PGHolder").SetActive(false);
                    }
                    once = true;
                }
            }           

        }

        if (collision.gameObject.name == "PlayerInside")
        {
            if (SceneManager.GetActiveScene().name != "Level 10")
                playerInside = true;
            else if (SceneManager.GetActiveScene().name == "Level 10")
                gameEnd = true;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            bloodSplash.SetActive(true);
            dead = true;
            GetComponentInChildren<Animator>().Play("New State");
            StartCoroutine(Death());
            movement = Vector3.zero;
        }
    }
}
