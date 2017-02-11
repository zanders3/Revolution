using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Team
{
    Red,
    Blue
}

public class Factory : MonoBehaviour
{
    public Team CurrentOwner;
	public Tank SpawnPrefab;
	public bool bIsActive = true;

	void Start()
	{
        GameState.Instance.OnGameTick.AddListener(OnGameTick);
	}

	void OnGameTick()
	{
		if (bIsActive)
		{
            List<Factory> targetFactories = CurrentOwner == Team.Red ? GameState.Instance.BlueFactories : GameState.Instance.RedFactories;
			Instantiate(SpawnPrefab, transform.position, transform.rotation).Setup(
                targetFactories[UnityEngine.Random.Range(0, targetFactories.Count)].transform.position,
                CurrentOwner
            );
		}
	}

    public void Damage()
    {
        if (CurrentOwner == Team.Blue)
            GameState.Instance.BlueHealth--;
        else if (CurrentOwner == Team.Red)
            GameState.Instance.RedHealth--;
    }
}
