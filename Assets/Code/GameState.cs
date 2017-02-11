using UnityEngine;
using UnityEngine.Events;


public class GameState : MonoBehaviour
{
	public int RedHealth = 10, BlueHealth = 10;
	public float GameTickTime = .5f;

	public static GameState Instance;

	public UnityEvent OnGameTick = new UnityEvent();
	float currentTickTime = 0f;

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		currentTickTime -= Time.deltaTime;
		while (currentTickTime <= 0f)
		{
			OnGameTick.Invoke();
			currentTickTime += GameTickTime;
		}
	}

    public float GetTickProgress()
    {
        return Mathf.Clamp(1.0f - currentTickTime / GameTickTime, 0.0f, 1.0f);
    }
}
