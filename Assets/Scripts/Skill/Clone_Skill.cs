using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{

    [Header("Clone info")]

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Header("CLone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        baseSkillUnlockButton[0].GetComponent<Button>().onClick.AddListener(UnlockBaseSkill);
    }

    public void CreateClone(Transform _clonePosition, Vector3 _offset, bool _ishitKonckback = true)
    {
        if (!baseSkillUnlockButton[0].unlocked)
        {
            return;
        }

        if (baseSkillUnlockButton[2].unlocked)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, FindCloseEnemy(_clonePosition.transform), canDuplicateClone, chanceToDuplicate, player, _offset, _ishitKonckback);
    }


    public void CreateCloneOnWithParry(Transform _enemyTransform)//生成反击clone攻击
    {
        StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(1.5f * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCorotine(Transform _transform, Vector3 _offset)//延迟生成clone
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }

    public bool CanDuplicate()
    {
        if (baseSkillUnlockButton[3].unlocked)
        {
            return true;
        }
        return false;
    }

    public bool canDuplicateCloneWithBlackHole()
    {
        if (branchSkillUnlockButton[0].unlocked)
        {
            return true;
        }
        return false;
    }
}
