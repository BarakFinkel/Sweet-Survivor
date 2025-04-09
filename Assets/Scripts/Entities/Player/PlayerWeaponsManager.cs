using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    public static PlayerWeaponsManager instance;
    
    [Header("Elements")]
    [SerializeField] private Transform weaponsParent;
    private Transform[] weaponPositions;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);  
    }

    void Start()
    {
        weaponPositions = weaponsParent.GetComponentsInChildren<Transform>();
    }

    public bool TryAddWeapon(WeaponDataSO weaponData, int weaponLevel)
    {
        foreach (Transform weaponPosition in weaponPositions)
        {
            if (weaponPosition.childCount == 0)
            {
                AssignWeapon(weaponData.Prefab, weaponPosition, weaponLevel);
                return true;
            }
        }

        return false;
    }

    public void AssignWeapon(Weapon weapon, Transform weaponPosition, int weaponLevel)
    {
        Weapon newWeapon = Instantiate(weapon, weaponPosition);
        newWeapon.ConfigureWeapon(weaponLevel);
    }
}
