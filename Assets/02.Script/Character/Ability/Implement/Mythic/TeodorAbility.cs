using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/테오도르")]
public class TeodorAbility : SyncAbilityBase, IHiddenAbility
{
    private bool isAden = false, isEkion = false;

    // 1000% 데미지, 2초 스턴
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
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 10, DamageType.마법);
                enemyBase.SetStunTime += !isAden || !isEkion ? 2f : 3f;
            }
        }
    }

    // 히든 스킬
    // 에이든과 에키온이 존재하면 3초 스턴
    [Header ("히든 스킬 UI 정보")] [SerializeField] private AbilityUiInfo hiddenAbilityUiInfo;
    public AbilityUiInfo HiddenAbilityUiInfo
    {
        get { return hiddenAbilityUiInfo; }
        set { hiddenAbilityUiInfo = value; }
    }
    public void CastHiddenAbility(CharacterBase characterBase)
    {
        // 에이든 체크
        isAden = false;
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.에이든].Count; i++)
        {
            if(GetUnitBase.unitPosMap[UnitType.에이든].ElementAt(i).Key.transform.childCount > 0)
            {
                isAden = true;
                break;
            }
        }
        if(!isAden) return;

        // 에키온 체크
        isEkion = false;
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.에키온].Count; i++)
        {
            if(GetUnitBase.unitPosMap[UnitType.에키온].ElementAt(i).Key.transform.childCount > 0)
            {
                isEkion = true;
                break;
            }
        }

        // 히든 활성화
        if(!isEkion) return;
        if(!MissionManager.instance.mythicHiddenAbilityActivateMap.ContainsKey(UnitType.테오도르)) MissionManager.instance.mythicHiddenAbilityActivateMap.Add(UnitType.테오도르, 1);
    }
}
