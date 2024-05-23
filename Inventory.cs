using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    public static Inventory Instance { get { return instance; } }

    public List<Key> keys = new List<Key>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public bool HasKey(Key key)
    {
        return keys.Contains(key);
    }

    public void AddKey(Key key)
    {
        keys.Add(key);
    }
}
