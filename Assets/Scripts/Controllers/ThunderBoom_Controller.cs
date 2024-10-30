using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBoom_Controller : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (other.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = other.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget);
        }
    }


}
