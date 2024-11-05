using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/item Effect/Freeze enemies")]

public class FreezeEnemies_Effect : ItemEffect
{

    [SerializeField] private float freezeDuration;
    [SerializeField] private float freezeScope;

    public override void ExecuteEffect(Transform _targetPosition)
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (player.currentHealth <= 30)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, freezeScope);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    hit.GetComponent<Enemy>().StartCoroutine("FreezeTimerFor", freezeDuration);
                }
            }
        }
    }
}
