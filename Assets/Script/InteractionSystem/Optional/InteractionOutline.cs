using UnityEngine;

namespace InteractionSystem.Optional
{
    public class InteractionOutline : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Color highlightColor = new Color(1f, 0.8f, 0.2f);
        [SerializeField] private float emissionIntensity = 0.3f;

        private MaterialPropertyBlock _propertyBlock;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            if (targetRenderer == null)
                targetRenderer = GetComponentInChildren<Renderer>();
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetHighlighted(bool highlighted)
        {
            if (targetRenderer == null) return;

            targetRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(EmissionColor, highlighted ? highlightColor * emissionIntensity : Color.black);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
