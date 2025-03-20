using System.Collections;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Settings")]
    public bool collected = false;

    public virtual void Collect(Player player)
    {
        if (collected)
            return;
        
        collected = true;
        StartCoroutine(MoveTowardsPlayer(player));
    }

    protected IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0;
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition;

        while (timer < 1)
        {
            targetPosition = player.GetCenterPoint();
            transform.position  = Vector2.Lerp(initialPosition, targetPosition, timer);
            
            timer += Time.deltaTime;
            yield return null;
        }

        HandleCollection();
    }

    protected virtual void HandleCollection()
    {
        gameObject.SetActive(false);
    }
}
