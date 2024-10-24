using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //dectetion the craft can be create 
        ItemData_Equipment craftItem = item.data as ItemData_Equipment;

        Inventory.instance.CanCraft(craftItem, craftItem.CraftingMaterial);
    }

}
