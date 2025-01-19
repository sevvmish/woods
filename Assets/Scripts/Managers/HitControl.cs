using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class HitControl : MonoBehaviour
{
    [Inject] private EffectsManager effects;
    [Inject] private ShowDPSUI showDPS;
    [Inject] private Camera _camera;
    [Inject] private Inventory inventory;

    public void MakeHit(IInteractable source, Transform player, Vector3 Correction, Item weapon)
    {
        GameObject hitter = Instantiate(Resources.Load<GameObject>("HitBox"));
        hitter.GetComponent<HitBox>().SetHitBox(source, effects, showDPS, weapon, player);
        hitter.transform.localScale = new Vector3(2, 1.5f, 2f);
        
        hitter.transform.position = player.transform.position + player.transform.forward + Correction;
        hitter.transform.rotation = player.transform.rotation;

        EndHit(hitter).Forget();
        inventory.RightHandEquipDurability?.Invoke();
    }
    private async UniTaskVoid EndHit(GameObject hitter)
    {
        await UniTask.Delay(100);
        Destroy(hitter);
    }
}
