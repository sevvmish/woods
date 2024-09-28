using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSaver : MonoBehaviour
{
    [SerializeField] private Transform[] screen;
    [SerializeField] private TextMeshProUGUI loadingText;

    
    private void Awake()
    {
        screen.ToList().ForEach(s => s.gameObject.SetActive(true));
        screen.ToList().ForEach(s => s.localScale = Vector3.one);
        loadingText.gameObject.SetActive(false);
        /*
        if (Globals.Language != null)
        {
            loadingText.gameObject.SetActive(true);
            loadingText.text = Globals.Language.Loading;
        }*/

    }

    public void HideScreen()
    {
        screen.ToList().ForEach(s => s.gameObject.SetActive(true));
        screen.ToList().ForEach(s => s.localScale = Vector3.one);
        screen.ToList().ForEach(s => s.DOScale(Vector3.zero, Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear));        
        loadingText.gameObject.SetActive(false);
    }

    
    public void ShowScreen(bool isLoading, bool isFast)
    {
        screen.ToList().ForEach(s => s.gameObject.SetActive(true));
        screen.ToList().ForEach(s => s.localScale = Vector3.zero);

        float _timer = Globals.SCREEN_SAVER_AWAIT;
        if (isFast)
        {
            _timer = 0;
        }

        if (isLoading)
        {
            //StartCoroutine(playSign());
            screen.ToList().ForEach(s => s.DOScale(Vector3.one, _timer).SetEase(Ease.Linear));
        }
        else
        {
            screen.ToList().ForEach(s => s.DOScale(Vector3.one, _timer).SetEase(Ease.Linear));
        }
        
    }
    


}
