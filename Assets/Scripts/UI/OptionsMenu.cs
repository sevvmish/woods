using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using VContainer;
using YG;

public class OptionsMenu : MonoBehaviour
{
    [Inject] private Sounds sounds;
    [Inject] private Musics musics;
    [Inject] private GameManager gm;
    [Inject] private ScreenSaver screenS;

    [Header("options menu")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button optionsButton;    
    [SerializeField] private Button continueButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    // Start is called before the first frame update
    void Start()
    {
        optionsButton.gameObject.SetActive(true);
        optionsPanel.SetActive(false);


        if (!Globals.IsMobile)
        {
            optionsPanel.transform.localScale = Vector3.one * 1.5f;
        }

        //options
        optionsButton.onClick.AddListener(() =>
        {
            openOptions();
        });


        continueButton.onClick.AddListener(() =>
        {
            continuePlay();
        });

        soundButton.onClick.AddListener(() =>
        {
            if (Globals.IsSoundOn)
            {
                Globals.IsSoundOn = false;
                soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOffSprite;
                AudioListener.volume = 0;
            }
            else
            {
                sounds.PlaySound(SoundTypes.click);
                Globals.IsSoundOn = true;
                soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOnSprite;
                AudioListener.volume = 1f;                
            }

            SaveLoadManager.Save();
        });


        musicButton.onClick.AddListener(() =>
        {
            if (Globals.IsMusicOn)
            {
                Globals.IsMusicOn = false;
                musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOffSprite;
                musics.StopAll();
            }
            else
            {
                sounds.PlaySound(SoundTypes.click);
                Globals.IsMusicOn = true;
                musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOnSprite;
                musics.StartMusic();
            }

            SaveLoadManager.Save();
        });
    }


    private IEnumerator playMainMenu()
    {
        screenS.ShowScreen(true, false);
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene("MainMenu");
    }

    private void continuePlay()
    {
        Globals.IsOptions = false;
        YandexGame.GameplayStart();
        sounds.PlaySound(SoundTypes.click);
        optionsPanel.SetActive(false);
    }



    private void openOptions()
    {
        Globals.IsOptions = true;
        YandexGame.GameplayStop();

        if (Globals.IsSoundOn)
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOnSprite;
        }
        else
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOffSprite;
        }

        if (Globals.IsMusicOn)
        {
            musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOnSprite;
        }
        else
        {
            musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOffSprite;
        }

        sounds.PlaySound(SoundTypes.click);
        optionsPanel.SetActive(true);

        
        continueButton.transform.localScale = Vector3.zero;
        soundButton.transform.localScale = Vector3.zero;
        musicButton.transform.localScale = Vector3.zero;

        continueButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        soundButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        musicButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);

    }


    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Globals.IsOptions)
            {                
                openOptions();
            }
            else
            {
                
                continuePlay();
            }            
        }
    }
}
