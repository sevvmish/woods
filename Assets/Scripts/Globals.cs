using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static bool IsSoundOn;
    public static bool IsMusicOn;
    public static bool IsInitiated;
    public static bool IsMainMenuTutorial;
    public static string CurrentLanguage;
    public static Translation Language;

    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;
    public const float REWARDED_COOLDOWN = 70;
    public const float INTERSTITIAL_COOLDOWN = 63;

    public static bool IsMobile;
    public static bool IsOptions;
    public static bool IsLowFPS;

    public const float SCREEN_SAVER_AWAIT = 0.4f;
        
    public static MusicTypes LastPlayedMelody;

    public const float BASE_SPEED = 7f;
    public const float JUMP_POWER = 40f;
    public const float GRAVITY_KOEFF = 2.75f;
    public const float SPEED_INC_IN_NONGROUND_PC = 0.25f;
    public const float SPEED_INC_IN_NONGROUND_MOBILE = 0.35f; 
    public const float MASS = 1.5f; //3
    public const float DRAG = 5f; //1
    public const float ANGULAR_DRAG = 5f;

    public const float SHADOW_Y_DISTANCE = 8f;

    public static bool IsMobileChecker()
    {        
        //return true;

        if (Application.isMobilePlatform)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ShowBanners()
    {
        if (!Globals.MainPlayerData.AdvOff && YandexGame.SDKEnabled)
        {
            YandexGame.StickyAdActivity(true);
        }
    }

}
