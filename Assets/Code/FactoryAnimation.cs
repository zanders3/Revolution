using UnityEngine;
using UnityEngine.Events;

public class FactoryAnimation : MonoBehaviour
{
    public UnityEvent SpawnUnit = new UnityEvent();

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DoSpawnAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger("SpawnUnit");
            animator.SetTrigger("SpawnUnit");
        }
    }
}
