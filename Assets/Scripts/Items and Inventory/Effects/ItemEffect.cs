using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/item Effect")]

public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _targetPosition)
    {

    }
}
