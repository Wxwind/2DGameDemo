using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Item_txt : Item
{
    [TextArea]
    public string introduction;
    public GameObject interactivePanel;
    private TMP_Text interactiveTxt;
    private const float speed = 8;
    private int state = 1;
    private bool isWaitng;
    private void Awake()
    {
        interactiveTxt = interactivePanel.GetComponentInChildren<TMP_Text>();
    }

    public override void OnInteract()
    {
        switch (state)
        {
            case 1:
                if (isWaitng)
                {
                    return;
                }
                OnBeforeInteract?.Invoke();
                isWaitng = true;
                interactivePanel.SetActive(true);
                Tween t1=interactivePanel.transform.DOScaleX(0, 1f).From();
                interactiveTxt.text = "";
                t1.OnComplete(() => interactiveTxt.DOText(introduction, introduction.Length / speed).OnComplete(() =>
                    {
                        state = 2;
                        isWaitng = false;
                    })
                );
                break;
            case 2:
                interactivePanel.SetActive(false);
                state = 1;
                OnAfterInteract?.Invoke();
                break;
        }
    }


}
