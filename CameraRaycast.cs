using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private float interactionDistance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.green, 1f);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
            {
                Key key = hit.collider.GetComponent<Key>();
                if (key != null)
                {
                    key.Interact();
                }
            }
        }
    }
}
