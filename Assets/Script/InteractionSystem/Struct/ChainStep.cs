using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem.Struct
{
    [System.Serializable]
    public class ChainStep
    {
        public InteractionData Data;
        public UnityEvent<GameObject> OnStepCompleted;
    }
}
