using UnityEngine;

namespace InteractionSystem.Interface
{
    public interface IConditionalInteractable : IInteractable
    {
        bool CanInteract(GameObject interactor);
    }
}
