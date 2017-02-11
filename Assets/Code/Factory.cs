using System;
using UnityEngine;

public enum Team
{
    Red,
    Blue
}

public class Factory : MonoBehaviour
{
    public Team CurrentOwner;
	public Tank SpawnPrefab;
	public Path TargetPath;
	public bool bMovesForwardsAlongSpline = true;
	public bool bIsActive = true;

	void Start()
	{
        if (TargetPath != null)
        {
            TargetPath.FactoryStart = this;
            GameState.Instance.OnGameTick.AddListener(OnGameTick);
        }
	}

	void OnGameTick()
	{
		if (bIsActive)
		{
			Tank tank = Instantiate(SpawnPrefab, transform.position, transform.rotation);
			tank.Setup(TargetPath, bMovesForwardsAlongSpline);
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
