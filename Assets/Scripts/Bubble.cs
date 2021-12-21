using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject bubbleBreakPre;
    private Timer lifeTimer;
    private Action OnDestroySelf;
    private LayerMask ground;

    private void Awake()
    {
        ground=LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        lifeTimer.Tick(Time.deltaTime);
    }

    public void Init(float lifetime,Action action)
    {
        OnDestroySelf = action;
        lifeTimer = new Timer(lifetime, Death);
        lifeTimer.Run();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer==ground)
        {
            Death();
        }
    }

    private void Death()
    {
        OnDestroySelf();
        var trans = transform;
        Instantiate(bubbleBreakPre, trans.position, trans.rotation);
        Destroy(gameObject);
    }
}
