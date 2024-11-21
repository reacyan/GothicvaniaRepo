using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{

    [Header("Clone info")]

    private bool ishitKonckback = true;

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("CLone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;



    public void CreateClone(Transform _clonePosition, Vector3 _offset, bool _ishitKonckback = true)
    {

        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, FindCloseEnemy(_clonePosition.transform), canDuplicateClone, chanceToDuplicate, player, _offset);
    }


    public void CreateCloneOnCounterAttack(Transform _enemyTransform)//生成反击clone攻击
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1.5f * player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)//延迟生成clone
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
