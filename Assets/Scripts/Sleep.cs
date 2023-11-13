using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    DisableControls disableControls;
    Character character;
    DayTimeController dayTime;

    private void Awake()
    {
        disableControls = GetComponent<DisableControls>();
        character = GetComponent<Character>();
        dayTime = GameManager.instance.dayTimeController;
    }

    internal void DoSleep()
    {
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        ScreenTint screenTint = GameManager.instance.screenTint;

        disableControls.DisableControl();
        screenTint.Tint();
        yield return new WaitForSeconds(2f);

        character.FullHeal();
        character.FullRest();
        dayTime.SkipToMorning();

        
        screenTint.UnTint();
        disableControls.EnableControl();
        yield return new WaitForSeconds(2f);

        

        yield return null;
    }
}
