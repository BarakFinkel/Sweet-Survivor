using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    public static PlayerWeaponsManager instance;
    
    [Header("Elements")]
    [SerializeField] private Transform weaponsParent;
    private Transform[] weaponPositions;
    public static Action onWeaponsChanged;

    int numberOfEquippedWeapons = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        WeaponMerger.onMerge += MergeWeaponCallback;
    }

    void Start()
    {
        weaponPositions = new Transform[weaponsParent.childCount];
        
        for (int i = 0; i < weaponsParent.childCount; i++)
            weaponPositions[i] = weaponsParent.GetChild(i);
    }

    private void OnDisable()
    {
        WeaponMerger.onMerge -= MergeWeaponCallback;
    }

    public bool TryAddWeapon(WeaponDataSO weaponData, int weaponLevel)
    {
        foreach (Transform weaponPosition in weaponPositions)
        {
            if (weaponPosition.childCount == 0)
            {
                AssignWeapon(weaponData.Prefab, weaponPosition, weaponLevel);
                onWeaponsChanged?.Invoke();

                return true;
            }
        }

        return false;
    }

    private void AssignWeapon(Weapon weapon, Transform weaponPosition, int weaponLevel)
    {
        Weapon newWeapon = Instantiate(weapon, weaponPosition);
        newWeapon.ConfigureWeapon(weaponLevel);

        numberOfEquippedWeapons++;
    }

    public void MeltWeapon(int index)
    {
        Weapon meltWeapon = weaponPositions[index].GetComponentInChildren<Weapon>();
        
        if (meltWeapon == null)
            return;

        CurrencyManager.instance.AddCurrency
        (
            CurrencyType.Normal,
            WeaponStatsCalculator.GetRecycleWorth(meltWeapon.WeaponData, meltWeapon.level)
        );
        
        Destroy(meltWeapon.gameObject);
        numberOfEquippedWeapons--;
        onWeaponsChanged?.Invoke();
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();

        for (int i = 0; i < weaponPositions.Length; i++)
        {
            Weapon weapon = weaponPositions[i].GetComponentInChildren<Weapon>();
            
            if (weapon != null)
                weapons.Add(weapon);
            else
                weapons.Add(null);
        }

        return weapons.ToArray();
    }

    public int GetNumberOfWeapons() => numberOfEquippedWeapons;

    private void MergeWeaponCallback(Weapon weapon)
    {
        numberOfEquippedWeapons--;
    }
}
