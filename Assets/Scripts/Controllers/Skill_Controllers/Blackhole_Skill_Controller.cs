using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;//QTE按键预设
    [SerializeField] private List<KeyCode> hotKeyList;//QTE按键列表

    private float maxSize;//最大大小
    private float growSpeed;//扩大速度      
    private float shrinkSpeed;//缩小速度

    private bool canGrow = true;//是否可以扩大  
    private bool canShrink;//是否可以缩小

    private bool canCreateHotKey = true;//是否可以创建QTE按键   
    private bool isHitKonckback = false;//是否击退
    private bool cloneAttackReleased;//克隆攻击释放

    private int amountOfAttacks;//攻击次数  
    private float blackholeTimer;//黑洞计时器
    private float cloneAttackCooldown;//克隆攻击冷却时间
    private float cloneAttackTimer;//克隆攻击计时器
    //private float delayTime = .3f;
    //private float delayReleaseTime = 8;

    private List<Transform> targets = new List<Transform>();//敌人列表
    private List<GameObject> createdHotKey = new List<GameObject>();//创建的QTE按键列表

    public bool playerCanExitState { get; private set; }//玩家是否可以退出状态

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _cloneAttackCooldown, int _amountOfAttacks, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        cloneAttackCooldown = _cloneAttackCooldown;
        amountOfAttacks = _amountOfAttacks;
        blackholeTimer = _blackholeDuration;
    }


    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)//到达一定时间自动攻击
        {
            blackholeTimer = Mathf.Infinity;
            ReleaseClonaAttack();
        }

        if (Input.GetKeyDown(KeyCode.R) && !canShrink)
        {
            ReleaseClonaAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)//扩大黑洞
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        else if (canShrink)//缩小黑洞
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                //PlayerManager.instance.player.ExitBlackholeAbility();
                Destroy(gameObject);
            }
        }

    }


    private void ReleaseClonaAttack()//释放克隆攻击
    {

        DestroyHotKey();
        cloneAttackReleased = true;

        if (!SkillManager.instance.clone.crystalInsteadOfClone)
        {
            PlayerManager.instance.player.fx.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()//攻击逻辑
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", .3f);//延迟缩小黑洞
            }

            int randomIndex = Random.Range(0, targets.Count);//随机攻击列表中的敌人

            float xOffest;

            if (Random.Range(0, 100) > 50)
            {
                xOffest = 1.5f;
            }
            else
            {
                xOffest = -1.5f;
            }


            if (targets.Count > 0 && amountOfAttacks > 0)
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffest, 0), isHitKonckback);//创造克隆攻击
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        canShrink = true;
        cloneAttackReleased = false;
        playerCanExitState = true;
    }

    private void DestroyHotKey()
    {
        if (createdHotKey.Count <= 0)
        {
            return;
        }

        for (int i = 0; i <= createdHotKey.Count - 1; i++)//销毁QTE按键
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);//继续敌人动作
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);//暂停敌人动作

            CreateHotKey(collision);
        }
    }

    private void CreateHotKey(Collider2D collision)//为敌人创建QTE按键
    {
        if (hotKeyList.Count <= 0)
        {
            Debug.LogWarning("Not enough hot key a key code list");//list为空时报错
            return;
        }

        if (!canCreateHotKey)
        {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);//创建QTE按键
        createdHotKey.Add(newHotKey);//添加QTE按键


        KeyCode choosingKeyCode = hotKeyList[Random.Range(0, hotKeyList.Count)];//随机获取一个QTE按键提示
        hotKeyList.Remove(choosingKeyCode);//删去已被选择的键位

        Blackhole_HotKey_Controller newHotKeyScrip = newHotKey.GetComponent<Blackhole_HotKey_Controller>();//获取脚本
        newHotKeyScrip.SetupHotKey(choosingKeyCode, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);//添加敌人列表
}
