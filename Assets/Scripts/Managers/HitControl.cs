using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitControl : MonoBehaviour
{
    public static void MakeHit(Transform player)
    {
        GameObject hitter = Instantiate(Resources.Load<GameObject>("HitBox"));
        hitter.transform.position = player.transform.position + player.transform.forward + Vector3.up*1.5f;
        hitter.transform.rotation = player.transform.rotation;

        EndHit(hitter).Forget();
    }
    private static async UniTaskVoid EndHit(GameObject hitter)
    {
        await UniTask.Delay(10000);
        Destroy(hitter);
    }
}
