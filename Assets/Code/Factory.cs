using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Factory : UnitTarget
{
	public Unit[] SpawnPrefabs;
    public Transform SpawnLocation, SpawnEndLocation;
    public Factory TargetFactory;
    public AudioClip SpawnClip;
    public HealthUI HealthUI;
    public GameObject LargeExplosionPrefab;
    public float ExplosionRadius;
    public AudioClip[] ExplosionClips;

    FactoryAnimation factoryAnim;

    void Start()
    {
        factoryAnim = GetComponentInChildren<FactoryAnimation>();
        if (HealthUI)
            Instantiate(HealthUI, transform, false).Setup(this);
    }

    void Update()
    {
        if (Health <= 0)
        {
            int numExplosionClips = ExplosionClips.Length;
            for (int i = 0; i < numExplosionClips; ++i)
            {
                AudioSource.PlayClipAtPoint(ExplosionClips[i], Camera.main.transform.position);
            }
            Destroy(Instantiate(LargeExplosionPrefab, transform.position, transform.rotation), 3f);

            foreach (Collider collider in Physics.OverlapSphere(transform.position, ExplosionRadius))
            {
                UnitTarget target = collider.GetComponent<UnitTarget>();
                if (target != null)
                    target.Damage(DamageType.FactoryExplosion);
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

        Quaternion spawnRot = SpawnLocation.rotation;
        
        Unit newUnit = Instantiate(SpawnPrefabs[unitType], SpawnLocation.position, spawnRot);
        newUnit.Setup(
            FindTargetFactory(),
            Team,
            SpawnEndLocation.position
        );
    }

    public void SpawnUnit(int unitType)
    {
        factoryAnim.DoSpawnAnimation();
        if (SpawnClip != null)
        {
            AudioSource.PlayClipAtPoint(SpawnClip, Camera.main.transform.position);
        }
        StartCoroutine(DoSpawnUnit(unitType));
    }
}
