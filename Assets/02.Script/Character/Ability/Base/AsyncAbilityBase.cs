using System.Collections;
using UnityEngine;

// 비동기 스킬
public abstract class AsyncAbilityBase : AbilityBase
{
    protected WaitForSeconds oneSecond = new WaitForSeconds(1f);
    protected WaitForSeconds halfSecond = new WaitForSeconds(0.5f);
    public abstract IEnumerator CastAbility(CharacterBase characterBase);
}
