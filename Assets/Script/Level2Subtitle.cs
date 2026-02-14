using UnityEngine;
using VoiceSystem;

public class Level2Subtitle : InteractionSubtitlePrefab
{
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        FindAnyObjectByType<Level2Man>().Finded();
    }
}