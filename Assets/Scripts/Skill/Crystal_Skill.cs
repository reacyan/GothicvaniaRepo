using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float CrystalExistDuration;
    [SerializeField] private GameObject crystalPerfab;

    private GameObject currentCrystal;

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPerfab, player.transform.position, quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalScript.setCrystal(CrystalExistDuration);
        }
        else
        {
            player.transform.position = currentCrystal.transform.position;
            Destroy(currentCrystal);
        }
    }
}
