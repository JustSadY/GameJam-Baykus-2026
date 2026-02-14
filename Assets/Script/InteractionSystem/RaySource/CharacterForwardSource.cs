using UnityEngine;
using InteractionSystem.Interface;

namespace InteractionSystem.RaySource
{
    [System.Serializable]
    public class CharacterForwardSource : InteractionRaySettings
    {
        [SerializeField] private string socketName = "Head";
        [SerializeField] private float range = 3;

        public override bool TryGetInteractable(GameObject actor, LayerMask layer,
            out IInteractable interactable)
        {
            interactable = null;
            Ray ray;

            Transform socket = FindDeepChild(actor.transform, socketName);

            if (socket != null)
            {
                ray = new Ray(socket.position, socket.forward);
            }
            else
            {
                ray = new Ray(actor.transform.position, actor.transform.forward);
            }

            Debug.DrawRay(ray.origin, ray.direction * range, Color.green, 2f);

            if (Physics.Raycast(ray, out RaycastHit hit, range, layer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject == actor) return false;
                return hit.collider.TryGetComponent(out interactable);
            }

            return false;
        }

        private Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name) return child;
                var result = FindDeepChild(child, name);
                if (result != null) return result;
            }

            return null;
        }
    }
}