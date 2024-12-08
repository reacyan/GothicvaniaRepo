using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;//玩家
    private SpriteRenderer sr;//精灵渲染器
    private Animator anim;//动画器
    [SerializeField] private float colorLoosingSpeed;//颜色丢失速度
    [SerializeField] private bool ishitKonckback;//是否击退

    private float cloneTimer;//克隆体计时器
    [SerializeField] private Transform attackCheck;//攻击检查
    [SerializeField] private float attackCheckRadius = 1;//攻击检查半径
    private Transform closestEnemy;//最近的敌人
    private int facingDir = 1;//朝向

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

    public void SetupClone(Transform _newTransform, float _cloneDuration, Transform _closestEnemy, bool canDuplicate, float _chanceToDuplicate, Player _player, Vector3 _offset, bool _ishitKonckback = true)
    {
        anim.SetInteger("AttackNumber", Random.Range(1, 4));

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
            if (hit.GetComponent<Enemy>() != null)//如果击中的物体是敌人
            {
                player.stats.DoDamage(hit.GetComponent<CharacterStats>(),PlayerManager.instance.GetKonckDir(hit.GetComponent<CharacterStats>().transform,transform));//对敌人造成伤害  

                if (SkillManager.instance.clone.CanDuplicate())//如果可以复制克隆体  
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)//如果随机数小于复制克隆体的几率   
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0), ishitKonckback);//创建克隆体    
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()//朝最近的敌人
    {

        if (closestEnemy != null)//如果最近的敌人不为空
        {
            if (transform.position.x > closestEnemy.position.x)//如果克隆体的位置在敌人的右边
            {
                facingDir = -1;//朝左   
                transform.Rotate(0, 180, 0);//旋转180度
            }
        }
    }
}
