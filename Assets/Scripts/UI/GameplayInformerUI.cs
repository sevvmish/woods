using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using VContainer;

public class GameplayInformerUI : MonoBehaviour
{
    [Inject] private Sounds sounds;

    private TextMeshProUGUI mainTexter;
    private string currentData = "";
    private CancellationTokenSource cts;

    private void Awake()
    {
        mainTexter = GetComponent<TextMeshProUGUI>();
        cts = new CancellationTokenSource();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            YouNeedAxe();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            YouNeedPickaxe();
        }
    }

    public void YouNeedAxe()
    {
        sounds.PlaySound(SoundTypes.beepTick);
        ShowText(Globals.Language.YouNeedAxe);
    }
        
    public void YouNeedPickaxe()
    {
        sounds.PlaySound(SoundTypes.beepTick);
        ShowText(Globals.Language.YouNeedPickaxe);
    }
        
    public void YouNeedBetterAxe()
    {
        sounds.PlaySound(SoundTypes.beepTick);
        ShowText(Globals.Language.YouNeedBetterAxe);
    }
        
    public void YouNeedBetterPickAxe()
    {
        sounds.PlaySound(SoundTypes.beepTick);
        ShowText(Globals.Language.YouNeedBetterPickaxe);
    }
        


    public void ShowText(string data)
    {
        if (data == currentData) return;
        currentData = data;
        mainTexter.text = currentData;
        cts.Cancel();
        cts = new CancellationTokenSource();
        playText(cts.Token).Forget();
    }
    private async UniTaskVoid playText(CancellationToken token)
    {        
        mainTexter.transform.localScale = Vector3.zero;
        mainTexter.color = new Color(1, 1, 0, 1);
        mainTexter.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);

        for (int i = 0; i < 3; i++)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            await UniTask.Delay(100);

        }

        for (int i = 0; i < 30; i++)
        {
            if (token.IsCancellationRequested)
            {                
                return;
            }

            await UniTask.Delay(100);
                        
        }

        currentData = "";
        mainTexter.DOColor(new Color(0, 0, 0, 0), 1f).SetEase(Ease.Linear);
        for (int i = 0; i < 10; i++)
        {
            if (token.IsCancellationRequested)
            {                
                return;
            }

            await UniTask.Delay(100);

        }

    }
}
