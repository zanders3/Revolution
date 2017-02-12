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

    [System.NonSerialized]
    public Unit[] UnitPrefabs;

    public static GameState Instance;

	void Awake()
	{
		Instance = this;
        CurrencyTimer = CurrencyAwardTime;
        if (RedPlayer.FactoryList.Count > 0)
        {
            UnitPrefabs = RedPlayer.FactoryList[0].SpawnPrefabs;
        }
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
            AwardCurrency(CurrencyAwardAmount, Team.Red);
            AwardCurrency(CurrencyAwardAmount, Team.Blue);
        }
    }

    public void AwardCurrency(int amount, Team team)
    {
        PlayerController player = GetPlayerController(team);
        player.Currency += amount;
        player.Currency = Mathf.Min(player.Currency, CurrencyCap);
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
