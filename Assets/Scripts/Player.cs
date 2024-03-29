﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpHeight = 10;
    public bool canJump = true;
    Rigidbody2D rb;
    LineRenderer lr;
    //public GameObject gameManager;

    public AudioSource coinAudio;
    public AudioSource jumpAudio;

    string deviceType;

    public bool canMove = true;

    [SerializeField]
    public ParticleSystem deathParticals;



    [SerializeField]
    GameObject levelmanager;
    LevelManagerStrategy levelManager;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        lr = transform.GetComponent<LineRenderer>();
        coinAudio.volume = jumpAudio.volume = Memory.EffectVolume;
        
        if(levelmanager.GetComponent<AllCoinCollectLevelManager>() != null) 
        {
            levelManager = levelmanager.GetComponent<AllCoinCollectLevelManager>(); 
        }
        else if(levelmanager.GetComponent<OneCoinAtATimeLevelManager>() != null) 
        {
            levelManager = levelmanager.GetComponent<OneCoinAtATimeLevelManager>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpHeight * Time.deltaTime, ForceMode2D.Impulse);
        }*/
        if (!canMove)
        {
            return;
        }

        if (canJump)
        {
            Vector2 mousePos = Vector2.zero;
            if (deviceType == "Handheld")
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(touch.position);
                    }
                }
            } 
            else
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lr.enabled = true;
            }

            Vector2 dir = mousePos - new Vector2(transform.position.x, transform.position.y);

            
            if (Input.GetButtonDown("Fire1"))
            {
                rb.drag = 0;
                rb.velocity = Vector2.zero;
                rb.AddForce(dir.normalized * jumpHeight, ForceMode2D.Impulse);
                jumpAudio.Play();
                canJump = false;
            }
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, (Vector3)(dir.normalized*5));
        }
        else
        {
            lr.enabled = false;
        }
    }

    void Death()
    {
        StopMoving();
        this.GetComponent<SpriteRenderer>().enabled = false;
        deathParticals.Play();
        Invoke("DeathHelperMethod", 0.5f);
    }

    void DeathHelperMethod()
    {
        levelManager.FailLevel();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            //rb.velocity = Vector2.zero;
            //rb.drag = 10;
            canJump = true;
        }
        else if(collision.transform.tag == "Floor")
        {
            //rb.drag = 1;
            //lr.enabled = true;
            canJump = true;
        }
        else if(collision.transform.tag == "Death")
        {
            Death();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Floor")
        {
            rb.drag = 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor" || collision.transform.tag == "Wall")
        {
            canJump = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag == "Coin")
        {
            levelManager.CoinPickup();
            Destroy(collider.gameObject);
            coinAudio.Play();
            canJump = true;
        }
    }

    public void StopMoving()
    {
        jumpHeight = 0;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }
}
