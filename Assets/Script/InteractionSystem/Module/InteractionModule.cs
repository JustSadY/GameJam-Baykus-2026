using InteractionSystem.Interface;
using UnityEngine;

namespace InteractionSystem.Module
{
    [System.Serializable]
    public abstract class InteractionModule
    {
        [SerializeField] private KeyCode key = KeyCode.E;
        [SerializeField] private LayerMask layer = ~0;

        public KeyCode Key => key;
        public LayerMask Layer => layer;

        public abstract bool CanHandle(IInteractable target);
        public abstract void Execute(IInteractable target, GameObject actor);
    }
}
