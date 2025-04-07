using UnityEngine;

public static class ResourceManager
{
    // Stats Visuals
    const string statIconsDataPath = "Data/Stat Icons";
    private static StatIcon[] statIcons;

    // Objects
    const string objectDatasPath = "Data/Objects/";    
    private static ObjectDataSO[] objectDatas;

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

    public static ObjectDataSO[] Objects
    {
        get 
        { 
            if (objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDatasPath); 

            Debug.Log(objectDatas.Length);

            return objectDatas; 
        }
        
        private set
        {}
    }
}
