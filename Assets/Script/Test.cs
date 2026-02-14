using UnityEngine;
using VoiceSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private Subtitle[] _subtitle;

    void Start()
    {
        VoiceManager.Instance.PlaySequence(_subtitle);
    }
}