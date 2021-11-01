using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAbilityManager : MonoBehaviour
{
    public static SwitchAbilityManager instance;
    public SwitchState currentState;
    private List<SwitchableObject> list = new List<SwitchableObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
        currentState = SwitchState.fiction;
        SwitchAllTo(currentState);
    }

    public void Register(SwitchableObject switchableObejct)
    {
        list.Add(switchableObejct);
    }

    public void DeRegister(SwitchableObject switchableObejct)
    {
        list.Remove(switchableObejct);
    }

    public void SwitchToAnother()
    {
        currentState = currentState==SwitchState.reality ? SwitchState.fiction : SwitchState.reality;
        SwitchAllTo(currentState);
    }

    public void SwitchAllTo(SwitchState state)
    {
        foreach (var switchableObejct in list)
        {
            switchableObejct.SwitchTo(state);
        }
    }

 
}
