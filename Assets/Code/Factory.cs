using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Factory : TankTarget
{
	public Tank[] SpawnPrefabs;
    public Transform SpawnLocation, SpawnEndLocation;
    public Factory TargetFactory;
    
    public TankHealthUI HealthUI;
    public GameObject LargeExplosionPrefab;
    public float ExplosionRadius;

    FactoryAnimation factoryAnim;

    void Start()
    {
        factoryAnim = GetComponentInChildren<FactoryAnimation>();
        if (HealthUI)
            Instantiate(HealthUI, transform, false).Setup(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

    void Update()
    {
        if (Health <= 0)
        {
            Destroy(Instantiate(LargeExplosionPrefab, transform.position, transform.rotation), 3f);

            foreach (Collider collider in Physics.OverlapSphere(transform.position, ExplosionRadius))
            {
                TankTarget target = collider.GetComponent<TankTarget>();
                if (target != null)
                    target.Damage(1000);
            }
            
            Destroy(gameObject);
        }
    }

    public Factory FindTargetFactory()
    {
        if (TargetFactory != null)
            return TargetFactory;

        return GameState.Instance.FindEnemyFactory(Team);
    }

    IEnumerator DoSpawnUnit(int unitType)
    {
        yield return new WaitForSeconds(.27f);

        Tank newUnit = Instantiate(SpawnPrefabs[unitType], SpawnLocation.position, SpawnLocation.rotation);
        newUnit.Setup(
            FindTargetFactory(),
            Team,
            SpawnEndLocation.position
        );
    }

    public void SpawnUnit(int unitType)
    {
        factoryAnim.DoSpawnAnimation();
        StartCoroutine(DoSpawnUnit(unitType));
    }
}
