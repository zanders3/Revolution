
using UnityEngine;

public abstract class TankTarget : MonoBehaviour
{
    public Team Team;
    public int Health;

    public void Damage(int amount)
    {
        Health -= amount;
    }
}
