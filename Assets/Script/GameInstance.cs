using UnityEngine;
using UnityEngine.UI; // Image bileþenini kullanmak için bu satýrý ekleyin


public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance { get; private set; }

    [SerializeField] private GameObject InteractionUI;
    [SerializeField] public GameObject E;
    [SerializeField] public GameObject F;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDisable()
    {
        Image image = E.GetComponent<Image>();
        Color color = image.color;
        color.a = 0f; // Alfa deðerini sýfýr yaparak görünmez hale getir
        image.color = color;
        Image image2 = F.GetComponent<Image>();
        Color color2 = image2.color;
        color2.a = 0f; // Alfa deðerini sýfýr yaparak görünmez hale getir
        image2.color = color2;
    }

    private void Start()
    {
        if (InteractionUI != null) InteractionUI.SetActive(false);
    }

    public bool IsInteractionUIActive()
    {
        if (InteractionUI == null) return false;
        return InteractionUI.activeSelf;
    }

    public void SetInteractionUI(bool setActive)
    {
        if (InteractionUI == null) return;
        InteractionUI.SetActive(setActive);
    }
}