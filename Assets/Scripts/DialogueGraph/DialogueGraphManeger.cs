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
    public float txtSpeed;
    private RectTransform root;
    private Transform optionUI;
    
    private DialogueGraph dialogueGraph;
    private TMP_Text content;
    private TMP_Text speaker;
    private Image characterImg;
    private Node curNode;
    private GameObject hint;
    private bool isRunning;
    private bool isFinished=true;
    private bool isInOptionNode=false;
    private Timer freezeTimer;
    
    private Action OnStart;
    public Action OnEnd;

    //在对话运行的时候让主角不动
    public GameObject player;
    private PlayerController pc;
    private Rigidbody2D rb;

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
        hint=dialogUI.transform.Find("Panel/Hint").gameObject;
        freezeTimer = new Timer(0, ()=>isRunning = true);
        
        pc = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        freezeTimer.Tick(Time.deltaTime);
        if (!isRunning)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
        {
            ExecuteCurrentNode();
            hint.SetActive(false);
        }
    }

    public void ExecuteDialogGraph(DialogueGraph graph)
    {
        LoadDialogGraph(graph,() =>
            {
                pc.enabled = false;
                rb.velocity = Vector2.zero;
                pc.currentSpeed=Vector2.zero;
                pc.isIdling = true;
            },
            () => { pc.enabled = true; });
    
    }
    

    public void LoadDialogGraph(DialogueGraph graph,Action OnStart,Action OnEnd)
    {
        if (!isFinished)
        {
            Debug.LogError("有对话正在运行，无法加载DialogGraph");
            return;
        }
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
        curNode = startNodes[0];
        curNode = curNode.GetOutputPort("start")?.Connection.node;
        isRunning = true;
        isFinished = false;
        if (!dialogUI.activeSelf)
        {
           dialogUI.transform.localScale = new Vector3(1, 1, 1);
           dialogUI.SetActive(true); 
        }

        this.OnStart = OnStart;
        this.OnEnd = OnEnd;
        OnStart();
        ExecuteCurrentNode();
    }
    
    private void ExecuteCurrentNode()
    {
        if (curNode==null)
        {
            Debug.LogError("当前节点为空！");
            isRunning = false;
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
        Debug.Log("对话播放完毕");
        isRunning = false;
        Tweener tween = root.DOScale(new Vector3(0, 0, 0), 1.0f);
        tween.SetEase(Ease.Linear);
        tween.OnComplete(() => dialogUI.SetActive(false));
        isFinished = true;
        OnEnd();
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
        float duration = content.Length / txtSpeed;
        this.content.text = "";
        this.content.DOText(content, duration).OnComplete(() =>
        {
            //this.content.text += "\n(点击以继续...)";
            hint.SetActive(true);
        });
        isRunning = false;
        freezeTimer.ResetTimerAndRun(duration);
    }
    
}
