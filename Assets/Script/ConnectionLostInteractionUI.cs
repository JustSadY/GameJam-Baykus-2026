using System;
using System.Linq;
using InteractionSystem.Interface;
using InteractionSystem.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VoiceSystem
{
    public class ConnectionLostInteractionUI : InteractionUIController
    {
        public override void Initialize(GameObject owner, LayerMask layer)
        {
            base.Initialize(owner, layer);
        }

        public override void UpdateTick()
        {
            if (!GameInstance.Instance.isActiveAndEnabled) return;
            GameObject[] gameObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IInteractable>()
                .Select(interactable => (interactable as MonoBehaviour)?.gameObject).ToArray();
            foreach (GameObject o in gameObjects)
            {
                // if (Vector3.Distance(ownerGameObject.transform.position, o.transform.position) < detectionRadius) { }
            }
        }
    }
}