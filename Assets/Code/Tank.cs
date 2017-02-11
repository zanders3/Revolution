using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : MonoBehaviour
{
    NavMeshAgent agent;
    public Team Team { get; private set; }
    public Material[] RedMaterials;
    public Material[] BlueMaterials;

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
