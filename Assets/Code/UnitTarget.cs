
using UnityEngine;

public abstract class UnitTarget : MonoBehaviour
{
    public Team Team;
    public int Health;

    public void Damage(int amount)
    {
        Health -= amount;
    }
}
