using System;
using System.Collections;
using System.Collections.Generic;
using DialogueGraphEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BossHealth : MonoBehaviour
{
    public int maxHealth;
    public BossBloodBar bossBloodBarCanvas;

    private void Start()
    {
        bossBloodBarCanvas.SetBloodBar(1);
        bossBloodBarCanvas.SetBloodBar(0.2f);
    }

    public void SetBloodUI(int health)
    {
        float percent = InverseLerp(health);
        bossBloodBarCanvas.SetBloodBar(percent);
    }

    private float InverseLerp(int value)
    {
        return (float) value / maxHealth;
    }
}