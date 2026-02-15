using System;
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
        _glitchPrefab.SetActive(false);
    }

    public void StartGlitch()
    {
        Debug.Log("Starting glitch");
        StartCoroutine(ExecuteGlitchEffect());
    }

    private IEnumerator ExecuteGlitchEffect()
    {
        Invoke(nameof(NextScene), 1);
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StopAllCoroutines();
        StartCoroutine(LoadSceneCoroutine());
    }


    private IEnumerator LoadSceneCoroutine()
    {
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Next();
    }

    private void Next()
    {
        StopAllCoroutines();
        _glitchPrefab.SetActive(false);
    }
}