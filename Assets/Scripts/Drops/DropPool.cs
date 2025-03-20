using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

public abstract class DropPoolBase
{
    public abstract Drop Get();
    public abstract void Release(Drop drop);
    protected abstract void SubscribeToEvent();
    public abstract void UnsubscribeFromEvent();
}

public class DropPool<T> : DropPoolBase where T : Drop
{
    private ObjectPool<Drop> pool;
    private Drop prefab;
    private Transform parent;

    private static Action<T> onCollectEvent;

    public DropPool(T dropPrefab, Transform poolParent)
    {
        prefab = dropPrefab;
        parent = poolParent;

        pool = new ObjectPool<Drop>( 
            createFunc: () => MonoBehaviour.Instantiate(prefab, parent),
            actionOnGet: (obj) => {obj.collected = false; obj.gameObject.SetActive(true);},
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => MonoBehaviour.Destroy(obj.gameObject)
        );

        SubscribeToEvent();
    }

    public override Drop Get() => pool.Get();
    
    public override void Release(Drop obj) => pool.Release(obj);

    protected override void SubscribeToEvent()
    {
        // Use reflection to get the static field for the event
        FieldInfo field = typeof(T).GetField("onCollect", BindingFlags.Public | BindingFlags.Static);
        
        // If we indeed found the field of the corresponding type
        if (field != null && field.FieldType == typeof(Action<T>))
        {
            // Get the static event and subscribe the Release() method using reflection
            Action<T> eventHandler = (Action<T>)field.GetValue(null);
            eventHandler += Release;
            field.SetValue(null, eventHandler); // Subscribe to the static event
        }
        else
        {
            Debug.LogError("Event Error: Failed to find or match the event type." + field.FieldType);
        }
    }

    public override void UnsubscribeFromEvent()
    {
        // Use reflection to get the static field for the event
        FieldInfo field = typeof(T).GetField("onCollect", BindingFlags.Public | BindingFlags.Static);
        
        // If we indeed found the field of the corresponding type
        if (field != null && field.FieldType == typeof(Action<T>))
        {
            // Get the static event and unsubscribe the Release() method using reflection
            Action<T> eventHandler = (Action<T>)field.GetValue(null);
            eventHandler -= Release;
            field.SetValue(null, eventHandler);
        }
        else
        {
            Debug.LogError("Event Error: Failed to find or match the event type - " + field.FieldType);
        }
    }
}