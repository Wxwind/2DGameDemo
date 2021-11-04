using System.Collections;
using System.Collections.Generic;
using System;

public class Timer
{
    private readonly float time;
    private Action OnComplete;
    private float value;

    public bool IsRunning { get; private set; }
    public bool IsFinished { get; private set; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">计时器计时的时间</param>
    /// <param name="OnComplete">计时结束时的回调</param>
    public Timer(float time,Action OnComplete=null,bool active=false)
    {
        this.time = time;
        this.OnComplete = OnComplete;
        this.value = 0;
        this.IsRunning = active;
    }

    public void SetCallback(Action OnComplete)
    {
        this.OnComplete = OnComplete ?? this.OnComplete;
    }

    public void Tick(float dt)
    {
        if (!IsRunning|| IsFinished)
        {
            return;
        }
        value += dt;
        if (value>=time)
        {
            End();
        }
    }

    public void End()
    {
        OnComplete?.Invoke();
        IsRunning = false;
        IsFinished = true;
    }

    public void ResetAndRun()
    {
        value = 0;
        IsRunning = true;
        IsFinished = false;
    }

    public void Stop()
    {
        this.IsRunning = false;
    }

    public void Run()
    {
        this.IsRunning = true;
    }
}
