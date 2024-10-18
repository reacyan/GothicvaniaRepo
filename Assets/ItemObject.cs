using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private ItemData itemData;
    // Start is called before the first frame update
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = itemData.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("picked up item" + itemData.itemName);
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
