using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public Text RedCurrencyAmount;
    public Text BlueCurrencyAmount;

    // Update is called once per frame
    void Update ()
    {
        RedCurrencyAmount.text = GameState.Instance.GetPlayerController(Team.Red).Currency.ToString();
        BlueCurrencyAmount.text = GameState.Instance.GetPlayerController(Team.Blue).Currency.ToString();
    }
}
