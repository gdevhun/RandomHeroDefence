using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/에이든")]
public class AdenAbility : SyncAbilityBase
{
    // 일반 유닛 소환 가중치 4 감소
    public override void CastAbility(CharacterBase characterBase)
    {
        if(UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] == 0) return;
        UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] -= 4;
    }
}
