using System.Collections;
using System.Collections.Generic;
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

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPerfab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalScript.setCrystal(CrystalExistDuration, moveSpeed, canMoveToEnemy, canExplode);
        }
        else
        {
            Vector2 playerPos = player.transform.position;

            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }
}
