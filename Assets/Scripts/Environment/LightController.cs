using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private float range = 50;
    [SerializeField] private float intensityDay = 1;
    [SerializeField] private float intensityNight = 8;

    private DayTimeCycle dayTimer;
    private bool nightChecked;
    private bool dayChecked;

    private void Start()
    {
        dayTimer = GameObject.Find("=====MAIN=====").GetComponent<DayTimeCycle>();
        light.range = range;
        light.intensity = intensityDay;
    }


    // Update is called once per frame
    void Update()
    {
        if (!nightChecked && ((dayTimer.CurrentHour() >= 22) || (dayTimer.CurrentHour() >= 0 && dayTimer.CurrentHour() <= 5)))
        {
            nightChecked = true;
            dayChecked = false;

            if (light.intensity < intensityNight)
            {
                light.intensity = intensityNight;
            }
        }

        if (!dayChecked && dayTimer.CurrentHour() < 22 && dayTimer.CurrentHour() > 5)
        {
            dayChecked = true;
            nightChecked = false;

            if (light.intensity > intensityDay)
            {
                light.intensity = intensityDay;
            }
        }
    }
}
