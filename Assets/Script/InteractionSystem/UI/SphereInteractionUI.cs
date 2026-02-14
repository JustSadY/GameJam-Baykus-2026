using UnityEngine;
using System.Collections.Generic;
using InteractionSystem.Interface;
using System;

namespace InteractionSystem.UI
{
    [Serializable]
    public class SphereInteractionUI : InteractionUIController
    {
        private Collider[] results = new Collider[16];

        public override void UpdateTick()
        {
            if (ownerGameObject == null || uiPrefab == null) return;

            int numFound = Physics.OverlapSphereNonAlloc(ownerGameObject.transform.position, detectionRadius, results,
                interactionLayer);

            if (numFound == results.Length)
                results = new Collider[results.Length * 2];

            List<IInteractable> foundList = new List<IInteractable>();
            for (int i = 0; i < numFound; i++)
            {
                if (results[i].TryGetComponent(out IInteractable interactable))
                    foundList.Add(interactable);
            }

            RefreshUI(foundList);
        }
    }
}