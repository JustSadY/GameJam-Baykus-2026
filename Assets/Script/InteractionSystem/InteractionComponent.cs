using System;
using System.Collections.Generic;
using InteractionSystem.Interface;
using InteractionSystem.Module;
using InteractionSystem.Optional;
using InteractionSystem.RaySource;
using InteractionSystem.Struct;
using InteractionSystem.UI;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    public class InteractionComponent : MonoBehaviour
    {
        [SubclassSelector] [SerializeReference]
        private InteractionRaySettings raySettings;

        [SubclassSelector] [SerializeReference]
        private InteractionUIController interactionUIController;

        [SubclassSelector] [SerializeReference]
        private List<InteractionModule> modules = new List<InteractionModule>();

        private UnityEvent<GameObject> onInteracted;
        private UnityEvent<GameObject> onEnteredRange;
        private UnityEvent<GameObject> onExitedRange;
        private UnityEvent<float> onHoldProgress;

        public event Action<IInteractable> OnInteracted;
        public event Action<IInteractable> OnEnteredRange;
        public event Action<IInteractable> OnExitedRange;
        public event Action<float> OnHoldProgress;

        private bool isEnabled = true;
        private float holdTimer;
        private int activeHoldModuleIndex = -1;
        private IInteractable activeHoldTarget;
        private IInteractable primaryTarget;
        private IInteractable[] moduleTargets;
        private Dictionary<IInteractable, float> cooldowns = new Dictionary<IInteractable, float>();
        private List<IInteractable> expiredCooldowns = new List<IInteractable>();
        private HashSet<IInteractable> previousInRange = new HashSet<IInteractable>();
        private HashSet<IInteractable> tempInRange = new HashSet<IInteractable>();

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public IInteractable PrimaryTarget => primaryTarget;

        private void Awake()
        {
            moduleTargets = new IInteractable[modules.Count];

            LayerMask combinedLayer = 0;
            foreach (var module in modules)
                combinedLayer |= module.Layer;

            interactionUIController?.Initialize(gameObject, combinedLayer);
        }

        private void OnDestroy()
        {
            SetOutline(primaryTarget, false);
            interactionUIController?.Cleanup();
        }

        private void Update()
        {
            if (!isEnabled)
            {
                ResetHold();
                return;
            }

            UpdateCooldowns();
            interactionUIController?.UpdateTick();
            UpdateRangeEvents();
            UpdateModuleTargets();
            HandleInput();
        }

        private void UpdateModuleTargets()
        {
            IInteractable newPrimary = null;

            for (int i = 0; i < modules.Count; i++)
            {
                moduleTargets[i] = null;

                if (raySettings == null) continue;
                if (!raySettings.TryGetInteractable(gameObject, modules[i].Layer, out IInteractable target))
                    continue;
                if (!modules[i].CanHandle(target)) continue;
                if (IsOnCooldown(target)) continue;
                if (target is IConditionalInteractable cond && !cond.CanInteract(gameObject))
                    continue;

                moduleTargets[i] = target;
                if (newPrimary == null)
                    newPrimary = target;
            }

            if (newPrimary != primaryTarget)
            {
                SetOutline(primaryTarget, false);
                ResetHold();
                primaryTarget = newPrimary;
                SetOutline(primaryTarget, true);
            }

            interactionUIController?.SetHighlighted(primaryTarget);
        }

        private void HandleInput()
        {
            for (int i = 0; i < modules.Count; i++)
            {
                var target = moduleTargets[i];
                if (target == null) continue;

                var module = modules[i];
                var data = target.Data;

                if (data.Type == InteractionType.Hold && data.HoldDuration > 0)
                {
                    if (Input.GetKey(module.Key))
                    {
                        if (activeHoldModuleIndex != i || activeHoldTarget != target)
                        {
                            ResetHold();
                            activeHoldModuleIndex = i;
                            activeHoldTarget = target;
                        }

                        holdTimer += Time.deltaTime;
                        float progress = Mathf.Clamp01(holdTimer / data.HoldDuration);

                        onHoldProgress?.Invoke(progress);
                        OnHoldProgress?.Invoke(progress);
                        interactionUIController?.SetProgress(target, progress);

                        if (holdTimer >= data.HoldDuration)
                        {
                            PerformInteraction(module, target);
                            ResetHold();
                        }
                    }
                    else if (activeHoldModuleIndex == i)
                    {
                        ResetHold();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(module.Key))
                    {
                        PerformInteraction(module, target);
                    }
                }
            }
        }

        private void PerformInteraction(InteractionModule module, IInteractable target)
        {
            if (target == null) return;

            module.Execute(target, gameObject);

            if (target is MonoBehaviour mono)
                onInteracted?.Invoke(mono.gameObject);
            OnInteracted?.Invoke(target);

            if (target.Data.Cooldown > 0)
                cooldowns[target] = Time.time + target.Data.Cooldown;
        }

        private void SetOutline(IInteractable target, bool enabled)
        {
            if (target is MonoBehaviour mono && mono != null)
            {
                var outline = mono.GetComponent<InteractionOutline>();
                if (outline != null) outline.SetHighlighted(enabled);
            }
        }

        private bool IsOnCooldown(IInteractable interactable)
        {
            return cooldowns.ContainsKey(interactable);
        }

        private void UpdateCooldowns()
        {
            expiredCooldowns.Clear();
            foreach (var pair in cooldowns)
            {
                if (pair.Value <= Time.time)
                    expiredCooldowns.Add(pair.Key);
            }
            foreach (var key in expiredCooldowns)
                cooldowns.Remove(key);
        }

        private void ResetHold()
        {
            if (holdTimer > 0)
            {
                holdTimer = 0;
                interactionUIController?.SetProgress(activeHoldTarget, 0);
                onHoldProgress?.Invoke(0);
                OnHoldProgress?.Invoke(0);
            }
            activeHoldModuleIndex = -1;
            activeHoldTarget = null;
        }

        private void UpdateRangeEvents()
        {
            if (interactionUIController == null) return;

            tempInRange.Clear();
            foreach (var interactable in interactionUIController.InRangeInteractables)
                tempInRange.Add(interactable);

            foreach (var interactable in tempInRange)
            {
                if (!previousInRange.Contains(interactable))
                {
                    if (interactable is MonoBehaviour mono)
                        onEnteredRange?.Invoke(mono.gameObject);
                    OnEnteredRange?.Invoke(interactable);
                }
            }

            foreach (var interactable in previousInRange)
            {
                if (!tempInRange.Contains(interactable))
                {
                    if (interactable is MonoBehaviour mono)
                        onExitedRange?.Invoke(mono.gameObject);
                    OnExitedRange?.Invoke(interactable);
                }
            }

            (previousInRange, tempInRange) = (tempInRange, previousInRange);
        }
    }
}