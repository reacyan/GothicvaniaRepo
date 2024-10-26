using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newitem)//更新物品
    {
        item = _newitem;

        itemImage.color = Color.white;//UI颜色恢复

        if (item != null)
        {
            itemImage.sprite = item.data.icon;//添加图标

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();//数量显示
            }
            else
            {
                itemText.text = "";//显示为空
            }
        }
    }


    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            if (item.data.itemType == ItemType.Equipment)
            {
                Inventory.instance.EquipItem(item.data);
            }
        }
    }
}
