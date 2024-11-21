using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] protected UI_StashWindows stashWindows;

    public InventoryItem item;

    protected UI ui;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void Setup(UI_StashWindows _stashField)
    {
        stashWindows = _stashField;
    }

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


    public void CleanUpSlot()//清空槽位
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
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Inventory.instance.RemoveItem(item.data);
                return;
            }

            if (item.data.itemType == ItemType.Equipment)
            {
                Inventory.instance.EquipItem(item.data);
            }

            if (item.data.itemType == ItemType.Material && stashWindows != null)
            {
                stashWindows.ShowStashDescription(item.data);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui.itemToolTip == null)
        {
            return;
        }

        if (item != null && item.data as ItemData_Equipment != null)
        {
            Vector2 mousePosition = Input.mousePosition;

            float xOffset = 0;
            float yOffset = 0;

            if (mousePosition.x > Screen.width / 2)
            {
                xOffset = -75;
            }
            else
            {
                xOffset = 75;
            }

            if (mousePosition.y > Screen.height / 2)
            {
                yOffset = -75;
            }
            else
            {
                yOffset = 75;
            }

            ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);

            ui.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui.itemToolTip == null)
        {
            return;
        }

        if (item != null && item.data as ItemData_Equipment != null)
        {
            ui.itemToolTip.HideToolTip();
        }
    }
}
