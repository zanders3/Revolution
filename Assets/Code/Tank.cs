using UnityEngine;
using UnityEngine.AI;

public enum TankState
{
    Moving,
    Attacking
}

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : MonoBehaviour
{
    NavMeshAgent agent;
    public Team Team { get; private set; }
    public Material[] RedMaterials;
    public Material[] BlueMaterials;

    public float AttackRadius = 10f;

	public void Setup(Vector3 target, Team team)
    {
        GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = team == Team.Red ? RedMaterials : BlueMaterials;

        agent = GetComponent<NavMeshAgent>();
        agent.destination = target;
    }

    void Update()
    {
        
    }
}
