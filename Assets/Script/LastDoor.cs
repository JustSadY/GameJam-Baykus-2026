using System;
using InteractionSystem.Struct;
using UnityEngine;

public class LastDoor : InteractionSubtitlePrefab
{
    private bool isOpened = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private float smoothSpeed = 5f;

    /**
     * Initializes the rotation states based on Z axis and sets the default interaction data.
     */
    public void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, -90f);

        Data = new InteractionData()
        {
            IsActive = false
        };
    }

    /**
     * Handles the visual rotation of the door towards the target angle.
     */
    private void Update()
    {
        Quaternion targetRotation = isOpened ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    /**
     * Toggles the door state when the interaction occurs.
     * @param interactor The GameObject that initiated the interaction.
     */
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        ToggleDoorState();
    }

    /**
     * Switches the boolean state of the door.
     */
    private void ToggleDoorState()
    {
        isOpened = !isOpened;
    }

    /**
     * Activates the door interaction capability.
     */
    public void ActivatedDoor()
    {
        Data.IsActive = true;
    }

    /**
     * Checks if the door can be interacted with based on phone activity.
     * @param interactor The object checking for interaction.
     * @return True if interaction is possible, otherwise false.
     */
    public override bool CanInteract(GameObject interactor)
    {
        bool baseCheck = base.CanInteract(interactor);
        bool phoneCheck = !FindAnyObjectByType<LastPhone>().Data.IsActive;
        return baseCheck && phoneCheck;
    }
}