using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InteractionSystem.Struct;

namespace InteractionSystem.UI
{
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image progressBar;

        private Vector3 defaultScale;

        private void Awake()
        {
            defaultScale = transform.localScale;

            if (progressBar != null)
                progressBar.fillAmount = 0f;
        }

        public void SetData(InteractionData data)
        {
            if (promptText != null) promptText.text = data.Prompt;
            if (descriptionText != null) descriptionText.text = data.Description;

            if (progressBar != null)
                progressBar.gameObject.SetActive(data.Type == InteractionType.Hold);
        }

        public void SetHighlighted(bool highlighted)
        {
            transform.localScale = highlighted ? defaultScale * 1.2f : defaultScale;
        }

        public void SetProgress(float progress)
        {
            if (progressBar != null)
                progressBar.fillAmount = Mathf.Clamp01(progress);
        }
    }
}
