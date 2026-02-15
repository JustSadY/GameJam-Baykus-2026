using System;
using InteractionSystem.Struct;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LastPhone : InteractionSubtitlePrefab
{
    public int x;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void Start()
    {
        Data = new InteractionData
        {
            IsActive = false
        };
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _audioClip;
    }

    public void add()
    {
        x++;
        if (x >= 1)
        {
            _audioSource.Play();
            Data.IsActive = true;
        }
    }

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        FindAnyObjectByType<LastDoor>().ActivatedDoor();
        _audioSource.Stop();
    }


    public override bool CanInteract(GameObject interactor)
    {
        Boolean b = base.CanInteract(interactor);
        Boolean y = x >= 1;
        return b && y;
    }
}