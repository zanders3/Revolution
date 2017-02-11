using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Team
{
    Red,
    Blue
}

public class Factory : TankTarget
{
    public Team Team;
	public Tank SpawnPrefab;
	public bool bIsActive = true;

    public float SpawnAnimTime;
    public Transform SpawnLocation, SpawnEndLocation;

	void Start()
	{
        GameState.Instance.OnGameTick.AddListener(OnGameTick);
	}

	void OnGameTick()
	{
		if (bIsActive)
		{
            SpawnLocation = transform;

            List<Factory> targetFactories = Team == Team.Red ? GameState.Instance.BlueFactories : GameState.Instance.RedFactories;
			Instantiate(SpawnPrefab, SpawnLocation.position, SpawnLocation.rotation).Setup(
                targetFactories[UnityEngine.Random.Range(0, targetFactories.Count)].transform.position,
                Team,
                /*SpawnEndLocation.position*/Vector3.zero
            );
		}
	}

    public override void Damage()
    {
        if (Team == Team.Blue)
            GameState.Instance.BlueHealth--;
        else if (Team == Team.Red)
            GameState.Instance.RedHealth--;
    }
}
