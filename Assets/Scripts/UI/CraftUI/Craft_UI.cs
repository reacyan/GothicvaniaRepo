using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft_UI : UI
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

    public override void SwitchTo(GameObject _menu)
    {

        base.SwitchTo(_menu);

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
        SwitchTo(weaponOfCraft);
    }
}
