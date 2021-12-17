using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

[CreateNodeMenu("分支选项")]
public class OptionNode : Node
{
    [Input(ShowBackingValue.Never)] public int lastDialog;
    [Output(dynamicPortList = true), LabelText("分支选项"), TextArea]
    public List<string> branches = new List<string>();

    public override object GetValue(NodePort port)
    {
        return null;
    }

    public Node getNextNode(int id)
    {
        return GetOutputPort("branches" + " " + id).Connection.node;
    }
}
