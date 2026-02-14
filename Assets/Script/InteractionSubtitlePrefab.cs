using InteractionSystem.Interface;
using InteractionSystem.Struct;
using UnityEngine;
using VoiceSystem;

public class InteractionSubtitlePrefab : ConnectionLostInteractionPrefab
{
    [SerializeField] private Subtitle[] subtitles;

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        VoiceManager.Instance.PlaySequence(subtitles);
    }
}