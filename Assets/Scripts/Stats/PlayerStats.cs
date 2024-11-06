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

    public override void DecreaseHealth(int _damage)
    {
        base.DecreaseHealth(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (Inventory.instance.UseArmorEffect())
        {
            currentArmor.Effect(transform);
        }
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
}
