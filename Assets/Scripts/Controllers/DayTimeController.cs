using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum DayofWeek
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}

public enum Season
{
    Winter,
    Spring,
    Summer,
    Autumn
}

public class DayTimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;
    const float phaseLenght = 900f; // 15 minutes chunk of time
    const float phasesInDay = 96f; // secondsInDay divided by phaseLength

    float time = 21600f;
    public int days;

    List<TimeAgent> agents;

    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor = Color.white;

    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] TMPro.TextMeshProUGUI dayOfTheWeekText;
    [SerializeField] TMPro.TextMeshProUGUI seasonText;
    [SerializeField] Light2D _light;

    [SerializeField] float timeScale = 48f;
    [SerializeField] float startAtTime = 28800f;
    [SerializeField] float morningTime = 28800f;

    DayofWeek dayofWeek;

    Season currentSeason;
    const int seasonLength = 30;

    private void Awake()
    {
        agents = new List<TimeAgent>();
    }

    private void Start()
    {
        time = startAtTime;
        UpdateDayText();
        UpdateSeasonText();
    }

    public void Subscribe(TimeAgent timeAgent)
    {
        agents.Add(timeAgent);
    }

    public void UnSubscribe(TimeAgent timeAgent)
    {
        agents.Remove(timeAgent);
    }

    private float Hours
    {
        get { return time / 3600f; }
    }

    private float Minutes
    {
        get { return time % 3600f / 60f; }
    }
    private void Update()
    {
        time += Time.deltaTime * timeScale;

        TimeValueCalculation();
        DayLight();

        if (time > secondsInDay)
        {
            NextDay();
        }

        TimeAgents();

        if (Input.GetKeyDown(KeyCode.T)) 
        {
            SkipTime(hours : 4);
        }
    }

    private void TimeValueCalculation()
    {
        int hour = (int)Hours;
        int minute = (int)Minutes;

        text.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    private void DayLight()
    {
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        _light.color = c;
    }

    int oldPhase = -1; 

    private void TimeAgents()
    {
        if(oldPhase == -1)
        {
            oldPhase = CalculatePhase();
        }

        int currentPhase = CalculatePhase();

        while(oldPhase < currentPhase)
        {
            oldPhase += 1;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Invoke(this);
            }
        }
    }

    private int CalculatePhase()
    {
        return (int)(time / phaseLenght) + (int)(days * phasesInDay);
    }

    private void NextDay()
    {
        time -= secondsInDay;
        days += 1;

        int dayNum = (int)dayofWeek;
        dayNum += 1;
        if (dayNum >= 7)
        {
            dayNum = 0;
        }
        dayofWeek = (DayofWeek)dayNum;
        UpdateDayText();

        if(days >= seasonLength)
        {
            NextSeason();
        }
    }

    private void NextSeason()
    {
        days = 0;
        int seasonNum = (int)currentSeason;
        seasonNum += 1;

        if (seasonNum >= 4)
        {
            seasonNum = 0;
        }

        currentSeason = (Season)seasonNum;
        UpdateSeasonText();
    }

    private void UpdateSeasonText()
    {
        seasonText.text = currentSeason.ToString();
    }

    private void UpdateDayText()
    {
        dayOfTheWeekText.text = dayofWeek.ToString();
    }

    private void SkipTime(float seconds = 0, float minute = 0, float hours = 0)
    {
        float timeToSkip = seconds;
        timeToSkip += minute * 60f;
        timeToSkip += hours * 3600f;

        time += timeToSkip;
    }

    internal void SkipToMorning()
    {
        float secondsToSkip = 0f;

        if(time > morningTime)
        {
            secondsToSkip += secondsInDay - time + morningTime; 
        }
        else
        {
            secondsToSkip += morningTime - time;
        }

        SkipTime(secondsToSkip);
    }
}
