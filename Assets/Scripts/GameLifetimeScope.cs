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

            Globals.MainPlayerData.T = 50000;
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

            Globals.MainPlayerData.Inv = new int[32, 3] { { 0, 2, 1 }, {1, 5, 1 }, { 2, 1, 10 }, { 3, 0, 0 }, { 4, 6, 1 }, { 5, 0, 0 }, { 6, 0, 0 }, { 7, 0, 0 }, { 8, 0, 0 }, { 9, 0, 0 }, { 10, 0, 0 }, { 11, 0, 0 }, { 12, 0, 0 }, { 13, 0, 0 }, { 14, 0, 0 }, { 15, 0, 0 }, { 16, 0, 0 }, { 17, 0, 0 }, { 18, 0, 0 }, { 19, 0, 0 }, { 20, 0, 0 }, { 21, 0, 0 }, { 22, 0, 0 }, { 23, 0, 0 }, { 24, 0, 0 }, { 25, 0, 0 }, { 26, 0, 0 }, { 27, 0, 0 }, { 28, 0, 0 }, { 29, 0, 0 }, { 30, 0, 0 }, { 31, 0, 0 }, };

            Globals.Language = Localization.GetInstanse("ru").GetCurrentTranslation();
        }
                
        builder.RegisterComponentInHierarchy<Sounds>();
        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Camera>();

        builder.RegisterComponentInHierarchy<AssetManager>();
        builder.RegisterComponentInHierarchy<ItemManager>();

        builder.RegisterComponentInHierarchy<EffectsManager>();
        builder.RegisterComponentInHierarchy<Joystick>();        
        builder.RegisterComponentInHierarchy<FPSController>();

        builder.RegisterComponentInHierarchy<AimInformerUI>();
        builder.RegisterComponentInHierarchy<CharacterPanelUI>();
        builder.RegisterComponentInHierarchy<ShowDPSUI>();
        

        PlayerControl g = addPlayer(true, Vector3.zero, Vector3.zero, builder).GetComponent<PlayerControl>();
        builder.RegisterComponentInHierarchy<Inventory>();
        builder.RegisterComponentInHierarchy<PlayerControl>();        
        builder.RegisterComponentInHierarchy<FOVControl>();
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<CameraControl>();
        
        builder.RegisterComponentInHierarchy<HitControl>();

        //UI        
        builder.RegisterComponentInHierarchy<GameplayUI>();
        builder.RegisterComponentInHierarchy<InventoryUI>();
        builder.RegisterComponentInHierarchy<QuickBarUI>();
        builder.RegisterComponentInHierarchy<ScreenCenterCursor>();

        //Env
        builder.RegisterComponentInHierarchy<WorldGenerator>();        
        builder.RegisterComponentInHierarchy<NatureGenerator>();
        builder.RegisterComponentInHierarchy<TerrainGenerator>();

        builder.RegisterComponentInHierarchy<DayTimeCycle>();
        builder.RegisterComponentInHierarchy<InputControl>();

    }

    private GameObject addPlayer(bool isMain, Vector3 pos, Vector3 rot, IContainerBuilder builder)
    {
        //main template
        GameObject g = Instantiate(Resources.Load<GameObject>("main player"));
        g.transform.parent = GameObject.Find("MainPlayer").transform;
        g.transform.position = pos;
        g.transform.eulerAngles = rot;

        g.AddComponent<EquipControl>();
        builder.RegisterComponentInHierarchy<EquipControl>();

        g.AddComponent<PlayerControl>();
        g.AddComponent<ActionControl>();
        g.AddComponent<Inventory>();


        //vfx
        //GameObject vfx = Instantiate(Resources.Load<GameObject>("player vfx"), g.transform);
        //vfx.transform.localPosition = Vector3.zero;
        //vfx.transform.localEulerAngles = Vector3.zero;
        //g.GetComponent<PlayerControl>().SetEffectControl(vfx.GetComponent<EffectsControl>());

        //player
        GameObject skin = Instantiate(Resources.Load<GameObject>("SkinMale"), g.transform);
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;


        g.AddComponent<AudioListener>();

        g.SetActive(true);

        //aimer for mobile
        if (Globals.IsMobile)
        {
            GameObject aimer = Instantiate(Resources.Load<GameObject>("MobileAimBox"));
            aimer.SetActive(true);

            aimer.transform.parent = g.transform;
            aimer.transform.localPosition = new Vector3(0, 2.5f, 2);

            builder.RegisterComponentInHierarchy<AimerForMobile>();
        }

        

        return g;
    }
}
