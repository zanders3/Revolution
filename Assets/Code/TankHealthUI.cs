using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealthUI : MonoBehaviour
{
    public Tank tank;
    Camera mainCamera;
    public Image HealthBarImage;

	// Use this for initialization
	void Awake()
    {
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update()
    {
        // Update fill amount based on tank health (TODO)
        HealthBarImage.fillAmount = 0.4f;

        // Billboard to the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
