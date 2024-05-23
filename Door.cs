using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private enum DoorState { Unlocked, Locked, Jammed }
    [SerializeField] private DoorState currentState = DoorState.Unlocked;

    [SerializeField] private float doorInteractionDistance = 2f;
    [SerializeField] private float openSpeed = 2f;

    private bool isDoorMoving = false;
    private bool isDoorOpen = false;
    private bool isJammedDoorOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Quaternion jammedRotation;
    public Key requiredKey;

    private Transform player;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, -90, 0);
        jammedRotation = closedRotation * Quaternion.Euler(0, -20, 0);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
         if (Vector3.Distance(player.position, transform.position) <= doorInteractionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isDoorMoving)
            {
                InteractWithDoor();
            }
        }
    }

    void InteractWithDoor()
    {
        if (currentState == DoorState.Unlocked)
        {
            ToggleDoor();
        }
        else if (currentState == DoorState.Locked && Inventory.Instance.HasKey(requiredKey))
        {
            UnlockDoor();
        }
        else if (currentState == DoorState.Jammed)
        {
            isDoorMoving = true;
            if (isJammedDoorOpen)
            {
                StartCoroutine(CloseJammedDoorSmoothly());
            }
            else
            {
                StartCoroutine(OpenJammedDoorSmoothly());
            }
        }
    }

    void ToggleDoor()
    {
        isDoorMoving = true;
        if (isDoorOpen)
        {
            StartCoroutine(CloseDoorSmoothly());
        }
        else
        {
            StartCoroutine(OpenDoorSmoothly());
        }
    }

    void UnlockDoor()
    {
        currentState = DoorState.Unlocked;
        ToggleDoor();
    }

    IEnumerator OpenDoorSmoothly()
    {
        float timeElapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (timeElapsed < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, openRotation, timeElapsed);
            timeElapsed += Time.deltaTime * openSpeed;
            yield return null;
        }

        transform.rotation = openRotation;
        isDoorMoving = false;
        isDoorOpen = true;
    }

    IEnumerator CloseDoorSmoothly()
    {
        float timeElapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (timeElapsed < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, closedRotation, timeElapsed);
            timeElapsed += Time.deltaTime * openSpeed;
            yield return null;
        }

        transform.rotation = closedRotation;
        isDoorMoving = false;
        isDoorOpen = false;
    }

    IEnumerator OpenJammedDoorSmoothly()
    {
        float timeElapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (timeElapsed < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, jammedRotation, timeElapsed);
            timeElapsed += Time.deltaTime * openSpeed;
            yield return null;
        }

        transform.rotation = jammedRotation;
        isDoorMoving = false;
        isJammedDoorOpen = true;
    }

    IEnumerator CloseJammedDoorSmoothly()
    {
        float timeElapsed = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = closedRotation;

        while (timeElapsed < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
            timeElapsed += Time.deltaTime * openSpeed;
            yield return null;
        }

        transform.rotation = targetRotation;
        isDoorMoving = false;
        isJammedDoorOpen = false;
    }
}
