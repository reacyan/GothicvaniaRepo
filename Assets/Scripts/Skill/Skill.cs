using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skill : MonoBehaviour
{
    [Header("Base unlock")]
    public bool baseSkillUnlocked;
    [SerializeField] protected List<SkillTree_UI> baseSkillUnlockButton = new List<SkillTree_UI>();
    [SerializeField] protected List<SkillTree_UI> branchSkillUnlockButton = new List<SkillTree_UI>();



    protected float cooldownTimer;
    [SerializeField] protected float cooldown;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public void UnlockBaseSkill()
    {
        if (baseSkillUnlockButton[0].unlocked)
        {
            Debug.Log("unlocked this");

            baseSkillUnlocked = true;
        }
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform FindCloseEnemy(Transform _checkTransform)//选择最近的目标
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 15);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
