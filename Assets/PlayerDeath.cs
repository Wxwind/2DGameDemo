using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public void DeleteSelf()
    {
        Debug.Log("Player is dead");
        Destroy(gameObject);       
    }
}

