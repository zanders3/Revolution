using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnSpawnUnit : UnityEvent<int> { }

public class PlayerController : MonoBehaviour {

    public List<Factory> FactoryList;
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
    public string SpawnInputString1;
    public string SpawnInputString2;
    public string SpawnInputString3;

    [System.NonSerialized]
    public OnSpawnUnit OnSpawnUnit = new OnSpawnUnit();

    // Use this for initialization
    void Start ()
    {
        ChangeSelection(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool bSelectedFactoryChanged = false;
        for (int i = FactoryList.Count - 1; i >= 0; --i)
        {
            if (FactoryList[i] == null)
            {
                FactoryList.RemoveAt(i);
                if (SelectedFactoryIdx >= i)
                {
                    if (SelectedFactoryIdx == i)
                    {
                        bSelectedFactoryChanged = true;
                    }
                    --SelectedFactoryIdx;
                }
            }
        }
        if (SelectedFactoryIdx < 0)
        {
            if (FactoryList.Count > 0)
            {
                ChangeSelection(0);
            }
            else if (SelectionIndicator != null)
            {
                Destroy(SelectionIndicator);
            }
        }
        if (bSelectedFactoryChanged)
        {
            ChangeSelection(SelectedFactoryIdx);
        }

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
        else if (Input.GetButtonDown(SpawnInputString1))
        {
            TrySpawnUnit(1);
        }
        else if (Input.GetButtonDown(SpawnInputString2))
        {
            TrySpawnUnit(2);
        }
        else if (Input.GetButtonDown(SpawnInputString3))
        {
            TrySpawnUnit(3);
        }
    }

    void ChangeSelection(int NewSelectionIdx)
    {
        // Don't do anything if the new index is invalid
        if (NewSelectionIdx < 0 || NewSelectionIdx >= FactoryList.Count)
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
        if (SelectedFactoryIdx >= 0 && SelectedFactoryIdx < FactoryList.Count)
        {
            if (unitType >= 0 && unitType < FactoryList[SelectedFactoryIdx].SpawnPrefabs.Length)
            {
                int unitCost = FactoryList[SelectedFactoryIdx].SpawnPrefabs[unitType].Cost;
                if (Currency >= unitCost)
                {
                    Currency -= unitCost;
                    FactoryList[SelectedFactoryIdx].SpawnUnit(unitType);
                    OnSpawnUnit.Invoke(unitType);
                }
            }
        }
    }
}
