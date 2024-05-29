using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interact;
    [SerializeField] private float interactionDistance;
    private Camera playerCamera;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = Camera.main; 
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance && IsLookingAtKey())
        {
            interact.text = "Press (E) To Interact";

            if(Input.GetKeyDown(KeyCode.E))
                interact.text = "";
        }
        else
        {
            interact.text = "";
        }
    }

    public void Interact()
    {
        Inventory.Instance.AddKey(this);

        Disappear();
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }

    bool IsLookingAtKey()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }
}
