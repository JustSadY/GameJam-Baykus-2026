using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Image))]
public class CutBlackout : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1.0f;
    public bool activeAtStart = true;

    private Image _blackoutImage;

    private void Awake()
    {
        _blackoutImage = GetComponent<Image>();
    }

    private void Start()
    {
        if (activeAtStart)
        {
            SetInitialAlpha(1.0f);
            SetActive(false);
        }
    }

    private void SetInitialAlpha(float alpha)
    {
        Color currentColor = _blackoutImage.color;
        _blackoutImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }

    public void SetActive(bool isActive)
    {
        StopAllCoroutines();
        float targetAlpha = isActive ? 1.0f : 0.0f;
        StartCoroutine(FadeRoutine(targetAlpha));
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Start();
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        Color currentColor = _blackoutImage.color;
        float startAlpha = _blackoutImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeSpeed);
            _blackoutImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        _blackoutImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}