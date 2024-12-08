using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private ItemDrop myDropSystem;

    private Enemy enemy;

    public Stat dropCurrency;

    [Header("level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .2f;

    protected override void Start()
    {
        dropCurrency.SetDefaultValue(50);
        ApplyLevelModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        modify(strength);
        modify(agility);
        modify(intelligence);
        modify(vitality);

        modify(damage);
        modify(fireDamage);
        modify(iceDamage);
        modify(lightingDamage);

        modify(maxHealth);
        modify(armor);
        modify(evasion);
        modify(magicResistance);
        modify(dropCurrency);
    }

    public override void TakeDamage(int _damage, int _AttackDir, bool _ishitKonckbback = true)
    {
        base.TakeDamage(_damage, _AttackDir, _ishitKonckbback);
    }

    public void modify(Stat _stat)//修改属性
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    protected override void DieStats()
    {
        base.DieStats();
        enemy.Die();
        PlayerManager.instance.currency += dropCurrency.GetValue();
        myDropSystem.GenerateDrop();
    }
}
