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

    int m_cost;
    int m_idx;
    Team m_team;
    float cooldown = 0f;

    public void Setup(int idx, Unit unit, Team team)
    {
        m_idx = idx;
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

        GameState.Instance.GetPlayerController(m_team).OnSpawnUnit.AddListener(OnSpawnUnit);
    }

    void OnSpawnUnit(int idx)
    {
        if (m_idx == idx)
        {
            cooldown = 1f;
        }
    }

    void Update()
    {
        cooldown = Mathf.MoveTowards(cooldown, 0f, Time.deltaTime * 4f);

        int currency = GameState.Instance.GetPlayerController(m_team).Currency;
        if (currency >= m_cost)
        {
            CostText.color = Color.black;
            Overlay.fillAmount = cooldown;
        }
        else
        {
            CostText.color = Color.red;
            Overlay.fillAmount = 1f;
        }
    }
}
