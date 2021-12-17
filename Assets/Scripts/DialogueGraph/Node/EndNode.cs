using System;
using System.Collections;
using System.Collections.Generic;
using DialogueGraphEditor;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace  DialogueGraphEditor
{
    [CreateNodeMenu("结束")]
    public class EndNode : Node
    {
        [Input(ShowBackingValue.Never)] public int end;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}