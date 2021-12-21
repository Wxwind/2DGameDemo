using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Timer lifeTimer;
    private Action OnDestroy;
    private LayerMask ground;

    private void Awake()
    {
        ground=LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        lifeTimer.Tick(Time.deltaTime);
    }

    public void Init(float lifetime,Action OnDestroy)
    {
        lifeTimer = new Timer(lifetime, () => {
            OnDestroy();
            Destroy(gameObject);
        });
        this.OnDestroy = OnDestroy;
        lifeTimer.Run();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer==ground)
        {
            OnDestroy();
            Destroy(gameObject);
        }
    }
}
