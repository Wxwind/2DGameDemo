using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchState { reality, fiction }

public class SwitchableObject:MonoBehaviour
{ 
    public SwitchState currentState;  
    public GameObject realityObject;
    public GameObject fictionObject;

    private void Start()
    {

    }

    public void SwitchTo(SwitchState state)
    {
        currentState = state;
        if (state == SwitchState.reality)
        {         
            realityObject.gameObject.SetActive(true);
            fictionObject.gameObject.SetActive(false);
        }
        else
        {
            realityObject.gameObject.SetActive(false);
            fictionObject.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        SwitchAbilityManager.instance.Register(this);
    }
    private void OnDisable()
    {
        SwitchAbilityManager.instance.DeRegister(this);
    }

    private void OnDestroy()
    {
        SwitchAbilityManager.instance.DeRegister(this);
    }

}