using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    public Player player;
    public bool isDie = false;

    [Header("Major stats")]
    public Stat strength;// 1 point increase damage by 1 and crit.power by 1%
    public Stat agility;// 1 point increase evasion by 1% and crit.chance by 1%
    public Stat intelligence;//1 point increase magic damage 1 and magic resiestance by 3
    public Stat vitality;// 1 point increase health by 5 point

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;//暴击率
    public Stat critPower;//暴击伤害 --default value 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    [SerializeField] private float ailmentsDuration = 4;
    public bool isIgnited;// 造成灼烧伤害（持续失去血量）
    public bool isChilled;// 减少5%的护甲
    public bool isShocked;// 增加20%闪避性能

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTiemer;
    private int igniteDamage;

    [SerializeField] private GameObject ShockStrikePrefab;
    private int shockDamage;
    public int currentHealth;

    public int finalDamage;

    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        fx = GetComponent<EntityFX>();
        player = PlayerManager.instance.player;

        DecreaseHealth(0);
    }

    protected virtual void Update()
    {
        if (isIgnited || isChilled || isShocked)
        {
            ignitedTimer -= Time.deltaTime;
            chilledTimer -= Time.deltaTime;
            shockedTimer -= Time.deltaTime;
            igniteDamageTiemer -= Time.deltaTime;
        }


        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }
        if (chilledTimer < 0)
        {
            isChilled = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }

    public virtual void DecreaseHealth(int _damage)  //减少hp
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public void IncreaseHealth(int _Health)  //增加hp
    {
        currentHealth += _Health;

        if (currentHealth > GetMaxHealthValue())
        {
            currentHealth = GetMaxHealthValue();
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public void IncreaseStatsBy(int _modifier, float _duration, Stat _statsToModify)  //增加属性
    {
        StartCoroutine(StatsModCoroutine(_modifier, _duration, _statsToModify));  //启用一条线程
    }

    private IEnumerator StatsModCoroutine(int _modifier, float _duration, Stat _statsToModify)
    {
        _statsToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statsToModify.RemoveModifier(_modifier);
    }



    protected virtual void Die()  //死亡调用
    {
        isIgnited = false;
        isChilled = false;
        isShocked = false;
        isDie = true;
    }

    public virtual void DoDamage(CharacterStats _targetStats)  //伤害计算
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        //_targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats, totalDamage);
    }


    #region Magical damage and ailments
    public virtual void DoMagicDamage(CharacterStats _targetStats, int _totalDamage = 0)  //造成魔法伤害
    {
        //获取数值
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        //魔法总伤计算
        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage + _totalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)  //元素伤害检测
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _iceDamage && _lightingDamage > _fireDamage;

        while (!canApplyChill && !canApplyIgnite && !canApplyShock)  //相同伤害随机造成减益
        {
            int random = Random.Range(0, 3);
            if (random == 0 && _fireDamage > 0)
            {
                canApplyIgnite = true;
            }
            else if (random == 1 && _iceDamage > 0)
            {
                canApplyChill = true;
            }
            else if (random == 2 && _lightingDamage > 0)
            {
                canApplyShock = true;
            }

            _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
            return;
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _isIgnite, bool _isChill, bool _isShock)  //造成元素减益
    {
        bool canApplyIgnite = _isIgnite && !_isChill && !_isShock;
        bool canApplyChill = !_isIgnite && _isChill && !_isShock;
        bool canApplyShock = !_isIgnite && !_isChill && _isShock;

        if (_isIgnite && canApplyIgnite)
        {
            isIgnited = _isIgnite;
            ignitedTimer = ailmentsDuration;
            fx.IgniteFxFor(ignitedTimer);
        }

        if (_isChill && canApplyChill)
        {
            isChilled = _isChill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .4f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(chilledTimer);
        }

        if (_isShock && canApplyShock)
        {

            if (!isShocked)
            {
                ApplyShock(_isShock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                Transform closestEnemy = SkillHitNearsTarget(player.skillCheckRadius);

                if (closestEnemy != null)
                {
                    GameObject newShockStrike = Instantiate(ShockStrikePrefab, transform.position, Quaternion.identity);

                    newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
                }
            }
            //find closest target , only among the enemies
            //instanitate thunder strike
            //setup thunder strike
        }

    }

    private void ApplyIgniteDamage()  //造成灼烧伤害
    {
        if (igniteDamageTiemer < 0)
        {
            DecreaseHealth(igniteDamage);

            if (currentHealth <= 0)
            {
                Die();
            }

            igniteDamageTiemer = igniteDamageCooldown;
        }
    }

    public void ApplyShock(bool _isShock)  //雷击
    {
        if (isShocked)
        {
            return;
        }

        isShocked = _isShock;
        shockedTimer = ailmentsDuration;
        fx.ShockFxFor(shockedTimer);
    }

    public Transform SkillHitNearsTarget(float _skillCheckRadius)  //打击附近的目标
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.skillCheck.transform.position, _skillCheckRadius);//检测正前方一个圆形范围内的敌人

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            // if (closestEnemy == null)  // delete if you don't want shocked target to be hit by shock strike
            // {
            //     closestEnemy = transform;
            // }
        }

        return closestEnemy;
    }

    //设置灼伤伤害
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    //设置雷击伤害
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion


    #region Stat calculations
    public virtual void TakeDamage(int _damage) //造成伤害
    {
        finalDamage = _damage;

        DecreaseHealth(finalDamage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDie)
        {
            Die();
        }
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)  //护甲计算
    {
        if (isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .95f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 1, int.MaxValue);
        return totalDamage;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)  //计算魔法抗性
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 1, int.MaxValue);
        return totalMagicDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)   //闪避攻击
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private bool CanCrit()  //暴击计算
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)  //暴击伤害计算
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()  //MaxHp值计算
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #endregion


    public Stat GetStat(StatType buffType)
    {
        Stat stat = null;

        switch (buffType)
        {
            case StatType.strength:
                stat = strength;
                break;
            case StatType.agility:
                stat = agility;
                break;
            case StatType.intelligence:
                stat = intelligence;
                break;
            case StatType.vitality:
                stat = vitality;
                break;
            case StatType.damage:
                stat = damage;
                break;
            case StatType.critChance:
                stat = critChance;
                break;
            case StatType.critPower:
                stat = critPower;
                break;
            case StatType.maxHealth:
                stat = maxHealth;
                break;
            case StatType.armor:
                stat = armor;
                break;
            case StatType.evasion:
                stat = evasion;
                break;
            case StatType.magicResistance:
                stat = magicResistance;
                break;
            case StatType.fireDamage:
                stat = fireDamage;
                break;
            case StatType.iceDamage:
                stat = iceDamage;
                break;
            case StatType.lightingDamage:
                stat = lightingDamage;
                break;
        }

        return stat;
    }
}
