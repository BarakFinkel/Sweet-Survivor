using UnityEngine;

public class MeleeWeaponAnimationTriggers : MonoBehaviour
{
    private MeleeWeapon weapon => GetComponentInParent<MeleeWeapon>();

    private void StartDamageWindowTrigger() => weapon.EnableDamage();

    private void StopDamageWindowTrigger() => weapon.DisableDamage();

    private void DisableAim() => weapon.DisableAim();

    private void EnableAim() => weapon.EnableAim();
}