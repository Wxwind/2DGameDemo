using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Test : SerializedMonoBehaviour
{
    [PreviewField]
    [LabelText("这是精灵")]
    public Sprite m_sprite;
    [LabelText("这是字典")]
    public Dictionary<string, int> m_dic = new Dictionary<string, int>();
    [ReadOnly]
    public int m_int;
    public string m_string = "abc";
    [Button("按钮",30)]
    private void Testbutton()
    {
        Debug.Log("!");
       
    }
}
