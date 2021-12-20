using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [Header("当前输入")]
    [ReadOnly] public KeyCode currentKey;
    
    [Header("按键")]
    [ReadOnly] public KeyCode upKey;
    [ReadOnly] public KeyCode downKey;
    [ReadOnly] public KeyCode leftKey;
    [ReadOnly] public KeyCode rightKey;
    [ReadOnly] public KeyCode attackKey;
    [ReadOnly] public KeyCode abilityKey;
    [ReadOnly] public KeyCode jumpKey;
    [ReadOnly] public KeyCode dashKey;
    [ReadOnly] public int xInput=0;
    [ReadOnly] public int faceDir=1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        KeyInit();
    }

    private void KeyInit()
    {
        upKey = KeyCode.W;
        downKey = KeyCode.S;
        leftKey = KeyCode.A;
        rightKey = KeyCode.D;
        //attackKey = KeyCode.K;
        abilityKey = KeyCode.K;
        jumpKey = KeyCode.J;
        dashKey = KeyCode.L;
    }
    private void OnGUI()
    {
        Event e = Event.current;

        if (Input.anyKeyDown)
        {
            if (e.isKey)
            {
                if (e.keyCode!=KeyCode.None)
                {
                    currentKey = e.keyCode;                 
                }
            }
        }
    }
}
