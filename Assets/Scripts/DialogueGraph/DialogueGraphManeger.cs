using System;
using System.Collections;
using System.Collections.Generic;
using DialogueGraphEditor;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class DialogueGraphManeger: MonoBehaviour
{
    public static DialogueGraphManeger instance;
    
    public GameObject dialogUI;
    public GameObject option_pre;
    private RectTransform root;
    private Transform optionUI;
    
    private DialogueGraph dialogueGraph;
    private TMP_Text content;
    private TMP_Text speaker;
    private Image characterImg;
    private Node curNode;
    private bool isRunning;
    private bool isInOptionNode=false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        root = dialogUI.transform.Find("Panel").GetComponent<RectTransform>();
        optionUI=dialogUI.transform.Find("Panel/Option").GetComponent<Transform>();
        content = dialogUI.transform.Find("Panel/Chat/Content").GetComponent<TMP_Text>();
        speaker = dialogUI.transform.Find("Panel/Chat/Speaker").GetComponent<TMP_Text>();
        characterImg = dialogUI.transform.Find("Panel/Chat/Character").GetComponent<Image>();
    }

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
        {
            ExecuteCurrentNode();
        }
    }

    public void LoadDialogGraph(DialogueGraph graph)
    {
        dialogueGraph = graph;
        var startNodes = dialogueGraph.nodes.FindAll((x) =>x.GetType()==typeof(StartNode));
        if (startNodes.Count==0)
        {
            Debug.LogError("未创建开始节点！");
            return;
        }
        else if (startNodes.Count > 1)
        {
            Debug.LogError("有多个开始节点！");
            return;
        }
        else curNode = startNodes[0];
        curNode = curNode.GetOutputPort("start")?.Connection.node;
        isRunning = true;
        ExecuteCurrentNode();
    }
    
    private void ExecuteCurrentNode()
    {
        if (curNode==null)
        {
            Debug.LogError("当前节点为空！");
            return;
        }

        switch (curNode)
        {
           case ChatNode chatNode:
               PlayChatNode(chatNode);
               break;
           case OptionNode optionNode:
               if (!isInOptionNode)
               {
                   PlayOptionNode(optionNode);
                   isInOptionNode = true;
               }
               break;
           case EndNode endNode:
               PlayEndNode(endNode);
               break;
        }
    }

    /// <summary>
    /// 点击选择框时跳转到下一个节点并隐藏选择UI
    /// </summary>
    /// <param name="id">被点击选项的id</param>
    public void ClickOptionUnitCallBack(int id)
    {
        if (curNode is OptionNode node)
        {
            curNode=node.getNextNode(id);
            for (int i = 0; i < optionUI.childCount; i++)
            {
                Destroy(optionUI.GetChild(i).gameObject);
            }
            ExecuteCurrentNode();
            optionUI.DOScale(new Vector3(0, 0, 0), 1.0f);
            optionUI.gameObject.SetActive(false);
            isInOptionNode = false;
        }
        else Debug.LogError("当前节点不是选择节点！");
    }

    private void PlayOptionNode(OptionNode node)
    {
        optionUI.gameObject.SetActive(true);
        optionUI.DOScale(new Vector3(1, 1, 1), 1.0f);
        for (int i = 0; i < node.branches.Count; i++)
        {
            var option=Instantiate(option_pre, optionUI,false);
            option.GetComponent<OptionUI>().id = i;
            option.GetComponentInChildren<TMP_Text>().text=node.branches[i];
        }
    }

    private void PlayEndNode(EndNode node)
    {
        Debug.Log("dialogue has done");
        isRunning = false;
        Tweener tween = root.DOScale(new Vector3(0, 0, 0), 1.0f);
        tween.SetEase(Ease.Linear);
    }

    private void PlayChatNode(ChatNode node)
    {
        if (node.getNextChatUnit(out ChatNode.ChatUnit unit))
        {
            updateUI(unit.charactor, unit.speaker, unit.content);
        }
        else
        {
            //遍历完chatnode所有对话则跳转到下一个节点并执行节点
            curNode = curNode.GetOutputPort("nextDialog")?.Connection.node;
            ExecuteCurrentNode();
        }
    }

    private void updateUI(Sprite character, string speaker, string content)
    {
        this.characterImg.sprite = character;
        this.speaker.text = speaker;
        this.content.text = content;
    }
}
