using System;
using UnityEngine;

public abstract class Item:MonoBehaviour
{
    public PlayerInteract player;
    //public GameObject hint;
    public abstract void OnInteract();
    public Action OnBeforeInteract;
    public Action OnAfterInteract;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.items.Add(this);
        }
    }
    
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.items.Remove(this);
        }
    }

    public virtual void SetCallback(Action before, Action after)
    {
        OnBeforeInteract = before;
        OnAfterInteract = after;
    }
        
}