using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : UnitTarget
{
    NavMeshAgent agent;
    public Material[] RedMaterials;
    public Material[] BlueMaterials;
    
    public GameObject TankDeathPrefab, TankExplosion, FireGunPrefab;
    public float AttackRadius = 10f, TurretMoveSpeed = 1f, ExplosionRadius = 3f;
    public int Cost = 20;
    public Transform TurretTransform, CannonTip;
    public Shell Shell;
    public float TimeBetweenShots;
    public int GunDamageAmount = 1;
    public int ExplosionDamageAmount = 1;

    public HealthUI HealthUI;
    public bool UseTurretRotationOffset = false;
    public Vector3 RotationOffset;

    bool hasGotTurretTransform = false;
    Vector3 turretTransformEuler, rotationOffset;
    UnitAnimation unitAnim;
    Factory targetFactory;

	public void Setup(Factory targetFactory, Team team, Vector3 spawnTargetLocation)
    {
        this.targetFactory = targetFactory;
        Team = team;
        GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = Team == Team.Red ? RedMaterials : BlueMaterials;

        agent = GetComponent<NavMeshAgent>();

        if (HealthUI)
            Instantiate(HealthUI, transform, false).Setup(this);

        unitAnim = GetComponentInChildren<UnitAnimation>();

        StartCoroutine(RunAI(spawnTargetLocation));
    }

    Ray lastGunRay;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(lastGunRay);
    }

    void Update()
    {
        if (Health <= 0)
        {
            if (ExplosionRadius > 0f)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    Unit tank = colliders[i].GetComponent<Unit>();
                    if (tank != null)
                    {
                        tank.Damage(ExplosionDamageAmount);
                    }
                }
            }

            if (TankExplosion != null)
                Destroy(Instantiate(TankExplosion, transform.position, transform.rotation), 1.5f);
            if (TankDeathPrefab != null)
            {
                GameObject tankDeath = Instantiate(TankDeathPrefab, transform.position, transform.rotation);
                foreach (Transform explosionPiece in tankDeath.transform)
                {
                    explosionPiece.transform.parent = null;
                    explosionPiece.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, ExplosionRadius);
                    explosionPiece.GetComponent<MeshRenderer>().sharedMaterials = Team == Team.Red ? RedMaterials : BlueMaterials;
                    Destroy(explosionPiece.gameObject, 4f);
                }
                Destroy(tankDeath);
            }
            Destroy(gameObject);
        }
    }
    
    void LateUpdate()
    {
        if (agent != null)
            unitAnim.IsDriving = agent.velocity.magnitude > .4f;
        if (hasGotTurretTransform)
        {
            TurretTransform.eulerAngles = turretTransformEuler + rotationOffset + RotationOffset;
            hasGotTurretTransform = true;
        }
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

        if (UseTurretRotationOffset)
            rotationOffset = TurretTransform.eulerAngles;
        turretTransformEuler = TurretTransform.eulerAngles;
        hasGotTurretTransform = true;

        if (targetFactory != null)
            agent.destination = targetFactory.transform.position;

        while (true)
        {
            //Pick the best target
            UnitTarget target = null;
            while (target == null)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRadius);
                float bestTargetDist = float.MaxValue;
                for (int i = 0; i < colliders.Length; i++)
                {
                    UnitTarget newTarget = colliders[i].GetComponent<UnitTarget>();
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
                    MoveTurretToTarget(Quaternion.LookRotation(UseTurretRotationOffset ? agent.desiredVelocity : -agent.desiredVelocity, Vector3.up).eulerAngles);

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

                Vector3 targetEuler = Quaternion.LookRotation(UseTurretRotationOffset ? targetDelta : -targetDelta, Vector3.up).eulerAngles;
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
                if (FireGunPrefab != null)
                {
                    Destroy(Instantiate(FireGunPrefab, CannonTip.position, Quaternion.LookRotation(-CannonTip.forward, Vector3.up)), 2f);
                }

                unitAnim.Shoot();
                unitAnim.FireShell.RemoveAllListeners();
                unitAnim.FireShell.AddListener(() =>
                {
                    if (target != null)
                    {
                        Shell shell = Instantiate(Shell, CannonTip.position, CannonTip.rotation);
                        shell.Setup(target);
                    }
                });
                unitAnim.FireGun.RemoveAllListeners();
                unitAnim.FireGun.AddListener(() =>
                {
                    if (target != null)
                    {
                        lastGunRay = new Ray(CannonTip.position, (target.transform.position - CannonTip.position).normalized);
                        foreach (RaycastHit hit in Physics.RaycastAll(lastGunRay))
                        {
                            UnitTarget hitTarget = hit.collider.GetComponent<UnitTarget>();
                            if (hitTarget != null && hitTarget.Team != Team)
                            {
                                hitTarget.Damage(GunDamageAmount);
                                break;
                            }
                        }
                    }
                });
                yield return new WaitForSeconds(TimeBetweenShots);
            }

            agent.Resume();
        }
    }
}
