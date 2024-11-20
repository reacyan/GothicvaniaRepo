using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftSlot : UI_ItemSlot
{
    [SerializeField] protected UI_CraftWindows craftWindows;

    public void Setup(UI_CraftWindows _craftField)
    {
        craftWindows = _craftField;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //dectetion the craft can be create 
        if (item.data != null)
        {
            craftWindows.ShowCraftInformation(item.data as ItemData_Equipment);
        }
    }
}
