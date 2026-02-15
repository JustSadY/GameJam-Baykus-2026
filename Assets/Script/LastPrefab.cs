using UnityEngine;

public class LastPrefab : InteractionSubtitlePrefab
{
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        Debug.Log(interactor.name);
        FindAnyObjectByType<LastPhone>().add();
    }
}