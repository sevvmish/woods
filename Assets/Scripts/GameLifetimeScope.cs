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

            Globals.MainPlayerData.Inv = new int[32, 2] { { 2, 1 }, {5, 1 }, { 1, 10 }, { 0, 0 }, { 6, 1 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, {  0, 0 }, { 0, 0 }, {  0, 0 }, {  0, 0 }, { 0, 0 }, {  0, 0 }, {  0, 0 }, { 0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, { 0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, {  0, 0 }, };
            Globals.MainPlayerData.Equip = new int[] { 0 };
            Globals.MainPlayerData.Dur = new int[3,2] { {0,100 }, {1,100 }, {4,100 } };

            Globals.Language = Localization.GetInstanse("ru").GetCurrentTranslation();
        }
                
        builder.RegisterComponentInHierarchy<Sounds>();
        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Camera>();

        builder.RegisterComponentInHierarchy<GameplayInformerUI>();

        builder.RegisterComponentInHierarchy<AssetManager>();
        builder.RegisterComponentInHierarchy<ItemManager>();

        builder.RegisterComponentInHierarchy<InventoryInformerUI>();

        builder.RegisterComponentInHierarchy<EffectsManager>();
        builder.RegisterComponentInHierarchy<Joystick>();        
        builder.RegisterComponentInHierarchy<FPSController>();

        builder.RegisterComponentInHierarchy<AimInformerUI>();
        builder.RegisterComponentInHierarchy<CharacterPanelUI>();
        builder.RegisterComponentInHierarchy<ShowDPSUI>();
        

        PlayerControl g = addPlayer(true, Vector3.zero, Vector3.zero, builder).GetComponent<PlayerControl>();
        builder.RegisterComponentInHierarchy<Inventory>();
        builder.RegisterComponentInHierarchy<ItemActivation>();
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
        g.AddComponent<ItemActivation>();

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
