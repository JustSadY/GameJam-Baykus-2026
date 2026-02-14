using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    [SerializeField] private string _loadScene;

    private void Start()
    {
        Invoke(nameof(LoadScene), 5);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_loadScene);
    }
}