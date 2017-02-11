using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum Team
{
    Red,
    Blue
}

public class GameState : MonoBehaviour
{
    public PlayerController RedPlayer;
    public PlayerController BluePlayer;

    public int StartingCurrency;
    public float CurrencyAwardTime;
    public int CurrencyAwardAmount;
    float CurrencyTimer;

    public static GameState Instance;

	void Awake()
	{
		Instance = this;
        CurrencyTimer = CurrencyAwardTime;
	}

    private void Start()
    {
        RedPlayer.Currency = StartingCurrency;
        BluePlayer.Currency = StartingCurrency;
        RedPlayer.PlayerTeam = Team.Red;
        BluePlayer.PlayerTeam = Team.Blue;
    }

    private void Update()
    {
        // Award passive currency
        CurrencyTimer -= Time.deltaTime;
        while (CurrencyTimer <= 0.0f && CurrencyAwardTime > 0.0f)
        {
            CurrencyTimer += CurrencyAwardTime;
            RedPlayer.Currency += CurrencyAwardAmount;
            BluePlayer.Currency += CurrencyAwardAmount;
        }
    }

    public PlayerController GetPlayerController(Team playerTeam)
    {
        if (playerTeam == Team.Red)
        {
            return RedPlayer;
        }
        else
        {
            return BluePlayer;
        }
    }
}
