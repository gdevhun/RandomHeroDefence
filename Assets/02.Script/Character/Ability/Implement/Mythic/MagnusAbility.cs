using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/마그너스")]
public class MagnusAbility : SyncAbilityBase, IHiddenAbility
{
    private bool isMakdus = false, isAlisda = false;

    // 2000% 데미지 최대체력 6% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 20 + ((!isMakdus || !isAlisda) ? enemyBase.maxHp * 0.06f : enemyBase.maxHp * 0.12f), DamageType.물리);
            }
        }
    }

    // 히든 스킬
    // 막더스와 알리스다가 존재하면 최대체력 12% 데미지
    [Header ("히든 스킬 UI 정보")] [SerializeField] private AbilityUiInfo hiddenAbilityUiInfo;
    public AbilityUiInfo HiddenAbilityUiInfo
    {
        get { return hiddenAbilityUiInfo; }
        set { hiddenAbilityUiInfo = value; }
    }
    public void CastHiddenAbility(CharacterBase characterBase)
    {
        // 막더스 체크
        isMakdus = false;
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.막더스].Count; i++)
        {
            if(GetUnitBase.unitPosMap[UnitType.막더스].ElementAt(i).Key.transform.childCount > 0)
            {
                isMakdus = true;
                break;
            }
        }
        if(!isMakdus) return;

        // 알리스다 체크
        isAlisda = false;
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.알리스다].Count; i++)
        {
            if(GetUnitBase.unitPosMap[UnitType.알리스다].ElementAt(i).Key.transform.childCount > 0)
            {
                isAlisda = true;
                break;
            }
        }
    }
}
