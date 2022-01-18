using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathPre;
    private PlayerController pc;
    public GameObject restartMenu;
    public float waitForMenuTime;
    private Timer menuTimer;
    private Rigidbody2D rb;
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        menuTimer = new Timer(waitForMenuTime, () =>
        {
            restartMenu.SetActive(true);
            Time.timeScale = 0;
        });
    }

    private void Update()
    {
        menuTimer.Tick(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap_Enemy"))
        {
            Death();
        }
    }

    private void Death()
    {
        menuTimer.ReRun();
        rb.bodyType = RigidbodyType2D.Static;
        Instantiate(deathPre, transform.position, transform.rotation);
        deathPre.GetComponent<SpriteRenderer>().flipX = pc.faceDir == -1;
        GetComponent<SpriteRenderer>().enabled = false;
        pc.enabled = false;
        AudioManager.instance.PlaySFXAudio("Death");
    }
}