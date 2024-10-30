using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/item Effect/Ice And Fire")]

public class IceAndFireEffect : ItemEffect
{

    public float movespeed;
    private int moveFacing;

    [SerializeField] private GameObject IceAndFirePrefab;

    public override void ExecuteEffect(Transform _targetPosition)
    {
        Player player = PlayerManager.instance.player;
        moveFacing = PlayerManager.instance.player.facingDir;
        GameObject newIceAndFire = Instantiate(IceAndFirePrefab, player.transform.position, Quaternion.identity);

        newIceAndFire.GetComponent<IceAndFire_Controller>().SetupIceAndFire(movespeed, moveFacing, _targetPosition);

        Destroy(newIceAndFire, 1.5f);
        //TODO:set up new shock strike
    }
}
