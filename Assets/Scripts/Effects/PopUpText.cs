using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro damageText;

    public void SetupDamageText(Transform enemy, Vector3 spawnOffset, int damage, bool isCritHit)
    {
        transform.parent = enemy;
        transform.position = enemy.position + spawnOffset;

        damageText.text = damage.ToString();
        damageText.color = isCritHit ? Color.yellow : Color.white;

        animator.Play("effect");
    }

    public void SetupDodgeText(Transform player, Vector3 spawnOffset)
    {
        transform.parent = player;
        transform.position = player.position + spawnOffset;

        damageText.text = "Dodge!";
        damageText.color = Color.gray;

        animator.Play("effect");
    }
}
