using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    private PlayerController pc;
    private Rigidbody2D rb;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.interactKey))
        {
            if (items != null)
            {
                items[0].SetCallback(() =>
                    {
                        pc.enabled = false;
                        rb.velocity = Vector2.zero;
                        pc.currentSpeed=Vector2.zero;
                        pc.isIdling = true;
                    },
                    () => { pc.enabled = true; });
                items[0].OnInteract();
            }
        }
    }
}