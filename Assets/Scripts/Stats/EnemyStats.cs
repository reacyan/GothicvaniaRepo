using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .2f;

    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
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
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    public void modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }


    protected override void Die()
    {
        base.Die();

        enemy.Die();
    }
}
