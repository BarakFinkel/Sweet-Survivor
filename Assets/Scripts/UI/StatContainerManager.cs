using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;

    [Header("Elements")]
    [SerializeField] private StatContainer containerPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);   
    }

    private void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        List<StatContainer> statContainers = new List<StatContainer>();

        foreach(KeyValuePair<Stat, float> pair in statDictionary)
        {
            StatContainer containerInstance = Instantiate(containerPrefab, parent);
            statContainers.Add(containerInstance);

            containerInstance.Configure
            (
                ResourceManager.GetStatIcon(pair.Key),        // Getting the propper icon from the ResourcesManager 
                PlayerStatsManager.FormatStatName(pair.Key),  // Getting the formatted stat name from the PlayerStatsManager
                pair.Value.ToString("F1")                     // Getting the value from the stats' dictionary.
            );
        }

        // Waiting 2 frames before updating the font size.
        StartCoroutine(ResizeTextsWithDelay(statContainers, Time.deltaTime * 2));
    }

    public static void GenerateStatContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        instance.GenerateContainers(statDictionary, parent);
    }

    private IEnumerator ResizeTextsWithDelay(List<StatContainer> statContainers, float delay)
    {
        yield return new WaitForSeconds(delay);
        ResizeTexts(statContainers);
    }

    private void ResizeTexts(List<StatContainer> statContainers)
    {
        float minFontSize = Mathf.Infinity;

        // Finding the smallest font size of all stats containers.
        for (int i = 0; i < statContainers.Count; i++)
        {
            float fontSize = statContainers[i].GetFontSize();

            if(fontSize < minFontSize)
                minFontSize = fontSize;
        }

        for (int i = 0; i < statContainers.Count; i++)
            statContainers[i].SetFontSize(minFontSize);
    }
}
