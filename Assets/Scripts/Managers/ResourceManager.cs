using UnityEngine;

public static class ResourceManager
{
    //////////////////////////////  S T A T S  /////////////////////////////////////

    const string statIconsDataPath = "Data/Stat Icons";
    private static StatIcon[] statIcons;

    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {
            StatIconsSO data = Resources.Load<StatIconsSO>(statIconsDataPath);
            statIcons = data.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
        {
            if(stat == statIcon.stat)
            {
                return statIcon.icon;
            }
        }

        Debug.LogError("No icon found for stat: " + stat);
        return null;
    }

    /////////////////////////////// O B J E C T S //////////////////////////////////

    const string objectDatasPath = "Data/Objects/";    
    private static ObjectDataSO[] objectDatas;

    public static ObjectDataSO[] Objects
    {
        get 
        { 
            if (objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDatasPath); 

            return objectDatas; 
        }
        
        private set
        {}
    }

    public static ObjectDataSO GetRandomObject()
    {
        return Objects[Random.Range(0, Objects.Length)];
    }

    /////////////////////////////// W E A P O N S //////////////////////////////////
    
    const string weaponDatasPath = "Data/Weapons/";    
    private static WeaponDataSO[] weaponDatas;

    public static WeaponDataSO[] Weapons
    {
        get 
        { 
            if (weaponDatas == null)
                weaponDatas = Resources.LoadAll<WeaponDataSO>(weaponDatasPath); 

            return weaponDatas; 
        }
        
        private set
        {}
    }

    public static WeaponDataSO GetRandomWeapon()
    {
        return Weapons[Random.Range(0, Weapons.Length)];
    }
}
