using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneArea : MonoBehaviour
{
    public int nextSceneIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.SaveDataByJson(nextSceneIndex);
            GameManager.instance.LoadNewScene(nextSceneIndex);           
        }
    }
}
