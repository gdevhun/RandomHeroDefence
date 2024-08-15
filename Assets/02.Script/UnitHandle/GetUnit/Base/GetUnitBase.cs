using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public abstract class GetUnitBase : MonoBehaviour
{
    static protected ListGameObject spawnPosList; // 스폰 위치들
    static public Dictionary<UnitType, Dictionary<GameObject, int> > unitPosMap; // (유닛, (위치, 자식 수)) 맵핑
    static protected int maxUnit; // 최대 유닛 수
    static private int curUnit; // 현재 유닛 수
    static public int CurUnit
    {
        get { return curUnit; }
        set { curUnit = value; UpdateUnitUI(value); }
    }
    private static TextMeshProUGUI unitCntText; // 유닛 수 텍스트

    // 초기화
    private void Awake()
    {
        //if(maxUnit == 50) return;

        spawnPosList = new ListGameObject();
        Transform unitPosSet = GameObject.Find("UnitPosSet").transform;
        for(int i = 0; i < unitPosSet.childCount; i++) spawnPosList.gameObjectList.Add(unitPosSet.GetChild(i).gameObject);

        unitPosMap = new Dictionary<UnitType, Dictionary<GameObject, int> >();
        for(int i = 0; i < Enum.GetValues(typeof(UnitType)).Length; i++) unitPosMap[(UnitType)Enum.GetValues(typeof(UnitType)).GetValue(i)] = new Dictionary<GameObject, int>();

        maxUnit = 50;

        curUnit = 0;

        unitCntText = GameObject.Find("UnitCntText").GetComponent<TextMeshProUGUI>();
    }

    // 가중치에 따라 유닛을 소환하는 함수
    public virtual GameObject GetUnit(Dictionary<HeroGradeType, int> gradeWeightMap)
    {
        // 모든 가중치 합
        int totalWeight = 0;
        for(int i = 0; i < gradeWeightMap.Count; i++) totalWeight += gradeWeightMap.ElementAt(i).Value;

        // 가중치에 따른 유닛
        int randomWeight = UnityEngine.Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        for(int i = 0; i < gradeWeightMap.Count; i++)
        {
            accumulatedWeight += gradeWeightMap.ElementAt(i).Value;
            if(randomWeight >= accumulatedWeight) continue;

            // 사운드
            SoundManager.instance.SFXPlay(SoundType.GetUnit);

            // 소환 할 유닛 풀링
            return GetUnitFromPool(gradeWeightMap.ElementAt(i).Key);
        }

        return null;
    }

    // 소환 할 유닛 풀링
    protected virtual GameObject GetUnitFromPool(HeroGradeType heroGradeType) { return PoolManager.instance.GetPool(PoolManager.instance.unitPool.queMap, (UnitType)UnityEngine.Random.Range(0 + 5 * (int)heroGradeType, 5 + 5 * (int)heroGradeType)); }

    // 유닛 스폰 위치 반환
    public virtual GameObject GetUnitPos(UnitType unitType)
    {
        // 같은 유닛이 스폰된 위치가 있으면서
        if(unitPosMap.ContainsKey(unitType))
        {
            // 같은 유닛이 스폰된 위치들 중
            for(int i = 0; i < unitPosMap[unitType].Count; i++)
            {
                // 자식 수가 3 개 보다 작을 때 스폰 위치 반환
                if(unitPosMap[unitType].ElementAt(i).Value >= 3) continue;
                GameObject unitPos = unitPosMap[unitType].ElementAt(i).Key;
                unitPosMap[unitType][unitPos]++;
                return unitPos;
            }
        }

        // 여기 오는 경우
        // 1. 같은 유닛이 스폰된 위치가 있지만 자식이 모두 3 개인 경우
        // 2. 같은 유닛이 스폰된 위치가 없는 경우
        
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

    // 유닛 UI 갱신
    private static void UpdateUnitUI(int val) { unitCntText.text = val.ToString() + " / " + maxUnit; }

    // 소환, 합성, 도박, 신화에서 구현
    public abstract void GetUnitHandle();
}
