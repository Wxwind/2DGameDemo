using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathPre;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap_Enemy"))
        {
            Death();          
        }
    }

    private void Death()
    {
        var deathPrefab=Instantiate(deathPre, transform.position, transform.rotation);
        deathPrefab.GetComponent<SpriteRenderer>().flipX = GetComponent<PlayerController>().faceDir == -1;
        //AudioManager.instance.PlaySFXAudio("Death");
        Destroy(gameObject);
    }
}
