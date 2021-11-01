using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPlatform_F : MonoBehaviour
{
    public FloatPlatform_R r;
    private void OnEnable()
    {
        //如果初始状态是fiction，注意设置r.transform.position等于pos0（第一个检查点）
        transform.position = r.transform.position;
    }
}
