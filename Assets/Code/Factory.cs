using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Factory : TankTarget
{
    public Team Team;
	public Tank SpawnPrefab;
    public float SpawnAnimTime;
    public Transform SpawnLocation, SpawnEndLocation;
    public Factory TargetFactory;

    public override void Damage()
    {
        //if (CurrentOwner == Team.Blue)
        //    GameState.Instance.BlueHealth--;
        //else if (CurrentOwner == Team.Red)
        //    GameState.Instance.RedHealth--;
    }

    public void SpawnUnit(int unitType)
    {
        Tank newUnit = Instantiate(SpawnPrefab, SpawnLocation.position, SpawnLocation.rotation);
        newUnit.Setup(
            TargetFactory.transform.position,
            Team,
            SpawnEndLocation.position
        );
    }
}
