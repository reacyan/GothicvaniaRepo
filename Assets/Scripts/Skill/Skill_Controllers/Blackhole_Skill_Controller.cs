using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeyList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;

    private bool canGrow=true;
    private bool canShrink;

    private bool canCreateHotKey = true;
    private bool isHitKonckbback = false;
    private bool cloneAttackReleased;

    private int amountOfAttacks;
    private float blackholeTimer;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;
    private float delayTime=.3f;
    private float delayReleaseTime = 8;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

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

        if (blackholeTimer < 0)//����һ��ʱ���Զ�����
        {
            blackholeTimer = Mathf.Infinity;
            ReleaseClonaAttack();
        }

        if (Input.GetKeyDown(KeyCode.R) && !canShrink)
        {   
            ReleaseClonaAttack();
        }


        CloneAttackLogic();

        if (canGrow && !canShrink)//����ڶ�
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        else if (canShrink)//��С�ڶ�
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                //PlayerManager.instance.player.ExitBlackholeAbility();
                Destroy(gameObject);  
            }
        }

    }


    private void ReleaseClonaAttack()//�ͷſ�¡����
    {

        DestroyHotKey();
        cloneAttackReleased = true;

        PlayerManager.instance.player.MakeTransprent(true);
    }

    private void CloneAttackLogic()//�����߼�
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", .3f);//�ӳ���С�ڶ�
            }

            int randomIndex = Random.Range(0, targets.Count);//��������б��еĵ���

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
                SkillManager.instance.clone.CreateClone(targets[randomIndex], isHitKonckbback, new Vector3(xOffest, 0));//�����¡����
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

        for (int i = 0; i <= createdHotKey.Count-1; i++)//����QTE����
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);//�������˶���
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);//��ͣ���˶���

            CreateHotKey(collision);//Ϊ���˴���QTE����
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (hotKeyList.Count <= 0)
        {
            Debug.LogWarning("Not enough hot key a key code list");//listΪ��ʱ����
            return;
        }

        if (!canCreateHotKey)
        {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);//����QTE����
        createdHotKey.Add(newHotKey);//���QTE����


        KeyCode choosingKeyCode = hotKeyList[Random.Range(0, hotKeyList.Count)];//�����ȡһ��QTE������ʾ
        hotKeyList.Remove(choosingKeyCode);//ɾȥ�ѱ�ѡ��ļ�λ

        Blackhole_HotKey_Controller newHotKeyScrip = newHotKey.GetComponent<Blackhole_HotKey_Controller>();//��ȡ�ű�
        newHotKeyScrip.SetupHotKey(choosingKeyCode, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform)=>targets.Add(_enemyTransform);//��ӵ����б�
}
