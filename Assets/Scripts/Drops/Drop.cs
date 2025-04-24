using System.Collections;
using UnityEngine;

/// <summary>
/// An object created to be collected by the player and have varying effects when collected.
/// Serves as a base class to different drop and shouln't be created as it's own instance.
/// </summary>
public class Drop : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] AudioClip collectSound;
    public bool collected = false;

    /// <summary>
    /// Responsible for handling the collection logic.
    /// </summary>
    /// <param name="player">The player that collected the drop.</param>
    public virtual void Collect(Player player)
    {
        if (collected)
            return;
        
        collected = true;
        StartCoroutine(MoveTowardsPlayer(player));
    }

    /// <summary>
    /// Activates upon collection, and moves the drop to the player within 1 second.
    /// Upon the end of the time interval, will execute the collection handlement.
    /// </summary>
    /// <param name="player">The player that collected the drop.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Executes logic for post-collection of the drop.
    /// </summary>
    protected virtual void HandleCollection()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Plays the corresponding sound assigned to the drop.
    /// </summary>
    protected void PlayCollectSound()
    {
        if (!AudioManager.instance.IsSFXOn)
            return;
        
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }
}
