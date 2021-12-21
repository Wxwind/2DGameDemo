using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject restartMenu;
    private float waitForMenuTime;
    private Timer menuTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        waitForMenuTime = 2.0f;
        menuTimer = new Timer(waitForMenuTime, () => restartMenu.SetActive(true));    
    }

    private void Update()
    {
        menuTimer.Tick(Time.deltaTime);
    }

    private void Death()
    {
        menuTimer.ReRun();
    }
}

