using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action<DayTimeController> onTimeTick;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GameManager.instance.dayTimeController.Subscribe(this);
    }
    public void Invoke(DayTimeController dayTimeController)
    {
        onTimeTick?.Invoke(dayTimeController);
    }

    private void OnDestroy()
    {
        GameManager.instance.dayTimeController.UnSubscribe(this);
    }
}
