using UnityEngine;
using UnityEngine.SceneManagement;
using VoiceSystem;

public class Car : ConnectionLostInteractionPrefab
{
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}