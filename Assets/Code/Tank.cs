using UnityEngine;

public class Tank : MonoBehaviour
{
	bool bMoveForwards;
	BezierSpline spline;
    Path path;
	float splinePosition = 0f;
	public float Speed = 2f;

	public void Setup(Path path, bool bMoveForwards)
	{
        Debug.Assert(path != null);

        SetCurrentPath(path);
		this.bMoveForwards = bMoveForwards;
        this.splinePosition = bMoveForwards ? 0f : spline.TotalLength;
	}

    void SetCurrentPath(Path path)
    {
        spline = path.GetComponent<BezierSpline>();
        this.path = path;
    }

	void Update()
	{
		splinePosition += (bMoveForwards ? Speed : -Speed) * Time.deltaTime;
        if (splinePosition >= spline.TotalLength)
        {
            splinePosition -= spline.TotalLength;
            if (path.FactoryEnd != null)
                path.FactoryEnd.Damage();
            if (path.PathEnd.Count > 0)
            {
                SetCurrentPath(path.PathEnd[Random.Range(0, path.PathEnd.Count)]);
            }
            else
                Destroy(gameObject);
        }
        else if (splinePosition <= 0f)
        {
            if (path.FactoryStart != null)
                path.FactoryStart.Damage();
            if (path.PathStart.Count > 0)
            {
                SetCurrentPath(path.PathStart[Random.Range(0, path.PathStart.Count)]);
                splinePosition = spline.TotalLength + splinePosition;
            }
            else
                Destroy(gameObject);
        }

        float alpha = splinePosition / spline.TotalLength;
		transform.position = spline.GetPoint(alpha);
        transform.rotation = Quaternion.LookRotation(spline.GetVelocity(alpha).normalized, Vector3.up);
	}
}
