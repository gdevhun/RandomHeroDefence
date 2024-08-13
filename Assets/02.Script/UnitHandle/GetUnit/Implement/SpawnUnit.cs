using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnUnit : GetUnitBase, IConsumable
{
    // 소환에서 등급에 따른 가중치 설정
    public Dictionary<HeroGradeType, int> gradeWeightMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.일반, 72 },
        { HeroGradeType.고급, 24 },
        { HeroGradeType.희귀, 6 },
        { HeroGradeType.전설, 1 }
    };

    // 소환 구체화
    public override void GetUnitHandle()
    {
        // 재화 체크
        if(!ConsumeCurrency()) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }
        ++SpawnCnt;

        // 최대 유닛 체크
        if(CurUnit >= maxUnit) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 랜덤 유닛
        GameObject instantUnit = GetUnit(gradeWeightMap);

        // 스폰 위치
        GameObject unitPos = GetUnitPos(instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);

        // 스폰 위치 체크
        if(unitPos == null)
        {
            PoolManager.instance.ReturnPool(PoolManager.instance.unitPool.queMap, instantUnit, instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);
            SoundManager.instance.SFXPlay(SoundType.NotEnough);
            return;
        }

        // 유닛 소환
        instantUnit.transform.SetParent(unitPos.transform);
        instantUnit.transform.localPosition = new Vector3(unitPos.transform.childCount == 3 ? 0.1f : 0.2f * (unitPos.transform.childCount - 1), unitPos.transform.childCount == 3 ? 0 : 0.2f, -0.1f * (unitPos.transform.childCount - 1));
        ++CurUnit;
    }

    // 재화
    private int spawnCnt;
    public int SpawnCnt
    {
        get { return spawnCnt; } 
        set
        {
            spawnCnt = value;
            amount = 10 + spawnCnt;
            UpdateSpawnGoldUI();
        }
    }
    public int amount { get; set; }
    public bool ConsumeCurrency() { return CurrencyManager.instance.ConsumeCurrency(amount, true); }
    [Header ("소환 골드 텍스트")] [SerializeField] private TextMeshProUGUI spawnGoldText;
    private void UpdateSpawnGoldUI() { spawnGoldText.text = amount.ToString(); }
}
