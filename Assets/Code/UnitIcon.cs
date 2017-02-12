using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitIcon : MonoBehaviour
{
    public Image Icon;
    public Image Overlay;
    public Text Cost;
    public Text Key;
    public int UnitType;

    public void Setup(Unit unit, Team team, int unitType)
    {
        UnitType = unitType;
        Cost.text = unit.Cost.ToString();
        if (team == Team.Red)
        {
            Key.text = unit.RedKeyString;
            Icon.sprite = unit.RedIcon;
        }
        else
        {
            Key.text = unit.BlueKeyString;
            Icon.sprite = unit.BlueIcon;
        }
    }
}
