using InteractionSystem.Interface;
using InteractionSystem.Struct;
using UnityEngine;

namespace InteractionSystem.Optional
{
    public class ChainInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private ChainStep[] steps;
        [SerializeField] private bool loop;

        private int currentStep;
        private bool completed;
        private Collider _collider;

        public InteractionData Data => completed || steps.Length == 0 ? default : steps[currentStep].Data;
        public int CurrentStep => currentStep;
        public bool IsCompleted => completed;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void Interact(GameObject interactor)
        {
            if (completed || steps.Length == 0) return;

            steps[currentStep].OnStepCompleted?.Invoke(interactor);
            currentStep++;

            if (currentStep >= steps.Length)
            {
                if (loop)
                {
                    currentStep = 0;
                }
                else
                {
                    completed = true;
                    if (_collider) _collider.enabled = false;
                }
            }
        }

        public void ResetChain()
        {
            currentStep = 0;
            completed = false;
            if (_collider) _collider.enabled = true;
        }
    }
}
