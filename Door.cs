using System.Collections;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private enum DoorState { Unlocked, Locked, Jammed }

    [Header("Door States")]
    [Space(5)]
    [SerializeField] private DoorState currentState = DoorState.Unlocked;
    [SerializeField] private TextMeshProUGUI interact;
    [SerializeField] private TextMeshProUGUI states;
    private bool isDoorMoving = false;
    private bool isDoorOpen = false;
    private bool isJammedDoorOpen = false;

    [Header("Door Interaction")]
    [Space(5)]
    [SerializeField] private float doorInteractionDistance = 2f;
    [SerializeField] private float openSpeed = 2f;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Quaternion jammedRotation;

    [Header("Other")]
    [Space(5)]
    public Key requiredKey;
    private Transform player;
    private Camera playerCamera;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, -90, 0);
        jammedRotation = closedRotation * Quaternion.Euler(0, -20, 0);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = Camera.main; 
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= doorInteractionDistance && IsLookingAtDoor())
        {
            interact.text = "Press (E) To Interact";

            if (Input.GetKeyDown(KeyCode.E) && !isDoorMoving)
            {
                InteractWithDoor();
            }
        }
        else
        {
            if (interact != null)
            {
                interact.text = "";
            }
        }
    }

    bool IsLookingAtDoor()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, doorInteractionDistance))
        {
            if (hit.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    void InteractWithDoor()
    {
        if (currentState == DoorState.Unlocked)
        {
            ToggleDoor();
        }
        else if (currentState == DoorState.Locked)
        {
            if (Inventory.Instance.HasKey(requiredKey))
            {
                UnlockDoor();
            }
            else
            {
                states.text = "Locked";
                StartCoroutine(ClearStateTextAfterDelay(0.5f));
            }
        }
        else if (currentState == DoorState.Jammed)
        {
            states.text = "The Door is jammed";
            StartCoroutine(ClearStateTextAfterDelay(4f));
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
        states.text = "Unlocked";
        StartCoroutine(ClearStateTextAfterDelay(4f));
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
        states.text = "";
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
        states.text = "";
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
        states.text = "";
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
        states.text = "";
    }

    IEnumerator ClearStateTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        states.text = "";
    }
}
