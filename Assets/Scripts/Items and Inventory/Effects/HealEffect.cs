using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Hemophagia", menuName = "Data/item Effect/Hemophagia")]

public class HealEffect : ItemEffect
{
    public int healthValue;
    public float healthPrecent;

    public override void ExecuteEffect(Transform _targetPosition)
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (healthPrecent != 0)
        {
            healthValue = Mathf.RoundToInt(player.finalDamage * healthPrecent);
        }

        player.IncreaseHealth(healthValue);
    }
}
