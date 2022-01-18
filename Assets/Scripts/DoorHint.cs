using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorHint : MonoBehaviour
{
    public GameObject hintUI;
    public int nextScene;
    private bool canIn;
    [TextArea] public string content;

    private void Update()
    {
        if (canIn&&Input.GetKeyDown(InputManager.instance.interactKey))
        {
            GameManager.instance.LoadNewScene(nextScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hintUI.SetActive(true);
            hintUI.GetComponentInChildren<TMP_Text>().text = content;
            canIn = true;
        }
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        hintUI.SetActive(false);
        canIn = false;
    }
}