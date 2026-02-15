using UnityEngine;
using System;
using System.Collections;

namespace AudioSystem
{
    [Serializable]
    public class Sound
    {
        public string soundName;
        public AudioClip audioClip;
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public Sound[] musicSounds, sfxSounds;
        public AudioSource musicSource, sfxSource;

        [Header("Settings")] public bool isShuffle = false;
        private int _currentMusicIndex = -1;
        private Coroutine _fadeCoroutine;

        /**
         * Initializes the Singleton and ensures the object persists across scenes.
         */
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            sfxSource = GetComponent<AudioSource>();
        }

        public void SetMusicVolume(float targetVolume, float duration = 0.5f)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeVolumeRoutine(targetVolume, duration));
        }

        private IEnumerator FadeVolumeRoutine(float target, float duration)
        {
            float startVolume = musicSource.volume;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, target, time / duration);
                yield return null;
            }

            musicSource.volume = target;
        }

        public void Play()
        {
            if (musicSounds.Length == 0) return;
            _currentMusicIndex = isShuffle ? UnityEngine.Random.Range(0, musicSounds.Length) : 0;
            PlayCurrentIndex();
        }

        private void PlayNextAuto()
        {
            if (musicSounds.Length == 0) return;
            _currentMusicIndex = isShuffle
                ? UnityEngine.Random.Range(0, musicSounds.Length)
                : (_currentMusicIndex + 1) % musicSounds.Length;
            PlayCurrentIndex();
        }

        private void PlayCurrentIndex()
        {
            if (_currentMusicIndex < 0 || _currentMusicIndex >= musicSounds.Length) return;
            musicSource.clip = musicSounds[_currentMusicIndex].audioClip;
            musicSource.Play();
        }

        public void PlaySFX(string name)
        {
            Sound sound = Array.Find(sfxSounds, x => x.soundName == name);
            if (sound != null) sfxSource.PlayOneShot(sound.audioClip);
        }

        public void PlaySFX(AudioClip audioClip)
        {
            sfxSource.clip = audioClip;
            sfxSource.Play();
        }
    }
}