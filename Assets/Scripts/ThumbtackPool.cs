using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbtackPool : MonoBehaviour
{
    public static ThumbtackPool instance;
    public GameObject thumbtackPre;
    public int maxCount;
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject go = Instantiate(thumbtackPre, transform);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    public GameObject GetFromPool()
    {
        GameObject go = pool.Dequeue();
        go.SetActive(true);
        return go;
    }

    public void ReturnPool(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}