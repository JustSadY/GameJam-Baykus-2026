using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button ExitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        ExitButton.onClick.AddListener(Quit);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Quit()
    {
        Application.Quit();
    }
}