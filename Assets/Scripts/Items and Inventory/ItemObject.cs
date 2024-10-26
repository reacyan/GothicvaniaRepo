using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;


    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemdate, Vector2 _vector)
    {
        itemData = _itemdate;
        rb.velocity = _vector;

        SetupVisuals();
    }


    public void PickupItem()//拾取物品
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }

}
