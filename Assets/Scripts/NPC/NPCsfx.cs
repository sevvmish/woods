using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsfx : MonoBehaviour
{
    [Header("audio")]
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip attentionSound;

    public void PlayIdle()
    {
        if (_audio.isPlaying) return;

        _audio.Stop();
        _audio.clip = idleSound;
        _audio.Play();
    }

    public void PlayAttention()
    {
        if (_audio.isPlaying) return;

        _audio.Stop();
        _audio.clip = attentionSound;
        _audio.Play();
    }
}
