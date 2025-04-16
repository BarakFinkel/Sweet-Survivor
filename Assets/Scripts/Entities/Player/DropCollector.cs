using UnityEngine;

[RequireComponent(typeof(Player))]
public class DropCollector : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Collider2D collectorCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we collided with a drop, and the player's own collider (and not one of his weapons) is colliding with it, collect.
        if (collision.TryGetComponent(out Drop drop) && collision.IsTouching(collectorCollider))
        {
            drop.Collect(GetComponent<Player>());
        }
    }
}
