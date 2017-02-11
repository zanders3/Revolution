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
    Vector3 turretTransformEuler;
    TankAnimation tankAnim;

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

        tankAnim = GetComponentInChildren<TankAnimation>();
        turretTransformEuler = TurretTransform.eulerAngles;

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
    
    void LateUpdate()
    {
        tankAnim.IsDriving = agent.velocity.magnitude > .4f;
        if (TurretTransform != null)
            TurretTransform.eulerAngles = turretTransformEuler;
    }

    void MoveTurretToTarget(Vector3 targetEuler)
    {
        Vector3 currentEuler = turretTransformEuler;
        turretTransformEuler = new Vector3(
            Mathf.MoveTowardsAngle(currentEuler.x, targetEuler.x, TurretMoveSpeed * Time.deltaTime),
            Mathf.MoveTowardsAngle(currentEuler.y, targetEuler.y, TurretMoveSpeed * Time.deltaTime),
            0f
        );
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

                if (agent.desiredVelocity.sqrMagnitude > 0.1f)
                    MoveTurretToTarget(Quaternion.LookRotation(-agent.desiredVelocity, Vector3.up).eulerAngles);
                yield return null;
            }

            agent.Stop();

            //Rotate turret towards target
            while (target != null)
            {
                Vector3 targetPosition = target.gameObject.transform.position;
                Vector3 targetDelta = (targetPosition - transform.position).normalized;

                Vector3 targetEuler = Quaternion.LookRotation(-targetDelta, Vector3.up).eulerAngles;
                MoveTurretToTarget(targetEuler);
                if (Mathf.Abs(targetEuler.y - turretTransformEuler.y) < .1f)
                {
                    break;
                }

                yield return null;
            }
            
            //Fire projectile
            while (target != null)
            {
                tankAnim.Shoot();
                tankAnim.FireShell.RemoveAllListeners();
                tankAnim.FireShell.AddListener(() =>
                {
                    if (target != null)
                    {
                        Shell shell = Instantiate(Shell, CannonTip.position, CannonTip.rotation);
                        shell.Setup(target);
                    }
                });
                yield return new WaitForSeconds(TimeBetweenShots);
            }
            
            if (agent.isOnNavMesh)
                agent.Resume();
        }
    }
}
