using UnityEngine;

public class Key : MonoBehaviour
{
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
