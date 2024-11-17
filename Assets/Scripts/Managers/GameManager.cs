using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    

    private void Awake()
    {
        if (Globals.IsMobile && !Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 2;
            QualitySettings.shadowDistance = 40;
            QualitySettings.shadows = ShadowQuality.HardOnly;
        }
        else if (Globals.IsMobile && Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 0;
            QualitySettings.shadows = ShadowQuality.Disable;
        }
        else if (!Globals.IsMobile && !Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 4;
            QualitySettings.shadowDistance = 60;
            QualitySettings.shadows = ShadowQuality.All;
        }
        else if (!Globals.IsMobile && Globals.IsLowFPS)
        {
            QualitySettings.antiAliasing = 2;
            QualitySettings.shadowDistance = 40;
            QualitySettings.shadows = ShadowQuality.HardOnly;
        }

        //QualitySettings.antiAliasing = 0;
        //QualitySettings.shadows = ShadowQuality.Disable;
    }



    
}
