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

        builder.RegisterComponentInHierarchy<Sounds>();
        builder.RegisterComponentInHierarchy<Musics>();
        builder.RegisterComponentInHierarchy<Camera>();
        builder.RegisterComponentInHierarchy<Joystick>();
        builder.RegisterComponentInHierarchy<FPSController>();
        
        PlayerControl g = addPlayer(true, Vector3.zero, Vector3.zero).GetComponent<PlayerControl>();
        builder.RegisterComponentInHierarchy<PlayerControl>();
        builder.RegisterComponentInHierarchy<FOVControl>();
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<CameraControl>();
        builder.RegisterComponentInHierarchy<InputControl>();
        builder.RegisterComponentInHierarchy<ScreenCenterCursor>();

        builder.RegisterComponentInHierarchy<WorldGenerator>();
        builder.RegisterComponentInHierarchy<AssetManager>();
        builder.RegisterComponentInHierarchy<NatureGenerator>();
        builder.RegisterComponentInHierarchy<TerrainGenerator>();
        
        

    }

    private GameObject addPlayer(bool isMain, Vector3 pos, Vector3 rot)
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

        return g;
    }
}
