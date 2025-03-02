#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MenuItems
{
    [MenuItem("Tools/Organize Hierarchy")]
    static void OrganizeHierarchy()
    {
        new UnityEngine.GameObject("--- ENVIRONEMENT ---");
        new UnityEngine.GameObject(" ");

        new UnityEngine.GameObject("--- GAMEPLAY ---");
        new UnityEngine.GameObject(" ");

        new UnityEngine.GameObject("--- UI ---");
        new UnityEngine.GameObject(" ");

        new UnityEngine.GameObject("--- MANAGERS ---");
    }

    [MenuItem("Tools/Reload Scene")]
    static void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

#endif