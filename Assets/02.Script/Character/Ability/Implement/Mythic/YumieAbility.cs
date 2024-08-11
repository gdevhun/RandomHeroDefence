using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/유미")]
public class YumieAbility : AsyncAbilityBase, IHiddenAbility
{
    // 초당 500% 데미지, 이동속도 20 감소(3초 유지)
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;

        EnemyBase.DecreaseMoveSpeed += 0.2f;

        for(int i = 0; i < StageManager.instance.instantEnemyList.gameObjectList.Count; i++) StageManager.instance.instantEnemyList.gameObjectList[i].GetComponent<EnemyBase>().TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 5, DamageType.마법);
        yield return oneSecond;
        for(int i = 0; i < StageManager.instance.instantEnemyList.gameObjectList.Count; i++) StageManager.instance.instantEnemyList.gameObjectList[i].GetComponent<EnemyBase>().TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 5, DamageType.마법);
        yield return oneSecond;
        for(int i = 0; i < StageManager.instance.instantEnemyList.gameObjectList.Count; i++) StageManager.instance.instantEnemyList.gameObjectList[i].GetComponent<EnemyBase>().TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 5, DamageType.마법);
        yield return oneSecond;
        
        EnemyBase.DecreaseMoveSpeed -= 0.2f;
    }

    // 히든 스킬
    // 전설 유닛 5개 모으면 자동 소환
    [Header ("히든 스킬 UI 정보")] [SerializeField] private AbilityUiInfo hiddenAbilityUiInfo;
    public AbilityUiInfo HiddenAbilityUiInfo
    {
        get { return hiddenAbilityUiInfo; }
        set { hiddenAbilityUiInfo = value; }
    }
    public void CastHiddenAbility(CharacterBase characterBase) {}
}
