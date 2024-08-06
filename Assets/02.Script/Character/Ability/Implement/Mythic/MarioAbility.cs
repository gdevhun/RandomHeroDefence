using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/마리오")]
public class MarioAbility : SyncAbilityBase, IHiddenAbility
{
    private bool isBunker = false, isSoldier = false;

    // 5000% 데미지 총알 1개 발사
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        characterBase.enemyTrans.GetComponent<EnemyBase>().TakeDamage((!isBunker || !isSoldier) ? characterBase.heroInfo.attackDamage * 50 : characterBase.heroInfo.attackDamage * 100);
        characterBase.SetLastBulletPos(instantAbilityEffect, characterBase.enemyTrans, characterBase.gunPointTrans);
        instantAbilityEffect.transform.position += new Vector3(1f, 0, 0);
    }

    // 히든 스킬
    // 벙커와 솔져가 존재하면 10000% 데미지
    [Header ("히든 스킬 UI 정보")] [SerializeField] private AbilityUiInfo hiddenAbilityUiInfo;
    public AbilityUiInfo HiddenAbilityUiInfo
    {
        get { return hiddenAbilityUiInfo; }
        set { hiddenAbilityUiInfo = value; }
    }
    public void CastHiddenAbility(CharacterBase characterBase)
    {
        // 벙커 체크
        isBunker = false;
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.벙커].Count; i++)
        {
            if(GetUnitBase.unitPosMap[UnitType.벙커].ElementAt(i).Key.transform.childCount > 0)
            {
                isBunker = true;
                break;
            }
        }
        if(!isBunker) return;

        // 솔져 체크
        isSoldier = false;
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.솔져].Count; i++)
        {
            if(GetUnitBase.unitPosMap[UnitType.솔져].ElementAt(i).Key.transform.childCount > 0)
            {
                isSoldier = true;
                break;
            }
        }
    }
}
