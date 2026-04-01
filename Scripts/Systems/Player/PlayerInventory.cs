using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    
    public Dictionary<string, ItemPIckUp> inventory = new Dictionary<string, ItemPIckUp>();

    public event Action OnInventoryChanged;

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if(inventory.ContainsKey(itemData.itemID) && itemData.stackable && inventory[itemData.itemID].stackSize < itemData.maxStacks)
        {
            inventory[itemData.itemID].stackSize += amount;
        } 
        else if (!inventory.ContainsKey(itemData.itemID))
        {
            // Ponieważ ItemPIckUp dziedziczy po MonoBehaviour, musimy stworzyć niewidzialny obiekt, który go przechowa
            GameObject container = new GameObject($"{itemData.itemID}_inventory_item");
            container.transform.SetParent(this.transform);
            container.SetActive(false);

            ItemPIckUp newItem = container.AddComponent<ItemPIckUp>();
            newItem.itemData = itemData;
            newItem.stackSize = amount;

            inventory.Add(itemData.itemID, newItem);
        }
        OnInventoryChanged?.Invoke();
    }

    void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Item")){
                ItemPIckUp itemPickUp = other.GetComponent<ItemPIckUp>();
                ItemData itemData = itemPickUp.itemData;
                if(inventory.ContainsKey(itemData.itemID) && itemData.stackable && inventory[itemData.itemID].stackSize < itemData.maxStacks){
                    inventory[itemData.itemID].stackSize += itemPickUp.stackSize;
                } else {
                    inventory.Add(itemData.itemID, itemPickUp);
                }
                OnInventoryChanged.Invoke();
                Destroy(other.gameObject);
            }
        }

    /*void Update() {
        foreach(var item in inventory){
            Debug.Log($"Item: {item.Key}, Stack Size: {item.Value.stackSize}");
        }
    }*/
}
