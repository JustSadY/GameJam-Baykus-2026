using UnityEngine;

public class Head : InteractionSubtitlePrefab
{
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        this.gameObject.SetActive(false);
    }
}