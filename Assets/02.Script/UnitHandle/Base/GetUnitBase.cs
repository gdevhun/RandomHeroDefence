using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class GetUnitBase : MonoBehaviour
{
    // 가중치에 따라 유닛을 소환하는 함수
    protected GameObject GetUnit(Dictionary<HeroGradeType, int> gradeWeightMap)
    {
        // 모든 가중치 합
        int totalWeight = 0;
        for(int i = 0; i < gradeWeightMap.Count; i++) totalWeight += gradeWeightMap.ElementAt(i).Value;

        // 가중치에 따른 유닛
        int randomWeight = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        for(int i = 0; i < gradeWeightMap.Count; i++)
        {
            accumulatedWeight += gradeWeightMap.ElementAt(i).Value;
            if(randomWeight >= accumulatedWeight) continue;

            // 소환 할 유닛 풀링
            return GetUnitFromPool(gradeWeightMap.ElementAt(i).Key);
        }

        return null;
    }

    // 소환 할 유닛 풀링
    private GameObject GetUnitFromPool(HeroGradeType heroGradeType)
    {
        int random = Random.Range(0 + 5 * (int)heroGradeType, 5 + 5 * (int)heroGradeType);
        return PoolManager.instance.GetPool(PoolManager.instance.queUnitMap, (UnitType)random);
    }

    // 소환, 합성, 도박에서 구현
    public abstract void GetUnitHandle();
}
