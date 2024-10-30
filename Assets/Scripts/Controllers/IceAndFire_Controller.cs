using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : ThunderBoom_Controller
{
    private float movespeed;
    private int moveFacing;
    private Transform targetTransfrom;

    public void SetupIceAndFire(float _moveSpeed, int _moveFacing, Transform _targetTransfrom)
    {
        movespeed = _moveSpeed;
        moveFacing = _moveFacing;
        targetTransfrom = _targetTransfrom;
    }

    private void Start()
    {
        if (moveFacing == -1)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    private void Update()
    {
        Debug.Log(moveFacing);
        Debug.Log(targetTransfrom);
        if (targetTransfrom != null)
        {
            transform.right = targetTransfrom.transform.position - transform.position;//朝向目标

            transform.position = Vector2.MoveTowards(transform.position, targetTransfrom.position, movespeed * Time.deltaTime);//朝向目标移动
        }
        else
        {
            transform.position += new Vector3(movespeed * moveFacing * Time.deltaTime, 0, 0);//朝前方移动
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats enemy = other.GetComponent<EnemyStats>();

        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (enemy != null)
        {
            player.DoDamage(enemy);

            Destroy(gameObject, .15f);
        }
    }
}
