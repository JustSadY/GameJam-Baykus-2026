using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace VoiceSystem
{
    [Serializable]
    public struct Subtitle
    {
        public string title;
        public float durationIfNoAudio;
        public float pauseBeforeNext;
    }

    [RequireComponent(typeof(AudioSource))]
    public class VoiceManager : MonoBehaviour
    {
        public static VoiceManager Instance { get; private set; }
        public event Action OnSequenceFinished;

        [Header("Audio Settings")] [SerializeField]
        private AudioClip talkClip;

        [Range(0.1f, 2f)] [SerializeField] private float thinVowelPitch = 1.25f;
        [Range(0.1f, 2f)] [SerializeField] private float thickVowelPitch = 0.85f;
        [Range(0.1f, 2f)] [SerializeField] private float defaultPitch = 1.0f;

        [Header("UI & Effects")] [SerializeField]
        private TextMeshProUGUI subtitleText;

        [SerializeField] private GameObject subtitlePanel;
        [SerializeField] private float typingSpeed = 0.04f;

        private AudioSource _audioSource;
        private Coroutine _playbackCoroutine;
        private Coroutine _typingCoroutine;
        private bool _skipRequested;

        private readonly HashSet<char> _vowels = new HashSet<char>
        {
            'a', 'e', 'ı', 'i', 'o', 'ö', 'u', 'ü',
            'A', 'E', 'I', 'İ', 'O', 'Ö', 'U', 'Ü'
        };

        private readonly HashSet<char> _consonants = new HashSet<char>
        {
            'b', 'c', 'ç', 'd', 'f', 'g', 'ğ', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 's', 'ş', 't', 'v', 'y', 'z',
            'B', 'C', 'Ç', 'D', 'F', 'G', 'Ğ', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'Ş', 'T', 'V', 'Y', 'Z'
        };

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _audioSource = GetComponent<AudioSource>();
            ConfigureSpatialAudio();
            if (subtitlePanel != null) subtitlePanel.SetActive(false);
        }

        private void ConfigureSpatialAudio()
        {
            _audioSource.spatialBlend = 0.0f;
        }

        public void Skip() => _skipRequested = true;

        public void PlaySequence(Subtitle subtitle)
        {
            PlaySequence(new Subtitle[] { subtitle });
        }

        public void PlaySequence(Subtitle[] subtitles)
        {
            if (subtitles == null) return;
            if (_playbackCoroutine != null) StopCoroutine(_playbackCoroutine);
            _playbackCoroutine = StartCoroutine(PlaybackSequenceRoutine(subtitles));
        }

        /**
         * Core typewriter routine that handles character logic and sound modulation.
         * @param text The string to display and process for audio effects.
         */
        private IEnumerator TypeTextRoutine(string text)
        {
            subtitleText.text = string.Empty;

            foreach (char letter in text.ToCharArray())
            {
                subtitleText.text += letter;

                if (!char.IsWhiteSpace(letter))
                {
                    PlayCharacterSound(letter);
                }

                yield return new WaitForSeconds(typingSpeed);
            }

            _typingCoroutine = null;
        }

        /**
         * Determines the pitch based on the vowel type and plays the sound.
         * @param letter The character being currently processed.
         */
        private void PlayCharacterSound(char letter)
        {
            if (talkClip == null) return;

            if (_consonants.Contains(letter))
            {
                _audioSource.pitch = thinVowelPitch;
            }
            else if (_vowels.Contains(letter))
            {
                _audioSource.pitch = thickVowelPitch;
            }
            else
            {
                _audioSource.pitch = defaultPitch;
            }

            _audioSource.PlayOneShot(talkClip);
        }

        private IEnumerator PlaybackSequenceRoutine(Subtitle[] subtitles)
        {
            for (int i = 0; i < subtitles.Length; i++)
            {
                _skipRequested = false;
                Subtitle currentSubtitle = subtitles[i];

                if (!string.IsNullOrEmpty(currentSubtitle.title))
                {
                    if (subtitlePanel != null) subtitlePanel.SetActive(true);
                    if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
                    _typingCoroutine = StartCoroutine(TypeTextRoutine(currentSubtitle.title));
                }

                float duration = currentSubtitle.durationIfNoAudio;
                float timer = 0f;

                while (timer < duration && !_skipRequested)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (_typingCoroutine != null)
                {
                    StopCoroutine(_typingCoroutine);
                    subtitleText.text = currentSubtitle.title;
                }

                if (currentSubtitle.pauseBeforeNext > 0 && !_skipRequested)
                {
                    yield return new WaitForSeconds(currentSubtitle.pauseBeforeNext);
                }
            }

            FinishPlayback();
        }

        private void FinishPlayback()
        {
            if (subtitlePanel != null) subtitlePanel.SetActive(false);
            _playbackCoroutine = null;
            OnSequenceFinished?.Invoke();
        }
    }
}