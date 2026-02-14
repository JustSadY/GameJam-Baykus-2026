using InteractionSystem.Interface;
using InteractionSystem.Struct;
using Unity.VisualScripting;
using UnityEngine;

namespace VoiceSystem
{
    public class ConnectionLostInteractionPrefab : MonoBehaviour, IConditionalInteractable
    {
        
        public InteractionData Data { get; private set; }

        private void Awake()
        {
            Data = new InteractionData
            {
                IsActive = true
            };
        }


        public virtual void Interact(GameObject interactor)
        {
            Data.IsActive = false;
            MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
            if (!meshRenderer) return;
            Material material = meshRenderer.material;
            material.DisableKeyword("_EMISSION");
        }


        public virtual bool CanInteract(GameObject interactor)
        {
            return Data.IsActive;
        }
    }
}