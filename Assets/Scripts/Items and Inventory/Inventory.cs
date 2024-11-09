using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;


    [Header("Inventory UI")]

    [SerializeField] private Transform InventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] StatSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = InventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();

        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        StatSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)//装备物品
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;//父类转换子类
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//遍历字典
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)//是否有相同物品
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);//将已装备物品移除装备栏
            AddItem(oldEquipment);//将已装备物品添加到物品栏
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(_item);//将新物品从物品栏中移除

        UpdateSlotUI();//更新UI
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)//移除装备
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            itemToRemove.RemoveModifiers();
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);

            UpdateSlotUI();
        }
    }

    private void UpdateSlotUI()//更新物品槽位UI
    {

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].CleanUpSlot();//清空装备槽
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();//清空物品槽
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();//清空仓库
        }

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//遍历字典
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)//对比字典的物品是否与装备槽位对应
                {
                    equipmentSlot[i].UpdateSlot(item.Value);//更新装备槽位UI
                }
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);//更新物品槽
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);//更新仓库
        }

        for (int i = 0; i < StatSlot.Length; i++)
        {
            StatSlot[i].UpdateStatValueUI();
        }

    }

    public void AddItem(ItemData _item)//添加物品
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem(_item))
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    public bool CanAddItem(ItemData _item)
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            {
                Debug.Log("stash item");
                return true;
            }

            Debug.Log("No More Slot");
            return false;
        }
        return true;
    }

    private void AddToStash(ItemData _item)//添加到仓库
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)//添加到物品栏
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);//构造函数
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)//移除物品
    {
        if (_item.itemType == ItemType.Equipment)
        {
            RemoveToInventory(_item);
        }
        else if (_item.itemType == ItemType.material)
        {
            RemoveToStash(_item);
        }

        UpdateSlotUI();
    }

    private void RemoveToInventory(ItemData _item)//从物品栏中移除
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();

            }
        }
    }

    private void RemoveToStash(ItemData _item)//从仓库中移除
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                stash.Remove(value);
                stashDictionary.Remove(_item);

            }
            else
            {
                value.RemoveStack();
            }
        }
    }

    public bool CanCraft(ItemData_Equipment _ItemToCraft, List<InventoryItem> _requiredMaterial)//制作物品
    {

        List<InventoryItem> MaterialToMove = new List<InventoryItem>();//创建将被使用的材料的列表

        for (int i = 0; i < _requiredMaterial.Count; i++)//制造材料种类数
        {
            if (stashDictionary.TryGetValue(_requiredMaterial[i].data, out InventoryItem stashValue))//配对库存中是否存在对应材料
            {
                if (stashValue.stackSize < _requiredMaterial[i].stackSize)//对比材料数量是否足够
                {
                    Debug.Log("not enough" + stashValue.data.name);
                    return false;
                }
                else
                {
                    MaterialToMove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not material");
                return false;
            }
        }
        for (int i = 0; i < MaterialToMove.Count; i++)//移除被使用的材料
        {
            for (int J = 0; J < _requiredMaterial[i].stackSize; J++)//按照需求的材料数量移除
            {
                RemoveItem(MaterialToMove[i].data);
            }
        }

        AddItem(_ItemToCraft);//添加制作后的物品
        Debug.Log("is your craft" + _ItemToCraft.name);
        return true;
    }

    public ItemData_Equipment GetEquipment(EquipmentType _type)//获取装备
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//遍历字典
        {
            if (item.Key.equipmentType == _type)//当字典中有与_type配对的类型时
            {
                equipedItem = item.Key;//赋值
            }
        }
        return equipedItem;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.flask);

        if (currentFlask == null)
        {
            return;
        }

        if (Time.time > currentFlask.itemLastTime + currentFlask.itemCooldown)
        {
            currentFlask.itemLastTime = Time.time;
            currentFlask.Effect(null);
        }
        else
        {
            Debug.Log("flask cooldown");
        }
    }

    public bool UseArmorEffect()
    {
        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor == null)
        {
            return false;
        }

        if (Time.time > currentArmor.itemLastTime + currentArmor.itemCooldownTimer)
        {
            currentArmor.itemCooldownTimer = currentArmor.itemCooldown;
            currentArmor.itemLastTime = Time.time;
            return true;
        }
        return false;
    }
}
