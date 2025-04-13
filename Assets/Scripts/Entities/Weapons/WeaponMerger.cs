using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    public static WeaponMerger instance;

    [Header("Settings")]
    private List<Weapon> weaponsToMerge = new List<Weapon>();

    [Header("Actions")]
    public static Action<Weapon> onMerge;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);   
    }

    public bool CanMerge(Weapon weapon)
    {
        if (weapon.level == 4)
            return false;

        Weapon[] playerWeapons = PlayerWeaponsManager.instance.GetWeapons();

        foreach (Weapon playerWeapon in playerWeapons)
        {
            if (
                playerWeapon == null 
                || playerWeapon == weapon 
                || playerWeapon.WeaponData.Name != weapon.WeaponData.Name 
                || playerWeapon.level != weapon.level
            )
            {
                continue;
            }

            weaponsToMerge.Add(weapon);
            weaponsToMerge.Add(playerWeapon);

            return true;
        }

        return false;
    }

    public void Merge()
    {
        if (weaponsToMerge.Count < 2)
        {
            Debug.LogError("WeaponMerger error: Less than 2 weapons found, cannot merge.");
            return;
        }

        DestroyImmediate(weaponsToMerge[1].gameObject);
        weaponsToMerge[0].Upgrade();
        
        Weapon upgradedWeapon = weaponsToMerge[0];
        onMerge?.Invoke(upgradedWeapon);

        weaponsToMerge.Clear();
    }
}
