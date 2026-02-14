using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VoiceSystem;

public class FirstCutScene : MonoBehaviour
{
    [SerializeField] private Subtitle[] _subtitles;
    [SerializeField] private string LoacScene;

    private void Start()
    {
        if (VoiceManager.Instance)
        {
            VoiceManager.Instance.OnSequenceFinished += OnSequenceFinished;
            VoiceManager.Instance.PlaySequence(_subtitles);
        }
    }

    private void OnSequenceFinished()
    {
        FindAnyObjectByType<CutBlackout>().activeAtStart = true;
        VoiceManager.Instance.OnSequenceFinished -= OnSequenceFinished;
        SceneManager.LoadScene(LoacScene);
    }
}