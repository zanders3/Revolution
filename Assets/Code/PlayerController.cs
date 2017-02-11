using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Factory[] FactoryList;
    int SelectedFactoryIdx = -1;
    public GameObject SelectionIndicatorPrefab;
    GameObject SelectionIndicator;

    [System.NonSerialized]
    public int Currency;

    [System.NonSerialized]
    public Team PlayerTeam;

    // Inputs for switching active factory
    public string UpInputString;
    public string DownInputString;

    // Inputs for spawning units
    public string SpawnInputString0;

    // Use this for initialization
    void Start ()
    {
        ChangeSelection(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetButtonDown(UpInputString))
        {
            ChangeSelection(SelectedFactoryIdx - 1);
        }
        else if (Input.GetButtonDown(DownInputString))
        {
            ChangeSelection(SelectedFactoryIdx + 1);
        }
        else if (Input.GetButtonDown(SpawnInputString0))
        {
            TrySpawnUnit(0);
        }
    }

    void ChangeSelection(int NewSelectionIdx)
    {
        // Don't do anything if we try to change to the currently selected factory
        if (NewSelectionIdx == SelectedFactoryIdx)
        {
            return;
        }

        // Don't do anything if the new index is invalid
        if (NewSelectionIdx < 0 || NewSelectionIdx >= FactoryList.Length)
        {
            return;
        }

        // Select the new factory
        SelectedFactoryIdx = NewSelectionIdx;

        if (SelectionIndicator != null)
        {
            SelectionIndicator.transform.position = FactoryList[SelectedFactoryIdx].transform.position;
        }
        else
        {
            SelectionIndicator = Instantiate(SelectionIndicatorPrefab, FactoryList[SelectedFactoryIdx].transform.position, FactoryList[SelectedFactoryIdx].transform.rotation);
        }
    }

    void TrySpawnUnit(int unitType)
    {
        if (SelectedFactoryIdx >= 0 && SelectedFactoryIdx < FactoryList.Length)
        {
            int unitCost = 20;
            if(Currency >= unitCost)
            {
                Currency -= unitCost;
                FactoryList[SelectedFactoryIdx].SpawnUnit(unitType);
            }
        }
    }
}
