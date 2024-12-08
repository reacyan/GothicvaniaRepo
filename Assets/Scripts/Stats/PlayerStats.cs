using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int _damage, int _AttackDir, bool _ishitKonckbback = true)
    {
        base.TakeDamage(_damage, _AttackDir, _ishitKonckbback);
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

    protected override void DieStats()
    {
        base.DieStats();

        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void OnEvasion()
    {
        base.OnEvasion();

        SkillManager.instance.dodge.CreateMirageOnDodge();
    }
}
