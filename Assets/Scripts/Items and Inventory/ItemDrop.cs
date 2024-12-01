using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemData[] possibleDrop;
    [SerializeField] private int possibleItemDropCount;
    [SerializeField] private GameObject DropPrefab;

    private List<ItemData> dropList = new List<ItemData>();

    public virtual void GenerateDrop()//选取掉落物品
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) < possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0; i < possibleItemDropCount; i++)//限制掉落数量
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }


    protected void DropItem(ItemData _item)//生成掉落物品
    {
        GameObject newDrop = Instantiate(DropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVector = new Vector2(Random.Range(-4, 4), Random.Range(4, 8));//随机方位

        newDrop.GetComponent<ItemObject>().SetupItem(_item, randomVector);
    }
}
