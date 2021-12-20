using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBloodBar : MonoBehaviour
{
    [Tooltip("血条消失速度")]
    public float fadeSpeed;
    private float healthPercent;
    private Image slowImg;
    private Image imImg;
    private Material imimgMat;

    private static readonly int HealthID = Shader.PropertyToID("_health");
    
    void Awake()
    {
        slowImg = transform.Find("slowImg").GetComponent<Image>();
        imImg=transform.Find("imImg").GetComponent<Image>();
        imimgMat = imImg.material;
    }

    private void Update()
    {
        if (slowImg.fillAmount >= healthPercent)
        {
            slowImg.fillAmount -= fadeSpeed / 1000;
        }
    }


    public void SetBloodBar(float percent)
    {
        imimgMat.SetFloat(HealthID, percent);
        healthPercent = percent;
    }
}