using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Glitch : MonoBehaviour
{
    [SerializeField] private Sprite[] _glitchSprites;
    [SerializeField] private float waitForSeconds = 0.1f;
    [SerializeField] private GameObject _glitchPrefab;
    private Image _mainImage;

    private void Start()
    {
        _mainImage = _glitchPrefab.GetComponent<Image>();
    }

    public void StartGlitch()
    {
        StartCoroutine(ExecuteGlitchEffect());
    }

    private IEnumerator ExecuteGlitchEffect()
    {
        Invoke(nameof(NextScene), 2);
        _glitchPrefab.SetActive(true);
        while (true)
        {
            if (_glitchSprites.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, _glitchSprites.Length);
                _mainImage.sprite = _glitchSprites[randomIndex];
            }

            yield return new WaitForSeconds(waitForSeconds);
        }
    }

    private void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnValidate()
    {
        StopAllCoroutines();
        _glitchPrefab.SetActive(false);
    }
}