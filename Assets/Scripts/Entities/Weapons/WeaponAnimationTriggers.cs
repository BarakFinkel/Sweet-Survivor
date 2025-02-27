using UnityEngine;

public class WeaponAnimationTriggers : MonoBehaviour
{
    private Weapon weapon => GetComponentInParent<Weapon>();

    private void StartDamageWindowTrigger()
    {
        weapon.EnableDamage();
    }

    private void StopDamageWindowTrigger()
    {
        weapon.DisableDamage();
    }
}