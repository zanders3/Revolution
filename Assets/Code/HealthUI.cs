﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    UnitTarget target;
    Camera mainCamera;
    public Image HealthBarImage;

    int maxHealth;

	void Start()
    {
        mainCamera = Camera.main;
	}

    public void Setup(UnitTarget target)
    {
        this.target = target;
        maxHealth = target.Health;
    }
	
	void Update()
    {
        HealthBarImage.fillAmount = target.Health / (float)maxHealth;

        // Billboard to the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
