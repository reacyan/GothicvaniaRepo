using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}

[CreateAssetMenu(fileName = "buff Effect", menuName = "Data/item Effect/Buff Effect")]

public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _targetPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatsBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        Stat stat = null;

        switch (buffType)
        {
            case StatType.strength:
                stat = stats.strength;
                break;
            case StatType.agility:
                stat = stats.agility;
                break;
            case StatType.intelligence:
                stat = stats.intelligence;
                break;
            case StatType.vitality:
                stat = stats.vitality;
                break;
            case StatType.damage:
                stat = stats.damage;
                break;
            case StatType.critChance:
                stat = stats.critChance;
                break;
            case StatType.critPower:
                stat = stats.critPower;
                break;
            case StatType.maxHealth:
                stat = stats.maxHealth;
                break;
            case StatType.armor:
                stat = stats.armor;
                break;
            case StatType.evasion:
                stat = stats.evasion;
                break;
            case StatType.magicResistance:
                stat = stats.magicResistance;
                break;
            case StatType.fireDamage:
                stat = stats.fireDamage;
                break;
            case StatType.iceDamage:
                stat = stats.iceDamage;
                break;
            case StatType.lightingDamage:
                stat = stats.lightingDamage;
                break;
        }

        return stat;
    }
}
