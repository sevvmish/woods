using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsfx : MonoBehaviour
{
    [Header("audio")]
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip attentionSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip getHitSound;
    [SerializeField] private AudioClip deathSound;

    public void PlayIdle()
    {
        playSound(idleSound);
    }

    public void PlayAttack()
    {
        playSound(attackSound);
    }

    public void PlayAttention()
    {
        playSound(attentionSound);
    }

    public void PlayGetHit()
    {
        playSound(getHitSound);
    }

    public void PlayDeath()
    {
        playSound(deathSound);
    }

    private void playSound(AudioClip clip)
    {
        if (clip == null) return;
        if (_audio.isPlaying) return;

        _audio.Stop();
        _audio.clip = clip;
        _audio.Play();
    }
}
