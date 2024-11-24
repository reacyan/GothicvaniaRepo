using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Craft_UI : MonoBehaviour
{
    //需要默认打开的窗口
    [SerializeField] private GameObject weaponOfCraft;
    [SerializeField] private GameObject stashIntroductory;
    [SerializeField] private GameObject craftIntroductory;
    [SerializeField] private GameObject stashUI;
    [SerializeField] private GameObject titleHeaderUI;
    [SerializeField] private GameObject stashHeaderUI;
    [SerializeField] private GameObject craftHeaderUI;
    [SerializeField] private GameObject createButtonUI;

    public void SwitchToCraftPanel(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        stashUI.SetActive(true);
        titleHeaderUI.SetActive(true);
        stashHeaderUI.SetActive(true);
        craftHeaderUI.SetActive(true);
        stashIntroductory.SetActive(true);
        craftIntroductory.SetActive(true);
        createButtonUI.SetActive(true);

        Inventory.instance.EnableCraft(_menu);
    }

    private void Start()
    {
        SwitchToCraftPanel(weaponOfCraft);
    }
}
