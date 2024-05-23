using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;

    public void Interact()
    {
        Inventory.Instance.AddKey(this);

        Disappear();
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
