using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
namespace DialogueGraph
{
    [NodeWidth(400)]
    [NodeTint(73, 236, 209)]//Node颜色
    public class SimpleNode : Node
    {
        [Input] public int a;
        [Input] public int b;
        [Output] public int nextDialog;
        public string text;

        public override object GetValue(NodePort port)
        {
            return GetSum();
        }
        public float GetSum()
        {
            return a + b;
        }

    }
}
