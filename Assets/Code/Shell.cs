using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    Vector3 firePosition, targetPosition;
    UnitTarget target;
    
    public float MoveSpeed;
    public float ShellExplosionRadius = 2f;

    public AnimationCurve YOffset;
    float distanceToTarget;
    float currentDistance;
    Vector3 prevPosition;

    public void Setup(UnitTarget target)
    {
        this.target = target;
        targetPosition = target.gameObject.transform.position;
        firePosition = transform.position;
        distanceToTarget = Vector3.Distance(targetPosition, firePosition);
        prevPosition = transform.position - transform.forward;
        currentDistance = 0f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ShellExplosionRadius);
    }

    void Update()
    {
        currentDistance += Time.deltaTime * MoveSpeed;

        float alpha = currentDistance / distanceToTarget;
        transform.position = Vector3.Lerp(firePosition, targetPosition, alpha) + YOffset.Evaluate(alpha) * Vector3.up;
        transform.rotation = Quaternion.LookRotation((transform.position - prevPosition).normalized);
        prevPosition = transform.position;

        if (currentDistance >= distanceToTarget)
        {
            if (target != null)
                target.Damage(DamageType.Shell);

            foreach (Collider collider in Physics.OverlapSphere(transform.position, ShellExplosionRadius))
            {
                UnitTarget target = collider.GetComponent<UnitTarget>();
                if (target != null)
                {
                    target.Damage(DamageType.ShellExplosion);
                }
            }

            Destroy(gameObject);
        }
    }
}
