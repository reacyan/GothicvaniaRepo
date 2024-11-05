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

        UseFreezeEffect();
    }

    private void UseFreezeEffect()
    {
        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor == null)
        {
            return;
        }

        if (Time.time > currentArmor.itemLastTime + currentArmor.itemCooldown)
        {
            currentArmor.itemLastTime = Time.time;
            currentArmor.Effect(player.transform);
        }
        else
        {
            Debug.Log("Armor cooldown");
        }
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
}
