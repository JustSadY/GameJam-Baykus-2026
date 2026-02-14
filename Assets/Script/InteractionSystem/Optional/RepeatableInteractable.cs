using System.Collections;
using InteractionSystem.Interface;
using InteractionSystem.Struct;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem.Optional
{
    public class RepeatableInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractionData data;
        [SerializeField] private float respawnTime = 5f;
        [SerializeField] private UnityEvent<GameObject> onInteract;

        private Collider _collider;

        public InteractionData Data => data;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void Interact(GameObject interactor)
        {
            onInteract?.Invoke(interactor);
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            if (_collider) _collider.enabled = false;
            yield return new WaitForSeconds(respawnTime);
            if (_collider) _collider.enabled = true;
        }
    }
}
