using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCharacter : TimeAgent
{
    public NPCDefinition character;

    [Range(0, 100)] public int relationship;

    public bool talkedToDay;
    public int talkedOnTheDayNumber = -1;

    private void Start()
    {
        Init();
        onTimeTick += ResetTalkState;
    }

    internal void IncreaseRelationship(int v)
    {
        if (talkedToDay == false) 
        {
            relationship += v;
            talkedToDay = true; 
        }
    }

    internal void DecreaseRelationship(int v)
    {
        relationship -= v;
    }

    void ResetTalkState(DayTimeController dayTimeController)
    {
        if (dayTimeController.days != talkedOnTheDayNumber)
        {
            talkedToDay = false;
            talkedOnTheDayNumber = dayTimeController.days;
        }
    }
}
