using System;
using UnityEngine;
using VoiceSystem;

[RequireComponent(typeof(AudioSource))]
public class Cat : ConnectionLostInteractionPrefab
{
    AudioSource _audioSource;
    Animator _animator;
    [SerializeField] AudioClip _audioClip;


    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        _animator.SetTrigger("Active");
        Invoke(nameof(Stop), 12f);
    }

    private void Stop()
    {
        _audioSource.Stop();
        this.gameObject.SetActive(false);
    }
}