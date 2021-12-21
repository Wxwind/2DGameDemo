using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject restartMenu;
    public float waitForMenuTime;
    private Timer menuTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        menuTimer = new Timer(waitForMenuTime, () => restartMenu.SetActive(true),true);    
    }

    private void Update()
    {
        menuTimer.Tick(Time.deltaTime);
    }
    
}

