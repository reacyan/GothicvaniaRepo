using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stashName;
    [SerializeField] private TextMeshProUGUI stashDescription;
    [SerializeField] private GameObject requestSlotParent;
    public Button button;

    private ItemData_Equipment stash;

    private Image[] requestSlot;


    private void Start()
    {
        requestSlot = requestSlotParent.GetComponentsInChildren<Image>();
        button.onClick.AddListener(CreateCraft);
    }

    public void ShowCraftDescription(ItemData_Equipment _stash)
    {
        stashName.text = _stash.itemName;

        stash = _stash;

        stashDescription.text = _stash.GetDescription();

        for (int i = 0; i < _stash.CraftingMaterial.Count; i++)
        {
            requestSlot[i].sprite = _stash.CraftingMaterial[i].data.icon;
            requestSlot[i].color = Color.white;
            requestSlot[i].GetComponentInChildren<TextMeshProUGUI>().text = "X" + _stash.CraftingMaterial[i].stackSize.ToString();
        }

    }

    public void CreateCraft()
    {
        if (stash != null)
        {
            Inventory.instance.CanCraft(stash, stash.CraftingMaterial);
        }
    }
}
