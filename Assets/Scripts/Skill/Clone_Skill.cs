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
    [SerializeField] private bool canCreateCloneOnDashStart;
    [SerializeField] private bool canCreateCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;


    public void CreateClone(Transform _clonePosition, bool _ishitKonckback = true, Vector3 _offset = default(Vector3))
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _ishitKonckback, FindCloseEnemy(_clonePosition.transform), _offset);
    }

    public void CreateCloneOnDashStart()
    {
        if (canCreateCloneOnDashStart)
        {
            CreateClone(player.transform, ishitKonckback, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (canCreateCloneOnDashOver)
        {
            CreateClone(player.transform, ishitKonckback, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1.5f * player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, true, _offset);
    }
}
