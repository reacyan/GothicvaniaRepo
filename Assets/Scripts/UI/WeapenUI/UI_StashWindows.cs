using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_StashWindows : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stashName;
    [SerializeField] private TextMeshProUGUI stashDescription;
    [SerializeField] private Image image;


    public void ShowStashDescription(ItemData stash)//显示物品描述
    {
        if (stash == null)
        {
            return;
        }

        stashName.text = stash.itemName;
        stashDescription.text = stash.itemDescription;

        ShowStashImage(stash);
    }

    public void ShowStashImage(ItemData stash)//显示物品图片
    {
        if (stash == null)
        {
            return;
        }

        image.sprite = stash.icon;

        image.color = Color.white;
    }
}
