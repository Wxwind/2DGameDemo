using System;
using System.Collections;
using System.Collections.Generic;
using DialogueGraphEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Barrage : MonoBehaviour
{
    public Vector2 minMaxSpeed;
    public float duringTime = 20f;
    public float intervalTime = 2f;
    public Transform pos1;
    public Transform pos2;
    public DialogueGraph barrageDia;
    private Timer barrageTimer;
    private Timer intervalTimer;
    private bool isEmissing = true;

    private void Awake()
    {
        barrageTimer = new Timer(duringTime, StopEmissing, true);
        intervalTimer = new Timer(intervalTime, Emit, true);
        pos1 = transform.Find("pos1");
        pos2 = transform.Find("pos2");
    }

    private void Update()
    {
        barrageTimer.Tick(Time.deltaTime);
        if (isEmissing)
        {
            if (intervalTimer.IsFinished)
            {
                intervalTimer.ReRun();
            }

            intervalTimer.Tick(Time.deltaTime);
        }
    }

    private void Emit()
    {
        float count = Random.Range(2, 6);
        for (int i = 0; i < count; i++)
        {
            float speed = -Random.Range(minMaxSpeed.x, minMaxSpeed.y);
            float posX = Random.Range(pos1.position.x, pos2.position.x);
            var thumbtack = ThumbtackPool.instance.GetFromPool().GetComponent<Thumbtack>();
            Vector3 position = new Vector3(posX, pos1.position.y, pos1.position.z);
            thumbtack.Init(speed,position);
           
        }
    }

    private void StopEmissing()
    {
        isEmissing = false;
    }
}