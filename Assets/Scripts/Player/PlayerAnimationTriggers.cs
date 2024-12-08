using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                player.stats.DoDamage(_target,PlayerManager.instance.GetKonckDir(_target.transform,transform));

                ItemData_Equipment equipmentEffect = Inventory.instance.GetEquipment(EquipmentType.weapon);

                if (equipmentEffect != null && !(equipmentEffect.itemName == "IceAndFire Swrod"))
                {
                    equipmentEffect.Effect(_target.transform);//enable attack of effect for weapon
                }
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
