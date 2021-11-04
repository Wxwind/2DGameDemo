using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using XNode;
namespace DialogueGraph
{
    [NodeWidth(400)]
    [NodeTint(200, 200, 200)]
    public class SimpleNode : Node
    {
        [Input(ShowBackingValue.Never)] public int lastDialog;
        [PreviewField]public Sprite charactor;
        public string text;
        [Output] public int nextDialog;
        
        public override object GetValue(NodePort port)
        {
            return 5;
        }

    }
}
