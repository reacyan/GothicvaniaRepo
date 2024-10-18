using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Linq;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;
    private float moveSpeed;
    private bool canExplode;
    private bool canGrow;
    private float growSpeed = 5;
    private bool canMove;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    private Animator anim => GetComponent<Animator>();

    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private Player player;


    public void SetupCrystal(float _crystalExistDuration, float _moveSpeed, bool _canExplode, bool _canMove, Transform _closestTarget, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalExistDuration;
        moveSpeed = _moveSpeed;
        canExplode = _canExplode;
        closestTarget = _closestTarget;
        canMove = _canMove;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    private void Update()
    {
        //追踪效果
        if (canMove && closestTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < .5f)
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

        //爆炸效果
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()//爆炸攻击
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicDamage(hit.GetComponent<CharacterStats>());
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
