using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public GameState State;
    public Text RevolutionariesText;
    public Text OppressorsText;
    public Image GameTickProgress;
	
	// Update is called once per frame
	void Update ()
    {
        GameTickProgress.fillAmount = State.GetTickProgress();
        RevolutionariesText.text = "Revolutionaries: " + State.RedHealth.ToString();
        OppressorsText.text = "Oppressors: " + State.BlueHealth.ToString();
    }
}
