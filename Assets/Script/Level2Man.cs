using UnityEngine;

public class Level2Man : InteractionSubtitlePrefab
{
    private int _findedObject = 0;

    public void Finded() => _findedObject++;


    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        Glitch glitch = FindAnyObjectByType<Glitch>();
        if (!glitch) return;
        glitch.StartGlitch();
    }

    public override bool CanInteract(GameObject interactor)
    {
        bool isBaseActive = base.CanInteract(interactor);
        bool isactive = isBaseActive && _findedObject >= 3;
        return isactive;
    }
}