using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class DayTimeCycle : MonoBehaviour
{
    [Inject] private Camera _camera;
    [Inject] private PlayerControl pc;

    [SerializeField] private Light mainSun;
    [SerializeField] private Material materialForTransparent;
    [SerializeField] private Material[] materialForNonTransparent;

    private Transform cameraTransform;
    private Transform sunTransform;
    private float currentTime;
    private float currentDay;

    //Sun data
    private int currentHourForLight;
    private float maxLightIntensity = 1.2f;
    private float midLightIntensity = 0.7f;
    private float minLightIntensity = 0.2f;
    private float from6AngleStart = 25f;
    private float to21AngleEnd = 155f;

    //Colors for back camera
    private Color maxDayColor = new Color(220f / 255f, 1f, 1f);
    private Color minDayColor = new Color(0, 0, 0);
    private Color startDayColor = new Color(220f / 255f, 226f / 255f, 226f / 255f);

    //Transparent material data
    private Color maxMatLight = new Color(1, 1, 1, 1);
    private Color mediumMatLight = new Color(0.8f, 0.8f, 0.8f, 1);
    private Color minMaxLight = new Color(0.2f, 0.2f, 0.2f, 1);


    //Sun and Moon
    [SerializeField] private Transform baseSunMoon;
    [SerializeField] private Transform sun;
    [SerializeField] private Transform moon;
    [SerializeField] private GameObject stars;
    private Transform player;
    private float _timer;
    private float _cooldown = 0.1f;

    public int CurrentHour()
    {
        return (int)(currentTime / 60f / 60f);
    }

    public int CurrentMinutes()
    {
        return (int)(currentTime / 60f - CurrentHour() * 60);
    }

    private void OnApplicationQuit()
    {
        materialForTransparent.SetColor("_Color", maxDayColor);
        materialForNonTransparent.ToList().ForEach(m => m.SetColor("_Color", maxDayColor));
    }

    private void Awake()
    {
        currentTime = Globals.MainPlayerData.T;
        currentDay = Globals.MainPlayerData.D;

        cameraTransform = _camera.transform;
        sunTransform = mainSun.transform;

        player = pc.transform;
        baseSunMoon.position = player.position;        
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

        //print(CurrentHour() + " : " + CurrentMinutes());

        lightDuringDay(CurrentHour());

        if (_timer > _cooldown)
        {
            _timer = 0;
            baseSunMoon.position = player.position;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private void lightDuringDay(int hour)
    {
        if (currentHourForLight == hour) { return; }
        currentHourForLight = hour;

        sunRotation(hour);
        colorsAndLights(hour);                
    }

    private void sunRotation(int hour)
    {
        //SUN rotating
        if (hour >= 6 && hour < 12)
        {
            float lerpForCurrentHour = (hour - 6) / 6f;

            float curAngle = Mathf.Lerp(from6AngleStart, 90, lerpForCurrentHour);
            sunTransform.localEulerAngles = new Vector3(curAngle, 0, 0);

            float _timer = (12 - hour) * 60 * 60 / Globals.TIME_SPEED_KOEF;
            sunTransform.DOLocalRotate(new Vector3(90, 0, 0), _timer).SetEase(Ease.Linear);

            moon.gameObject.SetActive(false);
            stars.gameObject.SetActive(false);
            sun.gameObject.SetActive(true);
            sun.localEulerAngles = new Vector3(Mathf.Lerp(-90, 0, lerpForCurrentHour), 0, 0);
            sun.DOLocalRotate(Vector3.zero, _timer).SetEase(Ease.Linear);
        }
        else if (hour >= 12 && hour < 21)
        {
            float lerpForCurrentHour = (hour - 12) / 9f;

            float curAngle = Mathf.Lerp(90, to21AngleEnd, lerpForCurrentHour);
            sunTransform.localEulerAngles = new Vector3(curAngle, 0, 0);

            float _timer = (21 - hour) * 60 * 60 / Globals.TIME_SPEED_KOEF;
            sunTransform.DOLocalRotate(new Vector3(to21AngleEnd, 0, 0), _timer).SetEase(Ease.Linear);

            moon.gameObject.SetActive(false);
            stars.gameObject.SetActive(false);
            sun.gameObject.SetActive(true);
            sun.localEulerAngles = new Vector3(Mathf.Lerp(0, 90, lerpForCurrentHour), 0, 0);
            sun.DOLocalRotate(new Vector3(90,0,0), _timer).SetEase(Ease.Linear);
        }
        else if (hour >= 21 && hour < 22)
        {
            float _timer = 1 * 60 * 60 / Globals.TIME_SPEED_KOEF;
            sunTransform.localEulerAngles = new Vector3(to21AngleEnd, 0, 0);
            sunTransform.DOLocalRotate(new Vector3(270, 0, 0), _timer).SetEase(Ease.Linear);

            moon.gameObject.SetActive(false);
            stars.gameObject.SetActive(false);
            sun.gameObject.SetActive(true);
            sun.localEulerAngles = new Vector3(90, 0, 0);
            sun.DOLocalRotate(new Vector3(130, 0, 0), _timer).SetEase(Ease.Linear);
        }
        else if ((hour >= 22 && hour <= 23) || (hour >= 0 && hour < 5))
        {
            sunTransform.localEulerAngles = new Vector3(270, 0, 0);
            sun.gameObject.SetActive(false);
            stars.gameObject.SetActive(true);
            moon.gameObject.SetActive(true);
            float _timer = 1 * 60 * 60 / Globals.TIME_SPEED_KOEF;
            switch (hour)
            {
                case 22:
                    
                    moon.localEulerAngles = new Vector3(-120, 0, 0);
                    moon.DOLocalRotate(new Vector3(-100, 0, 0), _timer).SetEase(Ease.Linear);
                    break;

                case 23:
                    moon.localEulerAngles = new Vector3(-100, 0, 0);
                    moon.DOLocalRotate(new Vector3(-75, 0, 0), _timer).SetEase(Ease.Linear);
                    break;

                case 0:
                    moon.localEulerAngles = new Vector3(-75, 0, 0);
                    moon.DOLocalRotate(new Vector3(-40, 0, 0), _timer).SetEase(Ease.Linear);
                    break;

                case 1:
                    moon.localEulerAngles = new Vector3(-40, 0, 0);
                    moon.DOLocalRotate(new Vector3(0, 0, 0), _timer).SetEase(Ease.Linear);
                    break;

                case 2:
                    moon.localEulerAngles = new Vector3(0, 0, 0);
                    moon.DOLocalRotate(new Vector3(40, 0, 0), _timer).SetEase(Ease.Linear);
                    break;

                case 3:
                    moon.localEulerAngles = new Vector3(40, 0, 0);
                    moon.DOLocalRotate(new Vector3(75, 0, 0), _timer).SetEase(Ease.Linear);
                    break;

                case 4:
                    moon.localEulerAngles = new Vector3(75, 0, 0);
                    moon.DOLocalRotate(new Vector3(110, 0, 0), _timer).SetEase(Ease.Linear);
                    break;
            }                        
        }
        else if (hour >= 5 && hour < 6)
        {
            float _timer = 1 * 60 * 60 / Globals.TIME_SPEED_KOEF;
            sunTransform.localEulerAngles = new Vector3(270, 0, 0);
            sunTransform.DOLocalRotate(new Vector3(385, 0, 0), _timer, RotateMode.FastBeyond360).SetEase(Ease.Linear);

            stars.gameObject.SetActive(false);
            moon.gameObject.SetActive(false);
            sun.gameObject.SetActive(true);
            sun.localEulerAngles = new Vector3(-130, 0, 0);
            sun.DOLocalRotate(new Vector3(-90, 0, 0), _timer).SetEase(Ease.Linear);
        }        
    }

    private void colorsAndLights(int hour)
    {
        //Colors and lights
        if (hour >= 6 && hour < 10)
        {
            float lerpForCurrentHour = (hour - 6) / 4f;

            Color curLight = Color.Lerp(startDayColor, maxDayColor, lerpForCurrentHour);
            _camera.backgroundColor = curLight;

            float _timer = (10 - hour) * 60 * 60 / Globals.TIME_SPEED_KOEF;
            _camera.DOColor(maxDayColor, _timer).SetEase(Ease.Linear);

            Color curColor = Color.Lerp(mediumMatLight, maxMatLight, lerpForCurrentHour);
            materialForTransparent.SetColor("_Color", curColor);
            materialForTransparent.DOColor(maxMatLight, "_Color", _timer).SetEase(Ease.Linear);
            materialForNonTransparent.ToList().ForEach(m=> m.SetColor("_Color", curColor));
            materialForNonTransparent.ToList().ForEach(m => m.DOColor(maxMatLight, "_Color", _timer).SetEase(Ease.Linear));

            float curIntensity = Mathf.Lerp(midLightIntensity, maxLightIntensity, lerpForCurrentHour);
            mainSun.intensity = curIntensity;
            mainSun.DOIntensity(maxLightIntensity, _timer).SetEase(Ease.Linear);

        }        
        else if (hour >= 10 && hour < 16)
        {
            _camera.backgroundColor = maxDayColor;
            materialForTransparent.SetColor("_Color", maxMatLight);
            materialForNonTransparent.ToList().ForEach(m => m.SetColor("_Color", maxMatLight));
            mainSun.intensity = maxLightIntensity;
        }
        else if (hour >= 16 && hour < 21)
        {
            float lerpForCurrentHour = (hour - 16) / 5f;

            Color curLight = Color.Lerp(maxDayColor, startDayColor, lerpForCurrentHour);
            _camera.backgroundColor = curLight;

            float _timer = (21 - hour) * 60 * 60 / Globals.TIME_SPEED_KOEF;
            _camera.DOColor(startDayColor, _timer).SetEase(Ease.Linear);

            Color curColor = Color.Lerp(maxMatLight, mediumMatLight, lerpForCurrentHour);
            materialForTransparent.SetColor("_Color", curColor);
            materialForTransparent.DOColor(mediumMatLight, "_Color", _timer).SetEase(Ease.Linear);
            materialForNonTransparent.ToList().ForEach(m => m.SetColor("_Color", curColor));
            materialForNonTransparent.ToList().ForEach(m => m.DOColor(mediumMatLight, "_Color", _timer).SetEase(Ease.Linear));

            float curIntensity = Mathf.Lerp(maxLightIntensity, midLightIntensity, lerpForCurrentHour);
            mainSun.intensity = curIntensity;
            mainSun.DOIntensity(midLightIntensity, _timer).SetEase(Ease.Linear);
        }
        else if (hour >= 21 && hour < 22)
        {
            float _timer = 1 * 60 * 60 / Globals.TIME_SPEED_KOEF;

            _camera.backgroundColor = startDayColor;
            _camera.DOColor(minDayColor, _timer).SetEase(Ease.Linear);

            materialForTransparent.SetColor("_Color", mediumMatLight);
            materialForTransparent.DOColor(minMaxLight, "_Color", _timer).SetEase(Ease.Linear);
            materialForNonTransparent.ToList().ForEach(m => m.SetColor("_Color", mediumMatLight));
            materialForNonTransparent.ToList().ForEach(m => m.DOColor(minMaxLight, "_Color", _timer).SetEase(Ease.Linear));

            mainSun.intensity = midLightIntensity;
            mainSun.DOIntensity(minLightIntensity, _timer).SetEase(Ease.Linear);
        }
        else if ((hour >= 22 && hour <= 23) || (hour >= 0 && hour < 5))
        {
            _camera.backgroundColor = minDayColor;
            materialForTransparent.SetColor("_Color", minMaxLight);
            materialForNonTransparent.ToList().ForEach(m => m.SetColor("_Color", minMaxLight));
            mainSun.intensity = minLightIntensity;
        }
        else if (hour >= 5 && hour < 6)
        {
            float _timer = 1 * 60 * 60 / Globals.TIME_SPEED_KOEF;

            _camera.backgroundColor = minDayColor;
            _camera.DOColor(startDayColor, _timer).SetEase(Ease.Linear);

            materialForTransparent.SetColor("_Color", minMaxLight);
            materialForTransparent.DOColor(mediumMatLight, "_Color", _timer).SetEase(Ease.Linear);
            materialForNonTransparent.ToList().ForEach(m => m.SetColor("_Color", minMaxLight));
            materialForNonTransparent.ToList().ForEach(m => m.DOColor(mediumMatLight, "_Color", _timer).SetEase(Ease.Linear));

            mainSun.intensity = minLightIntensity;
            mainSun.DOIntensity(midLightIntensity, _timer).SetEase(Ease.Linear);
        }
    }
}
