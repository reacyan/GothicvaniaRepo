using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        EnemyManager.instance.enemy.DamageEffect();
    }
}
