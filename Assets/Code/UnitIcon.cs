using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitIcon : MonoBehaviour
{
    public Image Icon;
    public Image Overlay;
    public Text CostText;
    public Text Key;

    int m_unitType;
    int m_cost;
    Team m_team;

    public void Setup(Unit unit, Team team, int unitType)
    {
        m_unitType = unitType;
        m_cost = unit.Cost;
        m_team = team;
        CostText.text = m_cost.ToString();
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

    private void Update()
    {
        int currency = GameState.Instance.GetPlayerController(m_team).Currency;
        if (currency >= m_cost)
        {
            CostText.color = Color.black;
            Overlay.fillAmount = 0;
        }
        else
        {
            CostText.color = Color.red;
            Overlay.fillAmount = 1;
        }
    }
}
