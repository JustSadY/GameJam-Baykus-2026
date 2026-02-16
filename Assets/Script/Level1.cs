using System;
using Cinemachine;
using UnityEngine;
using VoiceSystem;

public class Level1 : MonoBehaviour
{
    [SerializeField] private Subtitle[] _subtitles;

    [SerializeField] private GameObject _FirstCamera;

    private void Start()
    {
        Invoke(nameof(ALLAHCANIMIALSADAKURTULSAM), 1f);
    }

    private void ALLAHCANIMIALSADAKURTULSAM()
    {
        if (VoiceManager.Instance)
        {
            VoiceManager.Instance.PlaySequence(_subtitles);
            VoiceManager.Instance.OnSequenceFinished += SequencenFinished;
        }
    }
   

    private void SequencenFinished()
    {
        CinemachineBrain cinemachineBrain = _FirstCamera.GetComponent<CinemachineBrain>();
        if (cinemachineBrain)
        {
            cinemachineBrain.enabled = true;
        }

        VoiceManager.Instance.OnSequenceFinished -= SequencenFinished;
    }
}