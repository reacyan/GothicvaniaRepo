using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;
    private float moveSpeed;
    private bool canExplode;
    private bool canGrow;
    private float growSpeed = 5;
    private bool canMove;

    private Transform closestTarget;


    private Animator anim => GetComponent<Animator>();

    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    public void setCrystal(float _crystalExistDuration, float _moveSpeed, bool _canExplode, bool _canMove, Transform _closestTarget)
    {
        crystalExistTimer = _crystalExistDuration;
        moveSpeed = _moveSpeed;
        canExplode = _canExplode;
        closestTarget = _closestTarget;
        canMove = _canMove;
    }

    private void Update()
    {
        //追踪效果
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
            }
        }
        else
        {
            crystalExistTimer -= Time.deltaTime;//移动时不计入存在时间
        }

        //存在时间结束
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        //爆炸动画
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }


    public void FinishCrystal()
    {

        if (canExplode)
        {
            canGrow = true;
            canMove = false;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
