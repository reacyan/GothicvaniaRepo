using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [Range(0, 100)]
    [SerializeField] private float chanceToLooseEqiupments;
    [Range(0, 100)]
    [SerializeField] private float chanceToLooseStashes;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemToUnequipment = new List<InventoryItem>();
        List<InventoryItem> itemToRemove = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseEqiupments)
            {
                DropItem(item.data);
                itemToUnequipment.Add(item);
            }
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseStashes)
            {
                DropItem(item.data);
                itemToRemove.Add(item);
            }
        }


        for (int i = 0; i < itemToUnequipment.Count; i++)
        {
            inventory.UnequipItem(itemToUnequipment[i].data as ItemData_Equipment);
        }
        for (int i = 0; i < itemToRemove.Count; i++)
        {
            inventory.RemoveItem(itemToRemove[i].data);
        }

        PlayerManager.instance.SetDropCurrency();
    }
}
