using System;
using System.Collections;
using UnityEngine;

// 스킬 정보
[Serializable]
public class AbilityInfo
{
    [Header ("스킬 스프라이트")] public Sprite abilitySprite;
    [Header ("스킬 이름")] public string abilityName;
    [Header ("스킬 내용")] public string abilityContent;
}

public abstract class AbilityBase : MonoBehaviour
{
    // 스태미나
    [Header ("최대 스태미나")] [SerializeField] private int maxStamina;
    private int stamina;
    public int Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            Debug.Log("현재 스태미나 :" + stamina);

            // 최대 스태미너가 되고 타겟이 존재 할 때 스킬 시전
            if(stamina >= maxStamina && characterBase.isOnTarget)
            {
                CastAbility();
                stamina = 0;
            }
        }
    }

    private CharacterBase characterBase;
    public AbilityInfo abilityInfo;
    private void Start()
    {
        if(maxStamina > 0) StartCoroutine(StaminaIncreament());
        characterBase = GetComponent<CharacterBase>();
    }

    // 스태미나 증가
    private IEnumerator StaminaIncreament()
    {
        while(true)
        {
            yield return StageManager.instance.oneSecond;
            ++Stamina;  
        }
    }

    // 비활성화 시 초기화
    private void OnDisable()
    {
        Debug.Log("초기화");
        StopAllCoroutines();
        Stamina = 0;
    }

    // 스킬 => 각 캐릭터 마다 오버라이딩 구현
    protected abstract void CastAbility();
}
