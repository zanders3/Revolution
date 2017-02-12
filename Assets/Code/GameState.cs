using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Team
{
    Red,
    Blue
}

public enum GameStage
{
    FullscreenFade,
    Frontend,
    Frog,
    Gameplay,
    GameOver
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
    bool reloadScene = false;

    GameStage currentStage = GameStage.FullscreenFade;
    public Animator FrontendAnimator;
    public CanvasGroup FrontendUI, GameplayUI, FrogUI, GameOverUI, FullscreenUI;

    public Image RedWon, BlueWon;

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

    void Start()
    {
        RedPlayer.Currency = StartingCurrency;
        BluePlayer.Currency = StartingCurrency;
        RedPlayer.PlayerTeam = Team.Red;
        BluePlayer.PlayerTeam = Team.Blue;
    }

    IEnumerator MoveToGameplay()
    {
        currentStage = GameStage.Frog;
        FrontendAnimator.SetBool("InGameplay", true);
        yield return new WaitForSeconds(3f);
        currentStage = GameStage.Gameplay;
    }

    int NumAliveFactories(List<Factory> factoryList)
    {
        int count = 0;
        for (int i = 0; i < factoryList.Count; i++)
            if (factoryList[i] != null)
                count++;

        return count;
    }

    void Update()
    {
        FrontendUI.alpha = Mathf.MoveTowards(FrontendUI.alpha, currentStage == GameStage.Frontend ? 1f : 0f, Time.deltaTime * 3f);
        FrogUI.alpha = Mathf.MoveTowards(FrogUI.alpha, currentStage == GameStage.Frog ? 1f : 0f, Time.deltaTime * 8f);
        GameplayUI.alpha = Mathf.MoveTowards(GameplayUI.alpha, currentStage == GameStage.Gameplay ? 1f : 0f, Time.deltaTime);
        GameOverUI.alpha = Mathf.MoveTowards(GameOverUI.alpha, currentStage == GameStage.GameOver ? 1f : 0f, Time.deltaTime);
        FullscreenUI.alpha = Mathf.MoveTowards(FullscreenUI.alpha, currentStage == GameStage.FullscreenFade ? 1f : 0f, Time.deltaTime);

        if (currentStage == GameStage.FullscreenFade)
        {
            if (FullscreenUI.alpha >= 1f)
            {
                if (reloadScene)
                    SceneManager.LoadScene(0);
                currentStage = GameStage.Frontend;
            }
        }
        else if (currentStage == GameStage.Frontend)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(MoveToGameplay());
        }
        else if (currentStage == GameStage.Gameplay)
        {
            //Game over detection
            int numRedFactories = NumAliveFactories(RedPlayer.FactoryList);
            int numBlueFactories = NumAliveFactories(BluePlayer.FactoryList);
            if (numRedFactories <= 0)
            {
                RedWon.gameObject.SetActive(false);
                BlueWon.gameObject.SetActive(true);
                currentStage = GameStage.GameOver;
            }
            else if (numBlueFactories <= 0)
            {
                RedWon.gameObject.SetActive(true);
                BlueWon.gameObject.SetActive(false);
                currentStage = GameStage.GameOver;
            }

            // Award passive currency
            CurrencyTimer -= Time.deltaTime;
            while (CurrencyTimer <= 0.0f && CurrencyAwardTime > 0.0f)
            {
                CurrencyTimer += CurrencyAwardTime;
                AwardCurrency(CurrencyAwardAmount * numRedFactories, Team.Red);
                AwardCurrency(CurrencyAwardAmount * numBlueFactories, Team.Blue);
            }
        }
        else if (currentStage == GameStage.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentStage = GameStage.FullscreenFade;
                reloadScene = true;
            }
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
