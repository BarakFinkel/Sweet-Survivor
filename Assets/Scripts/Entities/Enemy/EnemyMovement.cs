using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;
    
    [Header("General Settings")]
    [SerializeField] private float moveSpeed;
    private bool canMove = false;

    void Update()
    {
        if (player != null && canMove)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }

    public void SetPlayer(Player _player)
    {
        this.player = _player;
    }

    public void EnableMovement()
    {
        if (canMove == false)
            canMove = true;
    }

    public void DisableMovement()
    {
        if (canMove == true)
            canMove = false;
    }
}
