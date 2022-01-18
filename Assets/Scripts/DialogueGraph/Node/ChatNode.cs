using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using XNode;

namespace DialogueGraphEditor
{
    [NodeWidth(300)]
    [NodeTint(200, 200, 200)]
    [CreateNodeMenu("普通对话")]
    public class ChatNode : Node
    {
        [Input(ShowBackingValue.Never)] public int lastDialog;
        [Output] public int nextDialog;
        [SerializeField]private List<ChatUnit> chats = new List<ChatUnit>();
        [System.NonSerialized]private int curIndex;
        
        public bool getNextChatUnit(out ChatUnit chatUnit)
        {
            if (curIndex+1>chats.Count)
            {
                chatUnit = null;
                return false;
            }
            chatUnit=chats[curIndex];
            curIndex++;
            return true;

        }

        public override object GetValue(NodePort port)
        {
            return null;
        }

        [System.Serializable]
        public class ChatUnit
        {
            [PreviewField]public Sprite charactor;
            public string speaker;
            [TextArea]public string content;
        }
        

    }
}
