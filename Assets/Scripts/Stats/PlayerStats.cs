using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
}
