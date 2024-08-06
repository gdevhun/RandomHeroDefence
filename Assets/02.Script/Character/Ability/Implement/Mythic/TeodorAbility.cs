using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/테오도르")]
public class TeodorAbility : AsyncAbilityBase, IHiddenAbility
{
    private bool isAden = false, isEkion = false;

    // 1.5초 스턴
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;

        CastHiddenAbility(characterBase);
        
        // 둘 중 하나라도 존재하지 않는 경우
        if(!isAden || !isEkion)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                    enemyBase.moveSpeed = 0;
                }
            }
            yield return oneSecond;
            yield return halfSecond;
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    hit.GetComponent<EnemyBase>().moveSpeed = hit.GetComponent<EnemyBase>().originMoveSpeed;
                }
            }
        }
        else // 에이든과 에키온이 존재하는 경우
        {
            for(int i = 0; i < StageManager.instance.instantEnemyList.gameObjectList.Count; i++)
            {
                EnemyBase enemyBase = StageManager.instance.instantEnemyList.gameObjectList[i].GetComponent<EnemyBase>();
                enemyBase.moveSpeed = 0;
            }
            yield return oneSecond;
            yield return halfSecond;
            for(int i = 0; i < StageManager.instance.instantEnemyList.gameObjectList.Count; i++)
            {
                StageManager.instance.instantEnemyList.gameObjectList[i].GetComponent<EnemyBase>().moveSpeed = StageManager.instance.instantEnemyList.gameObjectList[i].GetComponent<EnemyBase>().originMoveSpeed;
            }
        }

        PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantAbilityEffect, abilityEffectType);
    }

    // 히든 스킬
    // 에이든과 에키온이 존재하면 필드의 모든 몬스터 스턴
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
    }
}
