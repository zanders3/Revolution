using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealthUI : MonoBehaviour
{
    Tank tank;
    Camera mainCamera;
    public Image HealthBarImage;

    int maxTankHealth;

	void Start()
    {
        mainCamera = Camera.main;
	}

    public void Setup(Tank tank)
    {
        this.tank = tank;
        maxTankHealth = tank.Health;
    }
	
	void Update()
    {
        HealthBarImage.fillAmount = tank.Health / maxTankHealth;

        // Billboard to the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
