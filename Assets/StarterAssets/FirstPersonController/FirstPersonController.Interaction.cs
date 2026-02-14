using System.Linq;
using InteractionSystem.Interface;
using UnityEngine.InputSystem;
using UnityEngine;

namespace StarterAssets
{
    public partial class FirstPersonController
    {
        private void OnInteractionUI(InputAction.CallbackContext context)
        {
            if (GameInstance.Instance == null) return;

            bool isUIActive = !GameInstance.Instance.IsInteractionUIActive();
            GameInstance.Instance.SetInteractionUI(isUIActive);

            float speedMultiplier = isUIActive ? 0.1f : 1.0f;

            MoveSpeed = _defaultMoveSpeed * speedMultiplier;
            SprintSpeed = _defaultSprintSpeed * speedMultiplier;
            GameObject[] objects = GetallInteractionItem();
            foreach (GameObject o in objects)
            {
                IInteractable interfaceInteractable = o.GetComponent<IInteractable>();
                if (interfaceInteractable == null) continue;
                if (!interfaceInteractable.Data.IsActive) continue;
                MeshRenderer meshRenderer = o.GetComponent<MeshRenderer>();
                if (!meshRenderer) continue;
                Material material = meshRenderer.material;
                if (GameInstance.Instance.IsInteractionUIActive())
                    material.EnableKeyword("_EMISSION");
                else
                    material.DisableKeyword("_EMISSION");
            }
        }

        private GameObject[] GetallInteractionItem()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IInteractable>()
                .Select(interactable => (interactable as MonoBehaviour)?.gameObject)
                .ToArray();
        }
    }
}