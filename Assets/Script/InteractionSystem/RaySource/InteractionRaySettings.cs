using UnityEngine;
using InteractionSystem.Interface;

namespace InteractionSystem.RaySource
{
    [System.Serializable]
    public abstract class InteractionRaySettings
    {
        public abstract bool TryGetInteractable(GameObject actor, LayerMask layer, out IInteractable interactable);
    }
}