using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : TankTarget
{
    NavMeshAgent agent;
    public Team Team { get; private set; }
    public Material[] RedMaterials;
    public Material[] BlueMaterials;

    public int Health;
    public float AttackRadius = 10f, TurretMoveSpeed = 1f;
    public Transform TurretTransform, CannonTip;
    public Shell Shell;
    public float TimeBetweenShots;

    public TankHealthUI HealthUI;

    Vector3 factoryTarget;

	public void Setup(Vector3 target, Team team)
    {
        Team = team;
        factoryTarget = target;
        GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = team == Team.Red ? RedMaterials : BlueMaterials;

        agent = GetComponent<NavMeshAgent>();
        agent.destination = target;

        if (HealthUI)
        {
            Instantiate(HealthUI, transform, false).Setup(this);
        }

        StartCoroutine(RunAI());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }

    public override void Damage()
    {
        Health--;
        if (Health <= 0)
            Destroy(gameObject);
    }

    IEnumerator RunAI()
    {
        while (true)
        {
            //Pick the best target
            TankTarget target = null;
            while (target == null)
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
                    if (factory != null && factory.CurrentOwner != Team)
                    {
                        target = factory;
                        bestTargetDist = 0f;//factories are the highest priority target
                    }
                }
                yield return null;
            }

            agent.Stop();

            //Rotate turret towards target
            while (target != null)
            {
                Vector3 targetPosition = target.gameObject.transform.position;
                Vector3 targetDelta = (targetPosition - transform.position).normalized;
                Vector3 targetEuler = Quaternion.LookRotation(-targetDelta, Vector3.up).eulerAngles;
                Vector3 currentEuler = TurretTransform.eulerAngles;
                TurretTransform.eulerAngles = new Vector3(
                    Mathf.MoveTowardsAngle(currentEuler.x, targetEuler.x, TurretMoveSpeed * Time.deltaTime), 
                    Mathf.MoveTowardsAngle(currentEuler.y, targetEuler.y, TurretMoveSpeed * Time.deltaTime),
                    0f
                );

                if (Mathf.Abs(targetEuler.y - currentEuler.y) < .1f)
                {
                    break;
                }

                yield return null;
            }
            
            //Fire projectile
            while (target != null)
            {
                Shell shell = Instantiate(Shell, CannonTip.position, CannonTip.rotation);
                shell.Setup(target);
                yield return new WaitForSeconds(TimeBetweenShots);
            }

            agent.Resume();
        }
    }
}
