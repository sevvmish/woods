using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class DayTimeCycle : MonoBehaviour
{
    [Inject] private Camera _camera;

    [SerializeField] private Light mainSun;
    [SerializeField] private Material materialForLight;

    private Transform cameraTransform;
    private Transform sunTransform;
    private float currentTime;
    private float currentDay;

    private int currentHourForLight;
    private float maxLightIntensity = 1.2f;
    private float minLightIntensity = 0.2f;
    private float from6AngleStart = 25f;
    private float to21AngleEnd = 155f;

    private Color maxDayColor = new Color(210f / 255f, 1f, 1f);
    private Color minDayColor = new Color(0, 0, 0);
    private Color startDayColor = new Color(1f, 1f, 210f / 255f);

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

        cameraTransform = _camera.transform;
        sunTransform = mainSun.transform;
                
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * Globals.TIME_SPEED_KOEF;

        if (currentTime >= 86400)
        {
            currentTime = 0;
            currentDay++;
            Globals.MainPlayerData.D++;
            SaveLoadManager.Save();
        }

        print(CurrentHour() + " : " + CurrentMinutes());

        lightDuringDay(CurrentHour());

    }

    private void lightDuringDay(int hour)
    {
        if (currentHourForLight == hour) { return; }
        currentHourForLight = hour;

        if (hour >= 6 && hour < 12)
        {
            float curAngle = Mathf.Lerp(from6AngleStart, 90, (hour - 6) / 6f);
            sunTransform.localEulerAngles = new Vector3(curAngle, 0, 0);

            Color curLight = Color.Lerp(startDayColor, maxDayColor, (hour - 6) / 6f);
            _camera.backgroundColor = curLight;
            

            float _timer = (12 - hour)*60*60/Globals.TIME_SPEED_KOEF;
            sunTransform.DOLocalRotate(new Vector3(90,0,0), _timer).SetEase(Ease.Linear);
            _camera.DOColor(maxDayColor, _timer).SetEase(Ease.Linear);
        }
        else if (hour >= 12 && hour < 21)
        {
            float curAngle = Mathf.Lerp(90, to21AngleEnd, (hour - 12) / 9f);
            sunTransform.localEulerAngles = new Vector3(curAngle, 0, 0);

            Color curLight = Color.Lerp(maxDayColor, startDayColor, (hour - 12) / 9f);
            _camera.backgroundColor = curLight;

            float _timer = (21 - hour) * 60 * 60 / Globals.TIME_SPEED_KOEF;
            sunTransform.DOLocalRotate(new Vector3(to21AngleEnd, 0, 0), _timer).SetEase(Ease.Linear);
            _camera.DOColor(startDayColor, _timer).SetEase(Ease.Linear);
        }
        else if (hour >= 21 && hour < 22)
        {
            float _timer = 1 * 60 * 60 / Globals.TIME_SPEED_KOEF;
            sunTransform.localEulerAngles = new Vector3(to21AngleEnd, 0, 0);
            sunTransform.DOLocalRotate(new Vector3(270, 0, 0), _timer).SetEase(Ease.Linear);

            _camera.backgroundColor = startDayColor;
            _camera.DOColor(minDayColor, _timer).SetEase(Ease.Linear);
        }
        else if (hour >= 22 && hour <5)
        {
            sunTransform.localEulerAngles = new Vector3(270, 0, 0);
            _camera.backgroundColor = minDayColor;
        }
        else if (hour >= 5 && hour < 6)
        {
            float _timer = 1 * 60 * 60 / Globals.TIME_SPEED_KOEF;
            sunTransform.localEulerAngles = new Vector3(270, 0, 0);
            sunTransform.DOLocalRotate(new Vector3(385, 0, 0), _timer, RotateMode.FastBeyond360).SetEase(Ease.Linear);

            _camera.backgroundColor = minDayColor;
            _camera.DOColor(startDayColor, _timer).SetEase(Ease.Linear);
        }
    }
}
