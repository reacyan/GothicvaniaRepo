using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength;// 1 point increase damage by 1 and crit.power by 1%
    public Stat agility;// 1 point increase evasion by 1% and crit.chance by 1%
    public Stat intelligence;//1 point increase magic damage 1 and magic resiestance by 3
    public Stat vitality;// 1 point increase health by 3 and 5 point

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

    public System.Action onHealthChanged;

    protected virtual void Awake()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Start()
    {

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

        if (igniteDamageTiemer < 0 && isIgnited)
        {
            Debug.Log(igniteDamage);

            DecreaseHealth(igniteDamage);

            if (currentHealth <= 0)
            {
                Die();
            }

            igniteDamageTiemer = igniteDamageCooldown;
        }
    }

    //伤害计算
    public virtual void DoDamage(CharacterStats _targetStats)
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
        DoMagicDamage(_targetStats);
    }

    //造成魔法伤害
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        //获取数值
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        //魔法伤害计算
        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        //魔法抗性计算
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _iceDamage && _lightingDamage > _fireDamage;

        while (!canApplyChill && !canApplyIgnite && !canApplyShock)
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
            SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 1, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _isIgnite, bool _isChill, bool _isShock)
    {
        bool canApplyIgnite = isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

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

                HitNearsTargetWithShockStrike();
            }
            //find closest target , only among the enemies
            //instanitate thunder strike
            //setup thunder strike
        }

    }

    public void ApplyShock(bool _isShock)
    {
        if (isShocked)
        {
            return;
        }

        isShocked = _isShock;
        shockedTimer = ailmentsDuration;
        fx.ShockFxFor(shockedTimer);
    }

    private void HitNearsTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);

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
            if (closestEnemy == null)  // delete if you don't want shocked target to be hit by shock strike
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(ShockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    //造成伤害
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealth(_damage);
        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log(_damage);
    }

    public void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;
        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    protected virtual void Die()
    {
        isIgnited = false;
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
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

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack avoided");

            return true;
        }

        return false;
    }
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}
