using System;
using System.Collections;
using System.Collections.Generic;
using DialogueGraphEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public BossBloodBar bossBloodBarCanvas;
    public GameObject bossDeath;
    public GameObject ClearUI;
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
            DialogueGraphManeger.instance.OnEnd += showTongGuan;
            gameObject.SetActive(false);
            bossDeath.SetActive(true);
            bossDeath.transform.SetParent(null);
        }
    }

    public void showTongGuan()
    {
        ClearUI.SetActive(true);
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
    
}