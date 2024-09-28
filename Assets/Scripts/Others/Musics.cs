using System.Collections;
using UnityEngine;
using VContainer;

public class Musics : MonoBehaviour
{
    
    [SerializeField] private AudioClip loop01;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayMusic(MusicTypes _type)
    {
        _audio.pitch = 1;
        _audio.loop = true;
        _audio.volume = 0.3f;

        switch (_type)
        {
            
            case MusicTypes.loop01:                
                _audio.Stop();
                _audio.volume = 0.2f;
                _audio.clip = loop01;
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
        StartCoroutine(playMusicGradient());
    }
    private IEnumerator playMusicGradient()
    {
        //yield return new WaitForSeconds(0.1f);
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
    loop01
}
