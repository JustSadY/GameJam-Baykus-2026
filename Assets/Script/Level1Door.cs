using UnityEngine;
using UnityEngine.SceneManagement;
using VoiceSystem;

public class Level1Door : ConnectionLostInteractionPrefab
{
    [SerializeField] private string targetSceneName = "CutScene";


    public void LoadTargetLevel()
    {
        SceneManager.LoadScene(targetSceneName);
    }

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        LoadTargetLevel();
    }
}