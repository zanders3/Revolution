
using UnityEngine;

public enum DamageType
{
    Gun, Explosion, Shell, ShellExplosion, FactoryExplosion
}

public abstract class UnitTarget : MonoBehaviour
{
    public Team Team;
    public int Health;

    public int GunDamageTaken = 1;
    public int ExplosionDamageTaken = 1;
    public int ShellDamageTaken = 1;
    public int ShellExplosionDamageTaken = 1;
    public int FactoryExplosionDamageTaken = 1000;

    public void Damage(DamageType type)
    {
        if (type == DamageType.Gun)
            Health -= GunDamageTaken;
        else if (type == DamageType.Explosion)
            Health -= ExplosionDamageTaken;
        else if (type == DamageType.Shell)
            Health -= ShellDamageTaken;
        else if (type == DamageType.ShellExplosion)
            Health -= ShellExplosionDamageTaken;
        else if (type == DamageType.FactoryExplosion)
            Health -= FactoryExplosionDamageTaken;
    }
}
