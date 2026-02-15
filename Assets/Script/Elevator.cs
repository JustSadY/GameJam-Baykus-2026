using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    [SerializeField] private GameObject elevatorDoor;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isDoorOpen = false;
    private bool isAlreadyTriggered = false;
    private float moveSpeed = 3f;
    private float waitTime = 2f;


    private void Start()
    {
        if (elevatorDoor != null)
        {
            closedPosition = elevatorDoor.transform.localPosition;
            openPosition = closedPosition + new Vector3(-3.8f, 0, 0);
        }
    }

    private void Update()
    {
        Vector3 targetPosition = isDoorOpen ? openPosition : closedPosition;
        elevatorDoor.transform.localPosition = Vector3.Lerp(elevatorDoor.transform.localPosition, targetPosition,
            Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAlreadyTriggered)
        {
            isAlreadyTriggered = true;
            isDoorOpen = true;
            Invoke(nameof(LoadNextScene), 1.5f);
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}