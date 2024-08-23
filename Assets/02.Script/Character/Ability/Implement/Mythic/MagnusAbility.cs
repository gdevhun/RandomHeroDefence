using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/마그너스")]
public class MagnusAbility : SyncAbilityBase, IHiddenAbility
{
    private bool isViking = false, isAlisda = false;

    // 2000% 데미지 최대체력 2% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;

        CastHiddenAbility(characterBase);        
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 2f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 20 + ((!isViking || !isAlisda) ? enemyBase.maxHp * 0.04f : enemyBase.maxHp * 0.08f), characterBase.heroInfo.damageType);
            }
        }
    }

    // 히든 스킬
    // 바이킹와 알리스다가 존재하면 최대체력 4% 데미지
    [Header ("히든 스킬 UI 정보")] [SerializeField] private AbilityUiInfo hiddenAbilityUiInfo;
    public AbilityUiInfo HiddenAbilityUiInfo
    {
        get { return hiddenAbilityUiInfo; }
        set { hiddenAbilityUiInfo = value; }
    }
    public void CastHiddenAbility(CharacterBase characterBase)
    {
        // 바이킹 체크
        isViking = false;
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.바이킹].Count; i++)
        {
            if(GetUnitBase.unitPosMap[UnitType.바이킹].ElementAt(i).Key.transform.childCount > 0)
            {
                isViking = true;
                break;
            }
        }
        if(!isViking) return;

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

        // 히든 활성화
        if(!isAlisda) return;
        if(!MissionManager.instance.mythicHiddenAbilityActivateMap.ContainsKey(UnitType.마그너스)) MissionManager.instance.mythicHiddenAbilityActivateMap.Add(UnitType.마그너스, 1);
    }
}
