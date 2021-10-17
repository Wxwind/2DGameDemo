using System.Collections;
using System.Collections.Generic;
using System;

public class Timer
{
    private float time;
    private Action OnComplete;

    private float value;

    public bool IsRunning { get; private set; }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="time">计时器计时的时间</param>
    /// <param name="OnComplete">计时结束时的回调</param>
    public Timer(float time,Action OnComplete=null)
    {
        this.time = time;
        this.OnComplete = OnComplete ?? this.OnComplete;
        this.value = 0;
        this.IsRunning = true;
    }

    public void Tick(float dt)
    {
        if (IsRunning == false)
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
        this.IsRunning = false;
    }
}
