using UnityEngine;
using UnityEngine.Events;

public class FactoryAnimation : MonoBehaviour
{
    public UnityEvent SpawnUnit = new UnityEvent();

    public void DoSpawnAnimation()
    {
        GetComponent<Animator>().SetTrigger("SpawnUnit");
    }

    void OnSpawnUnit()
    {
        SpawnUnit.Invoke();
    }
}
