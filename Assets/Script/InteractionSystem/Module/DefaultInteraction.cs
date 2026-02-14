using InteractionSystem.Interface;
using UnityEngine;

namespace InteractionSystem.Module
{
    [System.Serializable]
    public class DefaultInteraction : InteractionModule
    {
        public override bool CanHandle(IInteractable target) => true;

        public override void Execute(IInteractable target, GameObject actor)
        {
            target.Interact(actor);
        }
    }
}
