using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JackPot : MonoBehaviour, IConsumable
{
    [Header ("신화 조합")] [SerializeField] private MythicUnit mythicUnit;
    [Header ("잭팟 유닛이 들어갈 칸")] [SerializeField] private List<Image> jackPotUnitImage = new List<Image>();
    [Header ("잭팟 버튼")] [SerializeField] private Button jackPotBtn;
    [Header ("유닛 핸들 패널 숨기기 버튼")] [SerializeField] private Button hideUnitHandleBtn;
    [Header ("도박 나가기 버튼")] [SerializeField] private Button gambleExitBtn;
    [HideInInspector] public bool isJackPot;

    private void Awake() { amount = 2; }

    // 잭팟 시작
    public void StartJackPot() { StartCoroutine(JackPotCo()); }

    // 잭팟 코루틴
    private IEnumerator JackPotCo()
    {
        // 재화 체크
        if(!ConsumeCurrency()) { SoundManager.instance.SFXPlay(SoundType.NotEnough); yield break; }

        // 버튼 비활성화
        jackPotBtn.interactable = false;

        // 신화 유닛 이미지 채움
        Dictionary<UnitType, int> jackPotUnitMap = new Dictionary<UnitType, int>(); // 잭팟 유닛 맵핑
        for(int i = 0; i < jackPotUnitImage.Count; i++)
        {
            // 신화 유닛 가져옴
            KeyValuePair<UnitType, MythicComb> mythicUnitInfo = GetJackPotUnitInfo();

            // 잭팟 유닛 맵핑
            if(jackPotUnitMap.ContainsKey(mythicUnitInfo.Key)) jackPotUnitMap[mythicUnitInfo.Key]++;
            else jackPotUnitMap.Add(mythicUnitInfo.Key, 1);

            // 잭팟 슬롯 이미지
            jackPotUnitImage[i].sprite = mythicUnitInfo.Value.mythicSprite;
            SoundManager.instance.SFXPlay(SoundType.GetUnit);
            yield return StageManager.instance.halfSecond;
        }

        // 잭팟 결과 처리
        ResultJackPot(jackPotUnitMap);
    }

    // 신화 유닛 정보 반환
    private KeyValuePair<UnitType, MythicComb> GetJackPotUnitInfo() { return mythicUnit.mythicCombMap.ElementAt(Random.Range(0, mythicUnit.mythicCombMap.Count)); }

    // 잭팟 결과 처리
    private void ResultJackPot(Dictionary<UnitType, int> jackPotUnitMap)
    {
        // 버튼 활성화
        jackPotBtn.interactable = true;

        // 모두 다른 신화면 실패
        if(jackPotUnitMap.Count == 3)
        {
            SoundManager.instance.SFXPlay(SoundType.NotEnough);
            MissionManager.instance.gachaFailures++;
            return;
        }

        // 같은 신화가 둘이면 원금 돌려줌
        if(jackPotUnitMap.Count == 2)
        {
            CurrencyManager.instance.AcquireCurrency(amount, false);
            SoundManager.instance.SFXPlay(SoundType.GetUnit);
            return;
        }

        // 같은 신화가 셋이면 신화 소환
        GameObject instantMyth = PoolManager.instance.GetPool(PoolManager.instance.unitPool.queMap, jackPotUnitMap.ElementAt(0).Key);
        GameObject mythPos = mythicUnit.GetUnitPos(jackPotUnitMap.ElementAt(0).Key);
        instantMyth.transform.SetParent(mythPos.transform);
        instantMyth.transform.localPosition = new Vector3(0, 0.2f, 0);
        ++GetUnitBase.CurUnit;
        SoundManager.instance.SFXPlay(SoundType.GetUnit);
    }

    // 재화
    public int amount { get; set; }
    public bool ConsumeCurrency() { return CurrencyManager.instance.ConsumeCurrency(amount, false); }
}
