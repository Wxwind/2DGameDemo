using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator anim;
    public GameObject restartMenu;
    private float waitForMenuTime;
    private Timer menuTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        waitForMenuTime = 3.0f;
        anim = GetComponent<Animator>();
        menuTimer = new Timer(waitForMenuTime, () => restartMenu.SetActive(true));
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
        anim.SetBool("Death", true);
        AudioManager.instance.PlaySFXAudio("Death");
        GetComponent<PlayerController>().enabled = false;
    }
}
