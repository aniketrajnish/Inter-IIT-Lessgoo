using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSIController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;
    Vector2 prevpos;
    bool yeet;
    [Header("Assign These:")]
    public GameObject sprite;
    public float rotSpeed, speed;
    public float minTimeScale;
    public float yeetTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public IEnumerator Yeet(float time)
    {
        yeet = true;
        yield return new WaitForSecondsRealtime(yeetTime);
        yeet = false;
    }
    void Update()
    {
        movement = Vector2.zero;

        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");
        float rawX = Input.GetAxisRaw("Horizontal");
        float rawY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(movX, movY);

        Vector2 movedir = rb.position - prevpos;

        if (movedir.magnitude > 0)
            sprite.transform.rotation = Quaternion.Lerp(sprite.transform.rotation,
                Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movedir.y * 100, movedir.x * 100) - 90),
                rotSpeed);

        float time = (movX != 0 || movY != 0) ? minTimeScale : 1f;
        float lerpTime = (movX != 0 || movY != 0) ? .05f : .2f;

        time = yeet ? 1 : time;
        lerpTime = yeet ? .1f : lerpTime;

        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime);

        if (rawX == 0 && rawY == 0)
            rb.velocity = Vector2.zero;

        else
        {
            rb.MovePosition(rb.position + movement.normalized * speed * Time.unscaledDeltaTime);
            prevpos = rb.position;
        }

        Time.fixedDeltaTime = Time.timeScale / 100;        
    }


}
