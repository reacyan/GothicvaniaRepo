using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI_CraftWindows : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stashName;
    [SerializeField] private TextMeshProUGUI stashDescription;
    [SerializeField] private Image image;
    [SerializeField] private GameObject craftingMaterialPrefab;
    [SerializeField] private Transform craftScroll;
    [SerializeField] private TextMeshProUGUI craftDescription;

    private ItemData_Equipment craft;

    private Image[] requestSlot;


    private void Start()
    {
        requestSlot = new Image[0];
    }

    public void ShowCraftInformation(ItemData_Equipment _craft)//显示物品描述
    {
        ResetRequestSlot(_craft);
        StartCoroutine(UpdateRequestSlot(_craft));

        ShowCraftName(_craft);
        ShowCraftImage(_craft);
        ShowCraftDescription(_craft);
    }

    private void ShowCraftName(ItemData_Equipment _craft)
    {
        craft = _craft;
        stashName.text = _craft.itemName;
        stashDescription.text = _craft.GetDescription();
    }

    private void ResetRequestSlot(ItemData_Equipment _craft)//重置材料槽
    {
        requestSlot = new Image[0];

        for (int i = 0; i < craftScroll.childCount; i++)
        {
            Destroy(craftScroll.GetChild(i).gameObject);
        }
    }

    private IEnumerator UpdateRequestSlot(ItemData_Equipment _craft)//强制更新场景状态后再扩展RequestSlot
    {
        yield return null;

        requestSlot = craftScroll.GetComponentsInChildren<Image>();

        for (int j = 0; j < _craft.CraftingMaterial.Count; j++)
        {
            if (_craft.CraftingMaterial.Count > requestSlot.Length)
            {
                GameObject newCraftScroll = Instantiate(craftingMaterialPrefab, craftScroll);
                requestSlot = craftScroll.GetComponentsInChildren<Image>();
            }

            ShowCraft(j);
        }
    }

    private void ShowCraft(int Count)
    {
        requestSlot[Count].color = Color.white;
        requestSlot[Count].sprite = craft.CraftingMaterial[Count].data.icon;
        requestSlot[Count].GetComponentInChildren<TextMeshProUGUI>().text = "X" + craft.CraftingMaterial[Count].stackSize.ToString();
    }

    private void ShowCraftImage(ItemData_Equipment craft)//显示物品图片
    {
        if (craft == null)
        {
            return;
        }

        image.sprite = craft.icon;

        image.color = Color.white;
    }

    private void ShowCraftDescription(ItemData_Equipment craft)//显示物品描述
    {
        craftDescription.text = craft.itemDescription;
    }

    public void CreateCraft()
    {
        if (craft != null)
        {
            Inventory.instance.CanCraft(craft, craft.CraftingMaterial);
        }
    }
}
