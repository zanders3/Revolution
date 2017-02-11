using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public Text RevolutionariesText;
    public Text OppressorsText;
    public Image GameTickProgress;
	
	// Update is called once per frame
	void Update ()
    {
        GameTickProgress.fillAmount = 0.0f;//  State.GetTickProgress();
        RevolutionariesText.text = "Revolutionaries: " + GameState.Instance.GetPlayerController(Team.Red).Currency.ToString();
        OppressorsText.text = "Oppressors: " + GameState.Instance.GetPlayerController(Team.Blue).Currency.ToString();
    }
}
