using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLayout : MonoBehaviour
{
    public Team team;
    public UnitIcon FirstIcon;

	// Use this for initialization
	void Start ()
    {
        int NumUnits = GameState.Instance.UnitPrefabs.Length;
        if (NumUnits > 0 && FirstIcon != null)
        {
            FirstIcon.Setup(0, GameState.Instance.UnitPrefabs[0], team);
            for (int i = 1; i < NumUnits; ++i)
            {
                Instantiate(FirstIcon, transform).Setup(i, GameState.Instance.UnitPrefabs[i], team);
            }
        }
	}
}
