using UnityEngine;

public static class ResourceManager
{
    // Stats Visuals
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
}
