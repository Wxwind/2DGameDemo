using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DialogueGraphEditor;
using DG.Tweening;
using Unity.Collections;

public class Test : MonoBehaviour
{
    //[PreviewField]
    //[LabelText("这是精灵")]
    //public Sprite m_sprite;
    //[LabelText("这是字典")]
    //public Dictionary<string, int> m_dic = new Dictionary<string, int>();
    //[ReadOnly]
    //public int m_int;
    //public string m_string = "abc";
    //[Button("按钮", 30)]
    public DialogueGraph graph;
    public GameObject p;
    private PlayerController pc;
    private Rigidbody2D rb;

    private void Awake()
    {
        pc = p.GetComponent<PlayerController>();
        rb = p.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        DialogueGraphManeger.instance.LoadDialogGraph(graph,() =>
            {
                pc.enabled = false;
                rb.velocity = Vector2.zero;
                pc.currentSpeed=Vector2.zero;
                pc.isIdling = true;
            },
            () => { pc.enabled = true; });
    }
}
