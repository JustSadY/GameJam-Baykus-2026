using InteractionSystem.Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem.RaySource
{
    [System.Serializable]
    public class MouseForwardSource : InteractionRaySettings
    {
        [SerializeField] private float range = 100f;

        private Camera _mainCamera;

        public override bool TryGetInteractable(GameObject actor, LayerMask layer, out IInteractable interactable)
        {
            interactable = null;

            if (_mainCamera == null)
                _mainCamera = Camera.main;

            if (_mainCamera == null) return false;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * range, Color.green, 2f);

            if (Physics.Raycast(ray, out RaycastHit hit, range, layer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject == actor) return false;
                return hit.collider.TryGetComponent(out interactable);
            }

            return false;
        }
    }
}