using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/에이든")]
public class AdenAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 일반 유닛 소환 가중치 4 감소(제한 3마리)
        if(UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] == 60) return;
        UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] -= 4;
    }
}