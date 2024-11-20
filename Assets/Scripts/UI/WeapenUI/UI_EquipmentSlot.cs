using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log(item.data);
            Inventory.instance.AddItem(item.data as ItemData_Equipment);
            Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
            CleanUpSlot();
        }
    }
}
