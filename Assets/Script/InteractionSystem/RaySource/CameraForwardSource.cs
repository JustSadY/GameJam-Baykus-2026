using UnityEngine;
using InteractionSystem.Interface;

namespace InteractionSystem.RaySource
{
    [System.Serializable]
    public class CameraForwardSource : InteractionRaySettings
    {
        [SerializeField] private float range = 3;

        public override bool TryGetInteractable(GameObject actor, LayerMask layer,
            out IInteractable interactable)
        {
            interactable = null;

            Camera mainCam = Camera.main;
            var ray = mainCam != null
                ? new Ray(mainCam.transform.position, mainCam.transform.forward)
                : new Ray(actor.transform.position, actor.transform.forward);

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