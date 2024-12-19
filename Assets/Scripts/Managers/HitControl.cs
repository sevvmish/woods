using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class HitControl : MonoBehaviour
{
    [Inject] private EffectsManager effects;
    [Inject] private ShowDPSUI showDPS;

    public void MakeHit(Transform player, Vector3 UPCorrection, Item weapon)
    {
        GameObject hitter = Instantiate(Resources.Load<GameObject>("HitBox"));
        hitter.GetComponent<HitBox>().SetHitBox(effects, showDPS, weapon, player);
        hitter.transform.localScale = new Vector3(1, 1, 1.5f);
        hitter.transform.position = player.transform.position + player.transform.forward + UPCorrection;
        hitter.transform.rotation = player.transform.rotation;

        EndHit(hitter).Forget();
    }
    private async UniTaskVoid EndHit(GameObject hitter)
    {
        await UniTask.Delay(100);
        Destroy(hitter);
    }
}
