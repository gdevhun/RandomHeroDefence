using UnityEngine;
public class RangeWeapon : MonoBehaviour
{
    [HideInInspector] public WeaponEffect weaponEffect;
    [HideInInspector] public DamageType damageType;
    [Header ("이동 속도")] public float moveSpeed;
    public float attackDamage;
    [HideInInspector] public CharacterBase characterBase;

    // 이동
    private void Update() { transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); }

    // 몬스터 타격
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                    enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(attackDamage), damageType);
                }
            }
            PoolManager.instance.ReturnPool(PoolManager.instance.weaponEffectPool.queMap, gameObject, weaponEffect);
        }
    }

    // 경계 처리
    private void OnTriggerExit2D(Collider2D other) { if(other.gameObject.CompareTag("Border")) PoolManager.instance.ReturnPool(PoolManager.instance.weaponEffectPool.queMap, gameObject, weaponEffect); }
}
