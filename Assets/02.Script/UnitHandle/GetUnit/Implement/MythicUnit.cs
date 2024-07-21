using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// 신화 조합식
[System.Serializable]
public class MythicComb
{ 
    [Header ("조합 할 신화 유닛")] public UnitType mythicType;
    [Header ("필요한 유닛과 수")] public List<UnitRequire> requireList;
}
[System.Serializable]
public class UnitRequire
{
    [Header ("필요한 유닛 타입")] public UnitType unitType;
    [Header ("필요한 유닛 수")] public int cnt;
}

public class MythicUnit : GetUnitBase
{
    [Header ("신화 조합식")] [SerializeField] private MythicComb mythicComb;

    // 신화 조합 테스트
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8)) GetUnitHandle();
    }

    // 소환 할 신화 소환
    protected override GameObject GetUnit(Dictionary<HeroGradeType, int> gradeWeightMap)
    {
        return GetUnitFromPool(HeroGradeType.Myth);
    }

    // 소환 할 신화 풀링
    protected override GameObject GetUnitFromPool(HeroGradeType heroGradeType)
    {
        return PoolManager.instance.GetPool(PoolManager.instance.queUnitMap, mythicComb.mythicType);
    }

    public override void GetUnitHandle()
    {
        // 신화 조합에 들어간 유닛 위치와 수 맵핑
        Dictionary<GameObject, int> usedPosMap = new Dictionary<GameObject, int>();

        // 필요한 유닛과 수가 있는지 체크
        for(int i = 0; i < mythicComb.requireList.Count; i++)
        {
            // 필요한 유닛과 수
            UnitRequire require = mythicComb.requireList.ElementAt(i);

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
            else return; // 필요한 유닛이 없으면 리턴

            // 여기 오는 경우
            // 필요한 유닛은 있는데 수가 부족한 경우
            return;
        }

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
                PoolManager.instance.ReturnPool(PoolManager.instance.queUnitMap, curUnit, curUnitType);
            }

            // 3.유닛 수 처리
            curUnit -= curPos.Value;
        }

        // 신화 소환
        GameObject instantMyth = GetUnit(null);
        GameObject mythPos = GetUnitPos(mythicComb.mythicType);
        instantMyth.transform.SetParent(mythPos.transform);
        instantMyth.transform.localPosition = new Vector3(mythPos.transform.childCount == 3 ? 0.1f : 0.2f * (mythPos.transform.childCount - 1), mythPos.transform.childCount == 3 ? -0.2f : 0, 0);
        ++curUnit;
    }
}
