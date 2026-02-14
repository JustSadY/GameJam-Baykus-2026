using UnityEngine;
using System.Collections.Generic;
using InteractionSystem.Interface;
using System;

namespace InteractionSystem.UI
{
    [Serializable]
    public abstract class InteractionUIController
    {
        protected GameObject ownerGameObject;
        protected LayerMask interactionLayer;
        protected Camera cachedCamera;
        protected Transform uiParent;

        [Header("Detection Settings")] [SerializeField]
        protected float detectionRadius = 5f;

        [Header("UI Settings")] [SerializeField]
        protected GameObject uiPrefab;

        [SerializeField] protected Vector3 uiOffset = new Vector3(0, 1.2f, 0);

        protected Dictionary<IInteractable, GameObject> activeUIs = new Dictionary<IInteractable, GameObject>();
        protected HashSet<IInteractable> foundThisFrame = new HashSet<IInteractable>();
        protected List<IInteractable> toRemove = new List<IInteractable>();

        private IInteractable highlightedInteractable;

        public IReadOnlyCollection<IInteractable> InRangeInteractables => foundThisFrame;

        public virtual void Initialize(GameObject owner, LayerMask layer)
        {
            ownerGameObject = owner;
            interactionLayer = layer;
            cachedCamera = Camera.main;

            uiParent = new GameObject("InteractionUIs").transform;
        }

        public abstract void UpdateTick();

        public virtual void Cleanup()
        {
            foreach (var pair in activeUIs)
            {
                if (pair.Value != null)
                    GameObject.Destroy(pair.Value);
            }
            activeUIs.Clear();

            if (uiParent != null)
                GameObject.Destroy(uiParent.gameObject);
        }

        public void SetHighlighted(IInteractable interactable)
        {
            if (highlightedInteractable == interactable) return;

            if (highlightedInteractable != null && activeUIs.TryGetValue(highlightedInteractable, out var prevUI))
            {
                var prevComp = prevUI.GetComponent<InteractionUI>();
                if (prevComp != null) prevComp.SetHighlighted(false);
            }

            highlightedInteractable = interactable;

            if (interactable != null && activeUIs.TryGetValue(interactable, out var newUI))
            {
                var newComp = newUI.GetComponent<InteractionUI>();
                if (newComp != null) newComp.SetHighlighted(true);
            }
        }

        public void SetProgress(IInteractable interactable, float progress)
        {
            if (interactable != null && activeUIs.TryGetValue(interactable, out var ui))
            {
                var comp = ui.GetComponent<InteractionUI>();
                if (comp != null) comp.SetProgress(progress);
            }
        }

        protected virtual void RefreshUI(IEnumerable<IInteractable> detectedInteractables)
        {
            foundThisFrame.Clear();
            foreach (var interactable in detectedInteractables)
            {
                foundThisFrame.Add(interactable);
                if (!activeUIs.ContainsKey(interactable))
                {
                    if (interactable is MonoBehaviour mono)
                    {
                        GameObject ui = GameObject.Instantiate(uiPrefab, mono.transform.position + uiOffset,
                            Quaternion.identity, uiParent);
                        activeUIs.Add(interactable, ui);

                        var interactionUI = ui.GetComponent<InteractionUI>();
                        if (interactionUI != null)
                            interactionUI.SetData(interactable.Data);
                    }
                }
            }

            toRemove.Clear();
            foreach (var pair in activeUIs)
            {
                if (!foundThisFrame.Contains(pair.Key))
                {
                    GameObject.Destroy(pair.Value);
                    toRemove.Add(pair.Key);
                }
                else if (pair.Key is MonoBehaviour mono)
                {
                    pair.Value.transform.position = mono.transform.position + uiOffset;
                    if (cachedCamera != null)
                    {
                        pair.Value.transform.LookAt(cachedCamera.transform);
                        pair.Value.transform.Rotate(0, 180, 0);
                    }
                }
            }

            foreach (var key in toRemove) activeUIs.Remove(key);
        }
    }
}
