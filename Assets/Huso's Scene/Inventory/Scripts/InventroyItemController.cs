using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventroyItemController : MonoBehaviour
{
    Item item;

    public Button RemoveButton;
    public void RemoveItem()
    {
        InvantroyManager.Instance.Revmove(item);

        Destroy(gameObject);
    }


    public void AddItem(Item newItem)
    {
        item = newItem;
    }

}
