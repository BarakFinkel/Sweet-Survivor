using TMPro;
using UnityEngine;

public class DamageTextEffect : MonoBehaviour
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
}
