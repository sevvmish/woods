using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        /*
        if (Globals.MainPlayerData == null)
        {

            Globals.MainPlayerData = new PlayerData();
            Globals.IsMobile = Globals.IsMobileChecker();
            Globals.IsSoundOn = true;
            Globals.IsMusicOn = true;
            Globals.IsInitiated = true;
            Globals.MainPlayerData.Gold = 2000;
            Globals.MainPlayerData.Resorter = 3;
            Globals.MainPlayerData.Trippler = 7;
            Globals.MainPlayerData.Backer = 4;

            Globals.Language = Localization.GetInstanse("ru").GetCurrentTranslation();
            Globals.MainPlayerData.Level = 100;
        }


        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Interstitial>();
        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Sounds>();
        builder.RegisterComponentInHierarchy<Camera>();
        builder.RegisterComponentInHierarchy<ScreenSaver>();

        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<OptionsMenu>();
        */

        builder.RegisterComponentInHierarchy<Sounds>();
        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Camera>();
        builder.RegisterComponentInHierarchy<Joystick>();
        builder.RegisterComponentInHierarchy<FPSController>();

        builder.RegisterComponentInHierarchy<GameManager>();

    }
}
