using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {        
        if (Globals.MainPlayerData == null)
        {

            Globals.MainPlayerData = new PlayerData();
            Globals.IsMobile = Globals.IsMobileChecker();
            Globals.IsSoundOn = true;
            Globals.IsMusicOn = true;
            Globals.IsInitiated = true;

            Globals.MainPlayerData.T = 0;
            Globals.MainPlayerData.D = 0;

            if (Globals.IsMobile)
            {
                Globals.MainPlayerData.Zoom = 50;
            }
            else
            {
                Globals.MainPlayerData.Zoom = 55;
            }

            //Globals.IsLowFPS = true;

            Globals.Language = Localization.GetInstanse("ru").GetCurrentTranslation();
        }

        builder.RegisterComponentInHierarchy<DayTimeCycle>();
        builder.RegisterComponentInHierarchy<Sounds>();
        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Camera>();
        builder.RegisterComponentInHierarchy<Joystick>();
        builder.RegisterComponentInHierarchy<FPSController>();

        builder.RegisterComponentInHierarchy<AimInformerUI>();

        PlayerControl g = addPlayer(true, Vector3.zero, Vector3.zero, builder).GetComponent<PlayerControl>();
        builder.RegisterComponentInHierarchy<PlayerControl>();
        builder.RegisterComponentInHierarchy<FOVControl>();
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<CameraControl>();
        builder.RegisterComponentInHierarchy<InputControl>();

        //UI        
        builder.RegisterComponentInHierarchy<GameplayUI>();        
        builder.RegisterComponentInHierarchy<ScreenCenterCursor>();

        //Env
        builder.RegisterComponentInHierarchy<WorldGenerator>();
        builder.RegisterComponentInHierarchy<AssetManager>();
        builder.RegisterComponentInHierarchy<NatureGenerator>();
        builder.RegisterComponentInHierarchy<TerrainGenerator>();
        
        

    }

    private GameObject addPlayer(bool isMain, Vector3 pos, Vector3 rot, IContainerBuilder builder)
    {
        //main template
        GameObject g = Instantiate(Resources.Load<GameObject>("main player"));
        g.transform.parent = GameObject.Find("MainPlayer").transform;
        g.transform.position = pos;
        g.transform.eulerAngles = rot;
        g.AddComponent<PlayerControl>();

        //vfx
        //GameObject vfx = Instantiate(Resources.Load<GameObject>("player vfx"), g.transform);
        //vfx.transform.localPosition = Vector3.zero;
        //vfx.transform.localEulerAngles = Vector3.zero;
        //g.GetComponent<PlayerControl>().SetEffectControl(vfx.GetComponent<EffectsControl>());

        //player
        GameObject skin = Instantiate(Resources.Load<GameObject>("skin"), g.transform);
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;


        g.AddComponent<AudioListener>();

        g.SetActive(true);

        //aimer
        if (Globals.IsMobile)
        {
            GameObject aimer = Instantiate(Resources.Load<GameObject>("MobileAimBox"));
            aimer.SetActive(true);

            aimer.transform.parent = g.transform;
            aimer.transform.localPosition = new Vector3(0, 2.5f, 0);

            builder.RegisterComponentInHierarchy<AimerForMobile>();
        }

        return g;
    }
}
