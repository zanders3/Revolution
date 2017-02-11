using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Factory[] FactoryList;
    int SelectedFactoryIdx = -1;
    public GameObject SelectionIndicatorPrefab;
    GameObject SelectionIndicator;

    public string UpInputString;
    public string DownInputString;

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

        // Deselect the currently selected factory (if it is valid)
        if (SelectedFactoryIdx >= 0 && SelectedFactoryIdx < FactoryList.Length)
        {
            FactoryList[SelectedFactoryIdx].bIsActive = false;
        }

        // Select the new factory
        SelectedFactoryIdx = NewSelectionIdx;
        FactoryList[SelectedFactoryIdx].bIsActive = true;

        if (SelectionIndicator != null)
        {
            SelectionIndicator.transform.position = FactoryList[SelectedFactoryIdx].transform.position;
        }
        else
        {
            SelectionIndicator = Instantiate(SelectionIndicatorPrefab, FactoryList[SelectedFactoryIdx].transform.position, FactoryList[SelectedFactoryIdx].transform.rotation);
        }
    }
}
