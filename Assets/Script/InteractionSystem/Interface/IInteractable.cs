using InteractionSystem.Struct;
using UnityEngine;

namespace InteractionSystem.Interface
{
    public interface IInteractable
    {
        InteractionData Data { get; }
        public void Interact(GameObject interactor);
    }
}