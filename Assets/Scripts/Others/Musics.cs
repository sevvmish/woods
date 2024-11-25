using System.Collections;
using UnityEngine;
using VContainer;

public class Musics : MonoBehaviour
{
    [Inject] private DayTimeCycle cycle;

    [SerializeField] private AudioClip ForestDay;
    [SerializeField] private AudioClip ForestNight;

    private AudioSource _audio;
    private bool isAmbient;
    private bool isDay;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isAmbient)
        {
            if ((cycle.CurrentHour() > 21 || cycle.CurrentHour() < 6) && isDay)
            {
                PlayMusic(MusicTypes.forestNight);
                StartCoroutine(playMusicGradient());
                isDay = false;
            }
            else if(cycle.CurrentHour() <= 21 && cycle.CurrentHour() >= 6 && !isDay)
            {
                isDay = true;
                StartCoroutine(playMusicGradient());
                PlayMusic(MusicTypes.forestDay);
            }
        }
    }

    public void PlayMusic(MusicTypes _type)
    {
        _audio.pitch = 1;
        _audio.loop = true;
        _audio.volume = 0.3f;

        switch (_type)
        {
            
            case MusicTypes.forestDay:                
                _audio.Stop();
                _audio.pitch = 0.8f;
                _audio.volume = 0.35f;
                _audio.clip = ForestDay;
                _audio.Play();
                break;

            case MusicTypes.forestNight:
                _audio.Stop();
                _audio.pitch = 0.8f;
                _audio.volume = 0.5f;
                _audio.clip = ForestNight;
                _audio.Play();
                break;

        }
    }

    public void StopAll()
    {
        _audio.Stop();
    }

    public void StartMusic()
    {
        isAmbient = true;
        isDay = true;
        StartCoroutine(playMusicGradient());
        PlayMusic(MusicTypes.forestDay);
        /*
        MusicTypes rnd = (MusicTypes)UnityEngine.Random.Range(0, 1);

        for (int i = 0; i < 10; i++)
        {
            if (rnd == Globals.LastPlayedMelody)
            {
                rnd = (MusicTypes)UnityEngine.Random.Range(0, 1);
            }
            else
            {
                break;
            }
        }

        Globals.LastPlayedMelody = rnd;
        PlayMusic(rnd);
        StartCoroutine(playMusicGradient());*/
    }
    
    private IEnumerator playMusicGradient()
    {
        
        float volume = _audio.volume;
        float delta = volume / 10f;
                
        _audio.volume = 0;

        for (int i = 0; i < 10; i++)
        {
            _audio.volume += delta;
            yield return new WaitForSeconds(0.15f);
        }
    }
    
}

public enum MusicTypes
{
    forestDay,
    forestNight
}
