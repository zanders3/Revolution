using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Factory : TankTarget
{
    public Team Team;
	public Tank SpawnPrefab;
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
        Tank newUnit = Instantiate(SpawnPrefab, transform.position, transform.rotation);
        newUnit.Setup(
            TargetFactory.transform.position,
            Team,
            Vector3.zero
        );
    }
}
