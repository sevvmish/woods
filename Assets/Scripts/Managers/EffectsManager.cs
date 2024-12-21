using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private CameraControl cameraControl;

    [SerializeField] private GameObject punchSwing;
    [SerializeField] private GameObject punchImpactBlunt;
    [SerializeField] private GameObject chopWoodEffect;
    [SerializeField] private GameObject mineStoneEffect;
    [SerializeField] private GameObject stoneDestroyed;
    [SerializeField] private GameObject treeDestroyed;
    [SerializeField] private GameObject collectResource;
    [SerializeField] private GameObject navMeshOnPlace;

    public CameraControl GetCameraControl { get => cameraControl; }

    public ObjectPool PunchSwingPool { get => punchSwingPool; }
    private ObjectPool punchSwingPool;

    public ObjectPool PunchImpactBluntPool { get => punchImpactBluntPool; }
    private ObjectPool punchImpactBluntPool;

    public ObjectPool ChopWoodEffectPool { get => chopWoodEffectPool; }
    private ObjectPool chopWoodEffectPool;

    public ObjectPool MineStoneEffectPool { get => mineStoneEffectPool; }
    private ObjectPool mineStoneEffectPool;

    public ObjectPool StoneDestroyedPool { get => stoneDestroyedPool; }
    private ObjectPool stoneDestroyedPool;

    public ObjectPool TreeDestroyedPool { get => treeDestroyedPool; }
    private ObjectPool treeDestroyedPool;

    public ObjectPool CollectResourcePool { get => collectResourcePool; }
    private ObjectPool collectResourcePool;

    public ObjectPool NavMeshOnPlacePool { get => navMeshOnPlacePool; }
    private ObjectPool navMeshOnPlacePool;


    // Start is called before the first frame update
    void Start()
    {
        punchSwing.SetActive(false);
        punchSwingPool = new ObjectPool(2, punchSwing, transform);

        punchImpactBlunt.SetActive(false);
        punchImpactBluntPool = new ObjectPool(2, punchImpactBlunt, transform);

        chopWoodEffect.SetActive(false);
        chopWoodEffectPool = new ObjectPool(2, chopWoodEffect, transform);

        mineStoneEffect.SetActive(false);
        mineStoneEffectPool = new ObjectPool(2, mineStoneEffect, transform);

        stoneDestroyed.SetActive(false);
        stoneDestroyedPool = new ObjectPool(2, stoneDestroyed, transform);

        treeDestroyed.SetActive(false);
        treeDestroyedPool = new ObjectPool(2, treeDestroyed, transform);

        collectResource.SetActive(false);
        collectResourcePool = new ObjectPool(2, collectResource, transform);

        navMeshOnPlace.SetActive(false);
        navMeshOnPlacePool = new ObjectPool(10, navMeshOnPlace, transform);
    }



    public void PlayEffectAtLocation(ObjectPool poolEffect, Vector3 pos, float _timer)
    {
        playEffectAtLocation(0, poolEffect, pos, Vector3.zero, Vector3.one, _timer).Forget();
    }
    public void PlayEffectAtLocation(float delay, ObjectPool poolEffect, Vector3 pos, Vector3 rot, Vector3 scale, float _timer)
    {
        playEffectAtLocation(delay, poolEffect, pos, rot, scale, _timer).Forget();
    }
    private async UniTaskVoid playEffectAtLocation(float delay, ObjectPool poolEffect, Vector3 pos, Vector3 rot, Vector3 scale, float _timer)
    {
        await UniTask.Delay((int)(delay * 1000));

        GameObject g = poolEffect.GetObject();
        g.transform.position = pos;
        g.transform.eulerAngles = rot;
        g.transform.localScale = scale;
        g.SetActive(true);

        await UniTask.Delay((int)(_timer * 1000));

        poolEffect.ReturnObject(g);
    }

    
    
}
