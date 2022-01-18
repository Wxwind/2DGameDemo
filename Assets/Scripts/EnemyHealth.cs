﻿using System;
using System.Collections;
using System.Collections.Generic;
using DialogueGraphEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public BossBloodBar bossBloodBarCanvas;
    private int hp;
    public DialogueGraph successDialog;

    private void Start()
    {
        bossBloodBarCanvas.SetBloodBar(1);
        hp = maxHealth;
    }

    public void Hurt(int attack)
    {
        hp -= attack;
        SetBloodUI();
        if (hp<=0)
        {
            DialogueGraphManeger.instance.ExecuteDialogGraph(successDialog);
        }
    }


    private void SetBloodUI()
    {
        float percent = InverseLerp(hp);
        bossBloodBarCanvas.SetBloodBar(percent);
    }

    private float InverseLerp(int value)
    {
        return (float) value / maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            var attack = other.GetComponent<Bubble>().attack;
            Hurt(attack);
        }
    }
}