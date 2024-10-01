using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float CrystalExistDuration;
    [SerializeField] private GameObject crystalPerfab;

    private GameObject currentCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;


    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;


    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindows;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPerfab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalScript.setCrystal(CrystalExistDuration, moveSpeed, canExplode, canMoveToEnemy, FindCloseEnemy(currentCrystal.transform));
        }
        else
        {
            if (!canMoveToEnemy)
            {
                Vector2 playerPos = player.transform.position;
                player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = playerPos;
            }

            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindows);
                }

                cooldown = 0;

                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                setCrystal(CrystalExistDuration, moveSpeed, canExplode, canMoveToEnemy, FindCloseEnemy(newCrystal.transform));

                if (crystalLeft.Count <= 0)
                {
                    //cooldown the skill
                    cooldown = multiStackCooldown;

                    //refill crystals
                    RefilCrystal();
                }
                return true;
            }
        }
        return false;
    }


    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPerfab);
        }
    }

    private void ResetAbility()
    {
        if (cooldown > 0)
        {
            return;
        }

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}



