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

    public static int IndexX = 9;
    public static int IndexY = 9;

    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;
    public const float REWARDED_COOLDOWN = 70;
    public const float INTERSTITIAL_COOLDOWN = 63;

    public const float MOVE_CELL_SPEED = 0.175f;
    public const float UNION_CELL_SPEED = 0.3f;
    public const float WAIT_CELL_BLOW = 0.15f;//0.09
    public const float FALL_CELL_SPEED = 0.1f;//0.081
    public const float MOVE_CELL_DISTANCE = 1f;
    public const Ease MOVE_CELL_EASE = Ease.OutSine;
    public const Ease FALL_CELL_EASE = Ease.Linear;
    public const float APPEAR_SCALE_SPEED = 0.1f;

    public static bool IsMobile;
    public static bool Is6CellAvailable;
    public static bool IsOptions;
    public static bool IsLowFPS;
    public static bool IsLostPrevious;

    public static int HowManyLost;

    public static bool isScreenSaverWithSign;


    public const float SCREEN_SAVER_AWAIT = 0.4f;
    public const int GOLD_PER_MOVE = 10;
    public const int GOLD_FOR_WIN = 20;

    public const float PLAYER_UPDATE_JOYSTICK_COOLDOWN = 0.1f;
    public const float PLAYER_BASE_MAXSPEED = 6f;
    public const float PLAYER_DEATH_WAIT_ANIMATION = 1.5f;
    public const float COOLDOWN_CHECK_DEAD_PLAYERS = 0.3f;
    public const float COOLDOWN_CHECK_DEAD_NPC = 0.3f;
    public const float COOLDOWN_UPDATE_ATTACK_NPC = 0.1f;
    public const float COOLDOWN_UPDATE_ATTACK_PLAYER = 0.1f;
    public const float HORS_VERTS_SPEED = 11f;//13f;

    public const float MOB_SCALE_UI = 1.6f;
    public const float PC_SCALE_UI = 0.6f;

    public const int WIN_REWARD = 50;

    public static MusicTypes LastPlayedMelody;

    
    public static int lastTutorForScreen = 0;

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
