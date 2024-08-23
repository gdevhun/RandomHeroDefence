using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 신화 조합식
[Serializable]
public class MythicComb
{ 
    [Header ("조합 할 신화 유닛")] public UnitType mythicType;
    [Header ("조합 할 신화 유닛 스프라이트")] public Sprite mythicSprite;
    [Header ("필요한 유닛과 수")] public List<UnitRequire> requireList;
}
[Serializable]
public class UnitRequire
{
    [Header ("필요한 유닛 타입")] public UnitType unitType;
    [Header ("필요한 유닛 등급")] public HeroGradeType gradeType;
    [Header ("필요한 유닛 스프라이트")] public Sprite unitSprite;
    [Header ("필요한 유닛 수")] public int cnt; // 유닛 수 => 같은 유닛 3 개 필요 시 1, 2, 3 할당 => ex) Hunter 3 개 필요 시 Hunter 1, Hunter 2, Hunter 3
}

public class MythicUnit : GetUnitBase
{
    public static MythicUnit instance;
    [Header ("신화 조합식")] public List<MythicComb> mythicCombList = new List<MythicComb>();
    public Dictionary<UnitType, MythicComb> mythicCombMap = new Dictionary<UnitType, MythicComb>(); // 신화 조합식 맵핑
    private UnitType selectedMythic; // 소환 할 신화
    public UnitType SelectedMythic
    {
        get { return selectedMythic; }
        set { selectedMythic = value; UpdateMythicInfoUI(value); } 
    }
    [Header ("신화 이미지")] [SerializeField] private Image mythicImage;
    [Header ("신화 이름 텍스트")] [SerializeField] private TextMeshProUGUI mythicText;
    [Header ("필요 유닛 이미지")] public List<Image> requireImageList = new List<Image>();
    public Dictionary<HeroGradeType, Color> gradeColorMap = new Dictionary<HeroGradeType, Color>(); // 등급, 오라 색 맵핑
    [Header ("오라 색")] [SerializeField] private List<Color> circleColorList = new List<Color>();
    [Header ("신화 조합 가능 체크 이미지")] public ListGameObject mythicCombCheckList = new ListGameObject();
    [Header ("신화 조합 가능 개수 표시")] public TextMeshProUGUI mythicCombCheckCnt;

    // 신화 조합식 맵핑
    // 등급 오라 색 맵핑
    private void Awake()
    {
        instance = this;
        for(int i = 0; i < mythicCombList.Count; i++) mythicCombMap.Add(mythicCombList[i].mythicType, mythicCombList[i]);
        for(int i = 0; i < circleColorList.Count; i++) gradeColorMap.Add((HeroGradeType)i, circleColorList[i]);
    }

    // 소환 할 신화 소환
    public override GameObject GetUnit(Dictionary<HeroGradeType, int> gradeWeightMap) { return GetUnitFromPool(HeroGradeType.신화); }

    // 소환 할 신화 풀링
    protected override GameObject GetUnitFromPool(HeroGradeType heroGradeType) { return PoolManager.instance.GetPool(PoolManager.instance.unitPool.queMap, selectedMythic); }

    // 소환 할 신화 위치 반환
    public override GameObject GetUnitPos(UnitType unitType)
    {   
        // 새로운 유닛 스폰 위치 반환
        // 1.아직 스폰되지 않은 새로운 스폰 위치들 중
        // 2.랜덤 스폰 위치 반환

        // 일단 스폰되지 않은 새로운 스폰 위치들 구함
        ListGameObject newUnitPosList = new ListGameObject();
        for(int i = 0; i < spawnPosList.gameObjectList.Count; i++)
        {
            if(spawnPosList.gameObjectList[i].transform.childCount > 0) continue;
            newUnitPosList.gameObjectList.Add(spawnPosList.gameObjectList[i]);
        }

        // 새로운 스폰 위치가 있으면 새로운 스폰 위치 반환
        if(newUnitPosList.gameObjectList.Count > 0)
        {
            GameObject newUnitPos = newUnitPosList.gameObjectList[UnityEngine.Random.Range(0, newUnitPosList.gameObjectList.Count)];
            unitPosMap[unitType][newUnitPos] = 1;
            return newUnitPos;
        }

        // 여기 오는 경우
        // 1.스폰되지 않은 새로운 스폰 위치도 없는 경우 => 모든 스폰 위치가 사용 중
        return null;
    }

    // 소환 할 신화 선택
    public void SelectMythic(string mythicName) { SelectedMythic = (UnitType)Enum.Parse(typeof(UnitType), mythicName, true); }

    // 조합 가능한 신화가 있는지 체크, 조합 가능한 개수 반환
    public void WrapCheckMythicComb() { CheckMythicComb(); }
    public int CheckMythicComb()
    {
        // 신화 조합 가능 체크
        int cnt = 0;

        for(int i = 4; i > -1; i--)
        {
            SelectMythic(mythicCombList[i].mythicType.ToString());
            
            bool isComb = true;
            for(int j = 0; j < 3; j++)
            {
                if(!requireImageList[j].transform.GetChild(0).gameObject.activeSelf)
                {
                    isComb = false;
                    break;
                }
            }

            mythicCombCheckList.gameObjectList[i].SetActive(isComb);
            if(isComb) cnt++;
        }

        return cnt;
    }

    // 신화 소환 정보 UI 갱신
    private void UpdateMythicInfoUI(UnitType mythicType)
    {
        mythicImage.sprite = mythicCombMap[mythicType].mythicSprite;
        mythicText.text = mythicType.ToString();
        for(int i = 0; i < requireImageList.Count; i++)
        {
            requireImageList[i].sprite = mythicCombMap[mythicType].requireList[i].unitSprite;

            // 유닛 수 구하기
            int unitCnt = 0;
            for(int j = 0; j < unitPosMap[mythicCombMap[mythicType].requireList[i].unitType].Count; j++) unitCnt += unitPosMap[mythicCombMap[mythicType].requireList[i].unitType].ElementAt(j).Key.transform.childCount;

            // 보유 유닛 체크 표시
            requireImageList[i].transform.GetChild(0).gameObject.SetActive(unitCnt > mythicCombMap[mythicType].requireList[i].cnt - 1);

            // 오라 색 표시
            Color newColor = gradeColorMap[mythicCombMap[mythicType].requireList[i].gradeType];
            requireImageList[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = newColor;
        }
    }

    // 신화 조합
    public override void GetUnitHandle()
    {
        // 신화 조합에 들어간 유닛 위치와 수 맵핑
        Dictionary<GameObject, int> usedPosMap = new Dictionary<GameObject, int>();

        // 신화 조합에 들어간 유닛 맵핑
        Dictionary<UnitType, int> uesdUnitMap = new Dictionary<UnitType, int>();

        // 필요한 유닛과 수가 있는지 체크 => reverse 체크 => 뒤에 있는 유닛의 cnt 부터 계산
        for(int i = mythicCombMap[selectedMythic].requireList.Count - 1; i > -1; i--)
        {
            // 필요한 유닛과 수
            UnitRequire require = mythicCombMap[selectedMythic].requireList.ElementAt(i);

            // 이미 같은 유닛을 체크한 적이 있는지 체크
            if(uesdUnitMap.ContainsKey(require.unitType)) continue;
            uesdUnitMap.Add(require.unitType, 1);

            // 필요한 유닛의 수가 있는지 체크
            bool isCnt = false;

            // 필요한 유닛이 있으면
            if(unitPosMap.ContainsKey(require.unitType))
            {
                // 유닛 수
                int unitCnt = 0;

                // 필요한 유닛의 위치를 모두 보면서
                for(int j = 0; j < unitPosMap[require.unitType].Count; j++)
                {
                    // 필요한 유닛의 위치와 수
                    KeyValuePair<GameObject, int> curPos = unitPosMap[require.unitType].ElementAt(j);
                    
                    // 유닛 수 누적
                    unitCnt += curPos.Value;

                    // 필요한 유닛 수 이상이 되면 break
                    // 조합에 사용된 위치와 초과 하지 않는 유닛 수 맵핑
                    if(unitCnt >= require.cnt)
                    {
                        isCnt = true;
                        usedPosMap[curPos.Key] = require.cnt - (unitCnt - curPos.Value);
                        break;
                    }

                    // 아직 더 누적 해야한다면
                    // 조합에 사용된 위치와 자식 수 맵핑
                    usedPosMap[curPos.Key] = curPos.Value;
                }

                // 필요한 유닛 수 이상이면 continue
                if(isCnt) continue;
            }
            else
            {
                SoundManager.instance.SFXPlay(SoundType.NotEnough);
                return; // 필요한 유닛이 없으면 리턴
            }

            // 여기 오는 경우
            // 필요한 유닛은 있는데 수가 부족한 경우
            SoundManager.instance.SFXPlay(SoundType.NotEnough);
            return;
        }

        // 신화 소환
        GameObject instantMyth = GetUnit(null);
        GameObject mythPos = GetUnitPos(selectedMythic);
        instantMyth.transform.SetParent(mythPos.transform);
        instantMyth.transform.localPosition = new Vector3(0, 0.2f, 0);
        ++CurUnit;

        // 신화 조합에 들어간 유닛 처리
        // 1.자식 수와 사용된 수가 같으면 맵핑 삭제하기, 아니면 자식 수 감소하기
        // 2.부모 해제하고 풀에 반환하기
        // 3.유닛 수 처리
        for(int i = 0; i < usedPosMap.Count; i++)
        {
            // 필요한 유닛의 위치와 수
            KeyValuePair<GameObject, int> curPos = usedPosMap.ElementAt(i);

            // 필요한 유닛 타입
            UnitType curUnitType = curPos.Key.transform.GetChild(0).GetComponent<CharacterBase>().heroInfo.unitType;

            // 1.자식 수와 사용된 수가 같으면 맵핑 삭제하기, 아니면 자식 수 감소하기
            if(curPos.Key.transform.childCount == curPos.Value) unitPosMap[curUnitType].Remove(curPos.Key);
            else unitPosMap[curUnitType][curPos.Key] -= curPos.Value;

            // 2.부모 해제하고 풀에 반환하기
            for(int j = 0; j < curPos.Value; j++)
            {
                GameObject curUnit = curPos.Key.transform.GetChild(0).gameObject;
                curUnit.transform.SetParent(PoolManager.instance.poolSet.transform);
                PoolManager.instance.ReturnPool(PoolManager.instance.unitPool.queMap, curUnit, curUnitType);
            }

            // 3.유닛 수 처리
            CurUnit -= curPos.Value;
        }

        // 신화 조합 가능 개수 표시
        UiUnit.instance.mythicCombPanel.SetActive(true);
        mythicCombCheckCnt.text = CheckMythicComb().ToString();
        UiUnit.instance.ExitPanel(UiUnit.instance.mythicCombPanel);

        // 사운드
        SoundManager.instance.SFXPlay(SoundType.MythicComb);
    }
}
