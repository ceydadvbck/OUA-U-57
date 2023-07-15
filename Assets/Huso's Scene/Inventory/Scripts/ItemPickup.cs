using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;

    void Pickup()
    {
        InvantroyManager.Instance.Add(Item);
        Destroy(gameObject);
    }


    private void OnMouseDown()
    {
        Pickup();
    }


}

