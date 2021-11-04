using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator anim;
    public GameObject restartMenu;
    public GameObject deathPrefab;
    private float waitForMenuTime;
    private SpriteRenderer sr;
    private Timer menuTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        waitForMenuTime = 2.0f;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        menuTimer = new Timer(waitForMenuTime, () => restartMenu.SetActive(true));    
    }

    private void Update()
    {
        menuTimer.Tick(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Death();          
        }
    }

    private void Death()
    {
        deathPrefab.SetActive(true);
        AudioManager.instance.PlaySFXAudio("Death");
        menuTimer.ResetAndRun();
        sr.enabled = false;
    }
}
