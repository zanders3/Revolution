using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class TankAnimation : MonoBehaviour
{
    Animator animator;
    
    public bool IsDriving { set { animator.SetBool("Driving", value); } }
    public UnityEvent FireShell = new UnityEvent();

    void Start()
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

