using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Abstract base class for a drop pool. Manages the logic for retrieving and releasing drops of any type.
/// </summary>
public abstract class DropPoolBase
{
    /// <summary>
    /// Gets an instance of the drop object from the pool.
    /// </summary>
    /// <returns>An instance of a Drop object.</returns>
    public abstract Drop Get();

    /// <summary>
    /// Releases a drop back to the pool for future use.
    /// </summary>
    /// <param name="drop">The drop object to release back into the pool.</param>  
    public abstract void Release(Drop drop);

    /// <summary>
    /// Subscribes to events (such as when a drop is collected) to manage the pool state.
    /// </summary>
    protected abstract void SubscribeToEvent();

    /// <summary>
    /// Unsubscribes from the events to prevent memory leaks when the pool is no longer in use.
    /// </summary>
    public abstract void UnsubscribeFromEvent();
}

public class DropPool<T> : DropPoolBase where T : Drop
{
    private ObjectPool<Drop> pool; // The object pool that handles the drop instances.
    private Drop prefab;           // The prefab for the drop object to instantiate.
    private Transform parent;      // The parent transform under which new drops are instantiated.

    /// <summary>
    /// Constructor to initialize the drop pool.
    /// </summary>
    /// <param name="dropPrefab">The prefab to be used for drops.</param>
    /// <param name="poolParent">The parent transform under which drops will be instantiated.</param>
    public DropPool(T dropPrefab, Transform poolParent)
    {
        prefab = dropPrefab;
        parent = poolParent;

        pool = new ObjectPool<Drop>
        ( 
            createFunc:      ()     => MonoBehaviour.Instantiate(prefab, parent),                    // Instantiates a drop object.
            actionOnGet:     (drop) => {drop.collected = false; drop.gameObject.SetActive(true);},   // Sets the drop collect flag to false, and activates it's gameObject.
            actionOnRelease: (drop) => {drop.collected = true; drop.gameObject.SetActive(false);},   // Sets the drop collect flag to true, and deactivates it's gameObject.
            actionOnDestroy: (drop) => MonoBehaviour.Destroy(drop.gameObject)                        // Destroys the drop object.
        );

        SubscribeToEvent();
    }

    public override Drop Get() => pool.Get();
    
    public override void Release(Drop drop) => pool.Release(drop);

    /// <summary>
    /// Dynamically subscribes to the static onCollect event of the drop type.
    /// </summary>
    protected override void SubscribeToEvent() => HandleEventSubscription(true);

    /// <summary>
    /// Dynamically unsubscribes to the static onCollect event of the drop type.
    /// </summary>
    public override void UnsubscribeFromEvent() => HandleEventSubscription(false);

    /// <summary>
    /// Subscribes to or unsubscribes from the static onCollect event of the drop type.
    /// This event triggers when a drop is collected, and the pool releases the drop back for reuse.
    /// Uses reflection to attach or detach the event handler dynamically.
    /// </summary>
    /// <param name="toSubscribe">True to subscribe, false to unsubscribe.</param>
    public void HandleEventSubscription(bool toSubscribe)
    {
        // Use reflection to get the static field for the event
        FieldInfo field = typeof(T).GetField("onCollect", BindingFlags.Public | BindingFlags.Static);

        // If we indeed found the field of the corresponding type
        if (field != null && field.FieldType == typeof(Action<T>))
        {
            // Get the static event and unsubscribe the Release() method using reflection
            Action<T> eventHandler = (Action<T>)field.GetValue(null);

            if (toSubscribe)
                eventHandler += Release;
            else
                eventHandler -= Release;

            field.SetValue(null, eventHandler);
        }
        else
        {
            Debug.LogError("Event Error: Failed to find or match the event type - " + field.FieldType);
        }
    }
}