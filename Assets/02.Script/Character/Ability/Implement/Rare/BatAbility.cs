using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/배트")]
public class BatAbility : SyncAbilityBase
{
    // 모든 몬스터 마방 20 감소
    public override void CastAbility(CharacterBase characterBase) { EnemyBase.DecreaseMagDef += 20f; }
}
