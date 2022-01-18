using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossSkill : MonoBehaviour
{
    /// <summary>
    /// 0:待机不动
    /// 1:发射激光
    /// 2.场上出现木板，跳到最上面的木板可以攻击，同时boss移动到最上面
    /// </summary>
    public int state;

    public GameObject beamPre;
    public Vector3 stateTwoPos;
    public Vector3 idlePos;
    public Timer stateLastTimer;
    public bool freezeStateMachine;
    public GameObject wood;
    private int lastSkill=0;


    void Start()
    {
        state = 0;
        stateLastTimer = new Timer(5, () =>
        {
            state = 0;
            freezeStateMachine = false;
        });
    }


    void Update()
    {
        stateLastTimer.Tick(Time.deltaTime);
        if (freezeStateMachine)
        {
            return;
        }

        switch (state)
        {
            case 0:
                int a;
                if (lastSkill == 0)
                {
                    a = Random.Range(1, 3);
                }
                else a = 3 - lastSkill;
                transform.DOMove(idlePos, 2.0f).OnComplete(() =>
                {
                    state = a;
                    freezeStateMachine = false;
                });
                freezeStateMachine = true;
                break;
            case 1:
                lastSkill = 1;
                var emitBeam = gameObject.AddComponent<EmitBeam>();
                emitBeam.Init(2,1.5f,beamPre);
                stateLastTimer.ResetTimerAndRun(2, () =>
                {
                    state = 0;
                    freezeStateMachine = false;
                });
                freezeStateMachine = true;
                break;
            case 2:
                lastSkill = 2;
                transform.DOMove(stateTwoPos, 1.0f).OnComplete(() => wood.SetActive(true));
                stateLastTimer.ResetTimerAndRun(8, () =>
                {
                    state = 0;
                    freezeStateMachine = false;
                    wood.SetActive(false);
                });
                freezeStateMachine = true;
                break;
        }
    }
}