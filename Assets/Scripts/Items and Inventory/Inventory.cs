using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<ItemData_Equipment> allCraftItem;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> craft;
    public Dictionary<ItemData_Equipment, InventoryItem> craftDictionary;


    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private UI_StashWindows stashWindows;
    [SerializeField] private UI_CraftWindows craftWindows;


    private UI_CraftSlot[] craftItemSlot;
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] StatSlot;

    [Header("Data Base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;

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

        craft = new List<InventoryItem>();
        craftDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
        craftItemSlot = new UI_CraftSlot[0];
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        StatSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        if (loadedEquipment.Count > 0)
        {
            foreach (var equip in loadedEquipment)
            {
                EquipItem(equip);
            }

        }

        if (loadedItems.Count > 0)//载入文件存在才调用
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }

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

        foreach (var item in equipmentDictionary)//遍历字典
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
            foreach (var item in equipmentDictionary)//遍历字典
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

        for (int i = 0; i < craft.Count; i++)
        {
            craftItemSlot[i].UpdateSlot(craft[i]);
        }
    }

    public void AddItem(ItemData _item)//添加物品
    {

        if (CanAddItem(_item))
        {
            if (_item.itemType == ItemType.Equipment)
            {
                AddToInventory(_item);
            }
            else if (_item.itemType == ItemType.Material)
            {
                AddToStash(_item);
            }
        }

        UpdateSlotUI();
    }


    public void EnableCraft(GameObject _menu)
    {
        UI_CraftSlot[] newSlot = _menu.GetComponentsInChildren<UI_CraftSlot>();

        ItemData_Equipment newCraft = newSlot[0].item.data as ItemData_Equipment;//获取菜单类型

        Transform newTransform = _menu.transform.Find("Craft panel/Viewport/Content");//获取父节点

        ResetCraftSlot(newTransform);

        StartCoroutine(UpdateCraftSlot(newCraft, _menu, newTransform));
    }

    private void ResetCraftSlot(Transform newTransform)
    {
        craftItemSlot = new UI_CraftSlot[0];//重置craftItemSlot
        craft.Clear();//清空craft
        craftDictionary.Clear();//清空craftDictionary

        for (int i = 0; i < newTransform.childCount; i++)
        {
            Destroy(newTransform.GetChild(i).gameObject);
        }
    }


    private IEnumerator UpdateCraftSlot(ItemData_Equipment _craft, GameObject _menu, Transform CraftParent)
    {
        yield return null;

        craftItemSlot = _menu.GetComponentsInChildren<UI_CraftSlot>();//获取清空后的slot长度

        for (int i = 0; i < allCraftItem.Count; i++)//从所有的craft中选取对应类型的craft
        {
            if (allCraftItem[i].equipmentType == _craft.equipmentType)
            {
                InventoryItem newItem = new InventoryItem(allCraftItem[i]);//构造函数
                craft.Add(newItem);
                craftDictionary.Add(allCraftItem[i], newItem);

                if (craft.Count > craftItemSlot.Length)//当craft长度超出slot长度，进行扩展
                {
                    GameObject newCraftSlot = Instantiate(craftSlotPrefab, CraftParent);//将实例化的slot挂载至父节点
                    newCraftSlot.GetComponent<UI_CraftSlot>().Setup(craftWindows);//并设置窗口
                    craftItemSlot = _menu.GetComponentsInChildren<UI_CraftSlot>();//获取当前slot长度
                }
            }
        }

        UpdateSlotUI();
    }


    public bool CanAddItem(ItemData _item)
    {
        if (_item == null)
        {
            return false;
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashItem) || inventoryDictionary.TryGetValue(_item, out InventoryItem equipmentItem))
        {
            return true;
        }

        if (_item.itemType == ItemType.Equipment&& inventory.Count >= inventoryItemSlot.Length)
        {
            GameObject newItemSlot = Instantiate(itemSlotPrefab, inventorySlotParent);
            inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();

            return true;
        }
        else if (_item.itemType == ItemType.Material&& stash.Count >= stashItemSlot.Length)
        {
            GameObject newStashSlot = Instantiate(itemSlotPrefab, stashSlotParent);
            newStashSlot.GetComponent<UI_ItemSlot>().Setup(stashWindows);

            stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
            return true;
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
        else if (_item.itemType == ItemType.Material)
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
            if (stashDictionary.TryGetValue(_requiredMaterial[i].data, out InventoryItem stash))//配对库存中是否存在对应材料
            {
                if (stash.stackSize < _requiredMaterial[i].stackSize)//对比材料数量是否足够
                {
                    Debug.Log("not enough " + stash.data.name);
                    return false;
                }
                else
                {
                    MaterialToMove.Add(stash);
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

        foreach (var item in equipmentDictionary)//遍历字典
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

    public void LoadData(GameData _data)
    {

        foreach (var pair in _data.inventory)//比较文件里保存data和所有data的，相同就保存在loadedItems里
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (var pair in _data.equipmentId)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == pair)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach (var pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (var pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (var pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    
#if UNITY_EDITOR

    [ContextMenu("fill item data base")]
    public void FillItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase()); 

    private List<ItemData> GetItemDataBase()//获得所有的equipmentData的IdName和data的函数
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/items" });//拿到了所有的items的文件名IdName
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);//这是通过找到的文件名拿到对应的位置
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);//这是实打实的通过位置转换拿到相应的数据
            itemDataBase.Add(itemData);//将数据填到itemDataBase里
        }

        return itemDataBase;
    }

#endif
}
