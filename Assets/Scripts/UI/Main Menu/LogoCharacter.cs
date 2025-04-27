using System;
using UnityEngine;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

using Random = UnityEngine.Random;

[Serializable]
public class AnimationData
{
    public string animationName;
    [Range(0.0f, 1.0f)] public float animationProbability;
}

[RequireComponent(typeof(Animator))]
public class LogoCharacter : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float minAnimCooldown = 2.0f;
    [SerializeField] private float maxAnimCooldown = 3.0f;
    [SerializeField] private List<AnimationData> animationDatas;
    private float timer = 0;

    private void Awake()
    {
        if (animationDatas.Count == 0)
        {
            PopulateAnimationNames();
            
            if (animationDatas.Count == 0)
                Debug.LogWarning("Logo warning - no animations assigned!");
        }
        else
        {
            NormalizeAnimationProbabilities();
            timer = Random.Range(minAnimCooldown, maxAnimCooldown);
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            if (animationDatas.Count == 0)
                return;

            string randomAnimName = PickRandomAnimation();
            animator.SetTrigger(randomAnimName);
            timer = Random.Range(minAnimCooldown, maxAnimCooldown);
        }
    }

    /// <summary>
    /// Manually repopulate the animation names based on the animator's current triggers.
    /// </summary>
    public void PopulateAnimationNames()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned!");
            return;
        }

        RemoveMissingAnimationNames();
        AddMissingAnimationNames();
    }

    /// <summary>
    /// Removing animation names that don't exist in the Animator's trigger parameters.
    /// </summary>
    public void RemoveMissingAnimationNames()
    {
        // Go through the existing list and check if any animations are no longer valid
        for (int i = animationDatas.Count - 1; i >= 0; i--)
        {
            string animationName = animationDatas[i].animationName;
            if (!HasTrigger(animationName))
            {
                Debug.LogWarning($"Animation trigger '{animationName}' not found in animator. Removing it from the list.");
                animationDatas.RemoveAt(i); // Remove invalid animation
            }
        }
    }

    /// <summary>
    /// Checks if the Animator has a trigger with the given name.
    /// </summary>
    /// <param name="triggerName">The trigger name to be checked for it's existence in the Animator's parameters</param>
    /// <returns></returns>
    private bool HasTrigger(string triggerName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Adding animation names that are missing from the Animation Names' list.
    /// </summary>
    public void AddMissingAnimationNames()
    {
        // Now, check for animations in the Animator and add any new ones
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                // Check if the animation already exists in the list
                if (!HasAnimationName(param.name))
                {
                    // Add new animation
                    animationDatas.Add(new AnimationData { animationName = param.name, animationProbability = 0.1f });
                    Debug.Log($"Added new animation: {param.name}");
                }
            }
        }
    }

    /// <summary>
    /// Checks if the animationDatas list has an animation name with the with the given trigger's parameter name.
    /// </summary>
    /// <param name="animationName">The animation name to be checked for it's existence in animationDatas.</param>
    /// <returns></returns>
    private bool HasAnimationName(string animationName)
    {
        foreach (AnimationData data in animationDatas)
        {
            if (animationName == data.animationName)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Picks a random animation name from the AnimationDatas list.
    /// </summary>
    /// <returns>The name of the animation to play.</returns>
    private string PickRandomAnimation()
    {
        float rand = Random.value; // Random value between 0 and 1
        float cumulative = 0.0f;

        foreach (AnimationData animData in animationDatas)
        {
            cumulative += animData.animationProbability;

            if (rand <= cumulative)
                return animData.animationName;
        }

        // Fallback (shouldn't happen if normalized correctly)
        return animationDatas[0].animationName;
    }

    /// <summary>
    /// Normalizes the animation probabilites so that their sum equals 1.
    /// This is used to ensure the chances are consistent.
    /// </summary>
    public void NormalizeAnimationProbabilities()
    {
        float sum = 0.0f;

        foreach (AnimationData animData in animationDatas)
            sum += animData.animationProbability;

        if (sum > 0)
        {
            for (int i = 0; i < animationDatas.Count; i++)
                animationDatas[i].animationProbability /= sum;
        }
    }

    #if UNITY_EDITOR
    
    [CustomEditor(typeof(LogoCharacter))]
    public class LogoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LogoCharacter logo = (LogoCharacter)target;

            // Draw default properties
            DrawDefaultInspector();

            // Add button to populate the animation names
            if (GUILayout.Button("Populate Animation Names"))
            {
                logo.PopulateAnimationNames(); // Call the method to populate the animation list
            }

            // Add button to normalize animation probabilities
            if (GUILayout.Button("Normalize Animation Probabilities"))
            {
                logo.NormalizeAnimationProbabilities();
                EditorUtility.SetDirty(logo); // Mark as dirty to make sure changes are saved
            }
        }
    }

    #endif
}
