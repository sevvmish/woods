using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeCycle : MonoBehaviour
{
    private float currentTime;
    private float currentDay;

    public int CurrentHour()
    {
        return (int)(currentTime / 60f / 60f);
    }

    public int CurrentMinutes()
    {
        return (int)(currentTime / 60f - CurrentHour() * 60);
    }


    private void Awake()
    {
        currentTime = Globals.MainPlayerData.T;
        currentDay = Globals.MainPlayerData.D;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * Globals.TIME_SPEED_KOEF;

        if (currentTime > 86400)
        {
            currentTime = 0;
            currentDay++;
            Globals.MainPlayerData.D++;
            SaveLoadManager.Save();
        }

        print(CurrentHour() + " : " + CurrentMinutes());
    }
}
