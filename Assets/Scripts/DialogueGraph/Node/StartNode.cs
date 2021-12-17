using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using XNode;
namespace DialogueGraphEditor
{
    [NodeWidth(100)]
    [NodeTint(0, 200, 200)]
    [CreateNodeMenu("开始")]
    public class StartNode : Node
    {
        [Output] public int start;

        public override object GetValue(NodePort port)
        {
            return null;
        }

    }
}
