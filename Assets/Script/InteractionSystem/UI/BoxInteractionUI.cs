using UnityEngine;
using System.Collections.Generic;
using InteractionSystem.Interface;
using System;

namespace InteractionSystem.UI
{
    [Serializable]
    public class BoxInteractionUI : InteractionUIController
    {
        [SerializeField] private Vector3 boxSize = Vector3.one;
        private Collider[] results = new Collider[16];

        public override void UpdateTick()
        {
            if (ownerGameObject == null || uiPrefab == null) return;

            int numFound = Physics.OverlapBoxNonAlloc(ownerGameObject.transform.position, boxSize / 2, results,
                ownerGameObject.transform.rotation, interactionLayer);

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