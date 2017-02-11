using System;
using UnityEngine;

public class Factory : MonoBehaviour
{
	public Tank SpawnPrefab;
	public BezierSpline TargetSpline;
	public bool bMovesForwardsAlongSpline = true;
	public bool bIsActive = true;

	void Start()
	{
		GameState.Instance.OnGameTick.AddListener(OnGameTick);
	}

	void OnGameTick()
	{
		if (bIsActive)
		{
			Tank tank = Instantiate(SpawnPrefab, transform.position, transform.rotation);
			tank.Setup(TargetSpline, bMovesForwardsAlongSpline);
		}
	}
}
