using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Image bileþenini kullanmak için bu satýrý ekleyin

public class EUI : MonoBehaviour
{
    [Tooltip("Tetikleyici olarak kullanýlacak olan BoxCollider")]
    [SerializeField] private BoxCollider triggerBox;

    private Coroutine _hideCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // Tetikleyiciye giren nesnenin "Player" etiketine sahip olup olmadýðýný kontrol edin
        if (other.CompareTag("Player"))
        {
            // Eðer çalýþan bir gizleme coroutine'i varsa, onu durdurun
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }
            
            // Image'ýn alfasýný tam görünür yapýn
            SetImageAlpha(1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Tetikleyiciden çýkan nesnenin "Player" etiketine sahip olup olmadýðýný kontrol edin
        if (other.CompareTag("Player"))
        {
            // Image'ý gecikmeli olarak gizlemek için coroutine'i baþlatýn
            _hideCoroutine = StartCoroutine(HideImageAfterDelay(2f));
        }
    }

    /// <summary>
    /// Belirtilen gecikme süresinden sonra Image'ý gizler ve tetikleyiciyi devre dýþý býrakýr.
    /// </summary>
    private IEnumerator HideImageAfterDelay(float delay)
    {
        // Belirtilen süre kadar bekleyin
        yield return new WaitForSeconds(delay);

        // Image'ýn alfasýný tam þeffaf yapýn
        SetImageAlpha(0f);

        // BoxCollider'ý devre dýþý býrakýn
        if (triggerBox != null)
        {
            triggerBox.enabled = false;
        }
    }

    /// <summary>
    /// TargetImage'ýn alfa deðerini ayarlar.
    /// </summary>
    private void SetImageAlpha(float alpha)
    {
        Image targetImage = GameInstance.Instance.E.GetComponent<Image>();
        if (targetImage != null)
        {
            Color currentColor = targetImage.color;
            currentColor.a = alpha;
            targetImage.color = currentColor;
        }
    }
}
