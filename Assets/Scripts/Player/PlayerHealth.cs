using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject restartMenu;
    public GameObject deathPrefab;
    private float waitForMenuTime;
    private SpriteRenderer sr;
    private Timer menuTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        waitForMenuTime = 2.0f;
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
        GetComponent<PlayerController>().enabled = false;
        deathPrefab.SetActive(true);
        AudioManager.instance.PlaySFXAudio("Death");
        menuTimer.ReRun();
        sr.enabled = false;
    }
}
