using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class Crystal_Skill : Skill
{
    [SerializeField] private float CrystalExistDuration;
    [SerializeField] private GameObject crystalPerfab;

    private GameObject currentCrystal;

    [Header("Moving crystal")]
    [SerializeField] private float moveSpeed;


    [Header("Multi stacking crystal")]
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindows;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        baseSkillUnlockButton[0].GetComponent<Button>().onClick.AddListener(UnlockBaseSkill);
    }


    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (baseSkillUnlockButton[1].unlocked)
            {
                return;
            }

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (branchSkillUnlockButton[0].unlocked)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    //实例化水晶
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPerfab, player.transform.position, Quaternion.identity);

        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(CrystalExistDuration, moveSpeed, baseSkillUnlockButton[1].unlocked, FindCloseEnemy(currentCrystal.transform), player);
    }

    //选择随机目标
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()//释放多重水晶
    {
        if (baseSkillUnlockButton[3].unlocked && crystalLeft.Count > 0)
        {

            if (crystalLeft.Count == amountOfStacks)
            {
                Invoke("ResetAbility", useTimeWindows);//延时补充
            }

            cooldown = 0;

            GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
            GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, quaternion.identity);

            crystalLeft.Remove(crystalToSpawn);

            newCrystal.GetComponent<Crystal_Skill_Controller>().
            SetupCrystal(CrystalExistDuration, moveSpeed, baseSkillUnlockButton[1].unlocked, FindCloseEnemy(newCrystal.transform), player);

            if (crystalLeft.Count <= 0)
            {
                //cooldown the skill
                cooldown = multiStackCooldown;

                //refill crystals
                RefilCrystal();
            }
            return true;
        }
        return false;
    }


    private void RefilCrystal()//补充水晶
    {

        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPerfab);
        }
    }

    private void ResetAbility()//重置技能
    {
        if (cooldown > 0)
        {
            return;
        }

        cooldownTimer = multiStackCooldown;//重置cd
        RefilCrystal();
    }
}



