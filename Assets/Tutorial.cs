using System;
using System.Collections;
using System.Collections.Generic;
using DialogueGraphEditor;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public DialogueGraph tutorialGraph;

    private void Start()
    {
        DialogueGraphManeger.instance.ExecuteDialogGraph(tutorialGraph);
    }
}
