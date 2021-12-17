using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class OptionUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public int id;
    private Image clickImg;
    private TMP_Text optionText;

    private void Awake()
    {
        clickImg = transform.Find("OnSelectImg").GetComponent<Image>();
        optionText = transform.Find("Text").GetComponent<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        clickImg.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        clickImg.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueGraphManeger.instance.ClickOptionUnitCallBack(id);
    }
}
