using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject punchSwing;
    [SerializeField] private GameObject punchImpactBlunt;

    public ObjectPool PunchSwingPool { get => punchSwingPool; }
    private ObjectPool punchSwingPool;

    public ObjectPool PunchImpactBluntPool { get => punchImpactBluntPool; }
    private ObjectPool punchImpactBluntPool;

    // Start is called before the first frame update
    void Start()
    {
        punchSwing.SetActive(false);
        punchSwingPool = new ObjectPool(2, punchSwing, transform);

        punchImpactBlunt.SetActive(false);
        punchImpactBluntPool = new ObjectPool(2, punchImpactBlunt, transform);
    }


    public void PlayEffectAtLocation(ObjectPool poolEffect, Vector3 pos, float _timer)
    {
        playEffectAtLocation(poolEffect, pos, _timer).Forget();
    }
    private async UniTaskVoid playEffectAtLocation(ObjectPool poolEffect, Vector3 pos, float _timer)
    {
        GameObject g = poolEffect.GetObject();
        g.transform.position = pos;
        g.transform.localScale = Vector3.one;
        g.SetActive(true);

        await UniTask.Delay((int)(_timer * 1000));

        poolEffect.ReturnObject(g);
    }
}