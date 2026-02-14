using System;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance { get; private set; }

    [SerializeField] private GameObject InteractionUI;

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