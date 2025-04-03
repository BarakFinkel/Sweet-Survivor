using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform weaponsParent;
    private Transform[] weaponPositions;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponPositions = weaponsParent.GetComponentsInChildren<Transform>();
    }

    public void TryAddWeapon(WeaponDataSO weaponData, int weaponLevel)
    {
        foreach (Transform weaponPosition in weaponPositions)
        {
            if (weaponPosition.childCount == 0)
            {
                AssignWeapon(weaponData.Prefab, weaponPosition, weaponLevel);
                break;
            }
        }
    }

    public void AssignWeapon(Weapon weapon, Transform weaponPosition, int weaponLevel)
    {
        Weapon newWeapon = Instantiate(weapon, weaponPosition);
        newWeapon.ConfigureWeapon(weaponLevel);
    }
}
