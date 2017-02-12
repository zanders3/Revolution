using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class UnitAnimation : MonoBehaviour
{
    Animator animator;
    
    public bool IsDriving { set { animator.SetBool("Driving", value); } }
    public UnityEvent FireShell = new UnityEvent();

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    void OnFireShell()
    {
        FireShell.Invoke();
    }
}

