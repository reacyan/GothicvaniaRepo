using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftFrame : MonoBehaviour
{
    [SerializeField] private Image image;

    public virtual void ShowStashImage(ItemData_Equipment craft)
    {
        if (craft == null)
        {
            return;
        }

        image.sprite = craft.icon;

        image.color = Color.white;
    }
}
