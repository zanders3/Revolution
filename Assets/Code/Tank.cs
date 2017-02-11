using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : MonoBehaviour
{
    NavMeshAgent agent;
    public Team Team { get; private set; }
    public Material[] RedMaterials;
    public Material[] BlueMaterials;

    public float AttackRadius = 10f;
    public Transform TurretTransform;

    Vector3 factoryTarget;

	public void Setup(Vector3 target, Team team)
    {
        Team = team;
        factoryTarget = target;
        GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = team == Team.Red ? RedMaterials : BlueMaterials;

        agent = GetComponent<NavMeshAgent>();
        agent.destination = target;
        StartCoroutine(RunAI());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }

    IEnumerator RunAI()
    {
        while (true)
        {
            //Pick the best target
            Tank target = null;
            Factory targetFactory = null;
            while (target == null && targetFactory == null)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRadius);
                float bestTargetDist = float.MaxValue;
                for (int i = 0; i < colliders.Length; i++)
                {
                    Tank tank = colliders[i].GetComponent<Tank>();
                    if (tank != null && tank.Team != Team)
                    {
                        float dist = Vector3.Distance(transform.position, tank.transform.position);
                        if (dist < bestTargetDist)
                        {
                            target = tank;
                            bestTargetDist = dist;
                        }
                    }
                    Factory factory = colliders[i].GetComponent<Factory>();
                    if (factory != null)
                    {
                        targetFactory = factory;
                    }
                }
                yield return null;
            }

            agent.Stop();

            Vector3 targetPosition = targetFactory != null ? targetFactory.transform.position : target.transform.position;
            Vector3 targetDelta = (targetPosition - transform.position).normalized;
            TurretTransform.rotation = Quaternion.LookRotation(-targetDelta, Vector3.up);
        }
    }
}
