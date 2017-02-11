using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : TankTarget
{
    NavMeshAgent agent;
    public Material[] RedMaterials;
    public Material[] BlueMaterials;
    
    public GameObject TankDeathPrefab, TankExplosion;
    public float AttackRadius = 10f, TurretMoveSpeed = 1f, ExplosionRadius = 3f;
    public Transform TurretTransform, CannonTip;
    public Shell Shell;
    public float TimeBetweenShots;
    public int ExplosionDamageAmount = 1;

    public TankHealthUI HealthUI;

    Vector3 turretTransformEuler;
    TankAnimation tankAnim;
    Factory targetFactory;

	public void Setup(Factory targetFactory, Team team, Vector3 spawnTargetLocation)
    {
        this.targetFactory = targetFactory;
        Team = team;
        GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = Team == Team.Red ? RedMaterials : BlueMaterials;

        agent = GetComponent<NavMeshAgent>();

        if (HealthUI)
            Instantiate(HealthUI, transform, false).Setup(this);

        tankAnim = GetComponentInChildren<TankAnimation>();
        turretTransformEuler = TurretTransform.eulerAngles;

        StartCoroutine(RunAI(spawnTargetLocation));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

    void Update()
    {
        if (Health <= 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
            for (int i = 0; i < colliders.Length; i++)
            {
                Tank tank = colliders[i].GetComponent<Tank>();
                if (tank != null)
                {
                    tank.Damage(ExplosionDamageAmount);
                }
            }

            Destroy(Instantiate(TankExplosion, transform.position, transform.rotation), 1.5f);
            GameObject tankDeath = Instantiate(TankDeathPrefab, transform.position, transform.rotation);
            foreach (Transform explosionPiece in tankDeath.transform)
            {
                explosionPiece.transform.parent = null;
                explosionPiece.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, ExplosionRadius);
                explosionPiece.GetComponent<MeshRenderer>().sharedMaterials = Team == Team.Red ? RedMaterials : BlueMaterials;
                Destroy(explosionPiece.gameObject, 4f);
            }
            Destroy(tankDeath);
            Destroy(gameObject);
        }
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

    IEnumerator RunAI(Vector3 postSpawnPosition)
    {
        for (float t = 0f; t<1f; t+=Time.deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, postSpawnPosition, agent.speed * Time.deltaTime);
            yield return null;
        }

        if (targetFactory != null)
            agent.destination = targetFactory.transform.position;

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
                    TankTarget newTarget = colliders[i].GetComponent<TankTarget>();
                    if (newTarget != null && newTarget.Team != Team)
                    {
                        float dist = Vector3.Distance(transform.position, newTarget.transform.position);
                        if (dist < bestTargetDist)
                        {
                            target = newTarget;
                            bestTargetDist = dist;
                        }
                    }
                }

                if (agent.desiredVelocity.sqrMagnitude > 0.1f)
                    MoveTurretToTarget(Quaternion.LookRotation(-agent.desiredVelocity, Vector3.up).eulerAngles);

                if (targetFactory == null)
                {
                    targetFactory = GameState.Instance.FindEnemyFactory(Team);
                    if (targetFactory != null)
                        agent.destination = targetFactory.transform.position;
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

            agent.Resume();
        }
    }
}
