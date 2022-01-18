using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorHint : MonoBehaviour
{
    public GameObject hintUI;
    public int nextScene;
    [TextArea] public string content;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hintUI.SetActive(true);
            hintUI.GetComponentInChildren<TMP_Text>().text = content;
        }

        if (Input.GetKeyDown(InputManager.instance.interactKey))
        {
            GameManager.instance.LoadNewScene(nextScene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetKeyDown(InputManager.instance.interactKey))
        {
            GameManager.instance.LoadNewScene(nextScene);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        hintUI.SetActive(false);
    }
}