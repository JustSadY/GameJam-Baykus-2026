using AudioSystem;
using UnityEngine;
using VoiceSystem;

public class Notify : MonoBehaviour
{
    private Subtitle subtitle = new Subtitle()
    {
        title = "Kişisel Algılama",
        durationIfNoAudio = 2
    };
    public void PlaySound(AudioClip _audioClip)
    {
        AudioManager.Instance.PlaySFX(_audioClip);
    }

    public void LoadGlicth()
    {
        Glitch glitch = FindAnyObjectByType<Glitch>(); 
        glitch.StartGlitch();
    }

    public void Next()
    {
        VoiceManager.Instance.PlaySequence(subtitle);
    }

    public void End()
    {
        Application.Quit();
    }
}