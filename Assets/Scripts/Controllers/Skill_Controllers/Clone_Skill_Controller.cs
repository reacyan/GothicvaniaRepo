using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    [SerializeField] private bool ishitKonckback;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 1;
    private Transform closestEnemy;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, bool _ishitKonckback, Transform _closestEnemy, bool canDuplicate, float _chanceToDuplicate, Player _player, Vector3 _offset = default(Vector3))
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }

        player = _player;
        ishitKonckback = _ishitKonckback;
        transform.position = _newTransform.position + _offset;

        cloneTimer = _cloneDuration;
        closestEnemy = _closestEnemy;
        canDuplicateClone = canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;

        FaceClosestTarget();
    }


    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, ishitKonckback, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
