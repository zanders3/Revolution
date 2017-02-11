using UnityEngine;

public class Tank : MonoBehaviour
{
	bool bMoveForwards;
	BezierSpline spline;
	float splinePosition = 0f;
	public float Speed = 2f;

	public void Setup(BezierSpline spline, bool bMoveForwards)
	{
		this.spline = spline;
		this.bMoveForwards = bMoveForwards;
	}

	void Update()
	{
		splinePosition += (bMoveForwards ? Speed : -Speed) * Time.deltaTime;
		transform.position = spline.GetPoint(splinePosition / spline.TotalLength);
	}
}
