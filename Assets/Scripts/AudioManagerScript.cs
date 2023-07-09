using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerScript : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundMusic;
    public AudioClip ballHit;
    public AudioClip powerUpHit;
    public AudioClip paddleHit;

    private void Start()
    {
        _musicSource.clip = _backgroundMusic;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        _sfxSource.PlayOneShot(audioClip);
    }

}
