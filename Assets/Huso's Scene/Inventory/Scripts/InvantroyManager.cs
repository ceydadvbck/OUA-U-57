using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvantroyManager : MonoBehaviour
{
    public static InvantroyManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject ItemExplanation;

    public Toggle EnableRemove;

    public InventroyItemController[] InventroyItems;


    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Revmove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }


        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();
            //var itemExplanation = obj.transform.Find("ItemExplanation").GetComponent<Text>();
            var ItemButton = obj.transform.Find("ItemButton").GetComponent<Button>();
            itemName.text = item.itemName;

            itemIcon.sprite = item.icon;
            ;
            if (EnableRemove.isOn)
                removeButton.gameObject.SetActive(true);

        }

        SetInventroyItems();
    }

    public void EnableItemsRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventroyItems()
    {
        InventroyItems = ItemContent.GetComponentsInChildren<InventroyItemController>();

        for (int i = 0; i < Items.Count; i++)
        {
            InventroyItems[i].AddItem(Items[i]);
        }
    }

    public void ItemExplanationButton()
    {

        Debug.Log("button");
        GameObject.Find("ItemExplanation").gameObject.SetActive(true);
        ItemExplanation.GetComponent<Text>().text = GetComponent<Item>().itemExplanation.ToString();
    }

}
