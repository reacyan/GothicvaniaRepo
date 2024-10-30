using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Shock Strike Effect", menuName = "Data/item Effect/Shock Strike")]

public class ShockStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject ShockStrikePrefab;

    public override void ExecuteEffect(Transform _targetPosition)
    {
        GameObject newShockStrike = Instantiate(ShockStrikePrefab, _targetPosition.position, Quaternion.identity);

        Destroy(newShockStrike, 1);
        //TODO:set up new shock strike
    }
}
