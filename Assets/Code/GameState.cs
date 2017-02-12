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
    public int CurrencyCap = 990;
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

        // Cap the currency to avoid numbers getting too large
        RedPlayer.Currency = Mathf.Min(RedPlayer.Currency, CurrencyCap);
        BluePlayer.Currency = Mathf.Min(BluePlayer.Currency, CurrencyCap);
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

    public Factory FindEnemyFactory(Team team)
    {
        List<Factory> enemyFactoryList = GetPlayerController(team == Team.Blue ? Team.Red : Team.Blue).FactoryList;
        enemyFactoryList.RemoveAll(factory => factory == null);
        if (enemyFactoryList.Count > 0)
            return enemyFactoryList[0];

        return null;
    }
}
