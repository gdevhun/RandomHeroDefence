using System;
using System.Collections.Generic;
using UnityEngine;

// 맵핑 관련 클래스
[Serializable]
public class Pool<T>
{
    [Header ("모든 프리팹")] public List<ListGameObject> allList = new List<ListGameObject>();
    public Dictionary<T, GameObject> prefMap = new Dictionary<T, GameObject>(); // (타입, 프리팹) 맵핑
    public Dictionary<T, Queue<GameObject> > queMap = new Dictionary<T, Queue<GameObject> >(); // (타입, 큐) 맵핑
}

// 게임 오브젝트 형 리스트를 가지는 클래스
[Serializable]
public class ListGameObject
{
    [Header ("프리팹")] public List<GameObject> gameObjectList;

    // 생성자
    public ListGameObject() { gameObjectList = new List<GameObject>(); }
    public ListGameObject(List<GameObject> gameObjList) { gameObjectList = gameObjList; }
}

// 유닛 타입
public enum UnitType
{
    갱스터, 헌터, 솔져, 시프, 레슬러,
    알렉스, 어쌔신, 바바리안, 벙커, 에키온,
    알론소, 에이든, 뱃, 통키, 바이킹,
    아아솔, 알리스다, 배니스, 루이지, 막더스,
    배트맨, 테오도르, 마그너스, 마리오, 유미
}
// 공격 무기 이펙트 (원거리무기총알, 근접무기이펙트)
public enum WeaponEffect
{
    GansterMelee, HunteBullet, SoldierBullet, ThiefMelee, WrestlerMelee,
    AlexBullet, AssassinMelee, BarbarianMelee, BunkerBullet, EkionMelee,
    AlonsoMelee, AdenMelee, BatBullet, TonkeyBullet, VikingMelee,
    AasoleBullet, AlisdaMelee, BaniesBullet, LouizyBullet, MakdusMelee,
    BatmanBullet, TeodorMelee, MagnusMelee, MarioBullet, YumieMelee,
    BatmanAbilityBullet, MarioAbilityBullet
}
// 사운드 타입
public enum SoundType
{
    GetUnit, Sell, Click, Upgrade, NotEnough, Roulette, MythicComb,
    평타각목둔기, 평타철둔기, 평타핸드, 평타에키온에이든테오도르, 총알마법평타, 총알물리평타,
    갱스터스킬, 레슬러스킬, 시프스킬, 헌터스킬,
    바바리안스킬, 벙커스킬, 알렉스스킬, 어쌔신스킬,
    알론소스킬, 통키스킬,
    막더스스킬, 알리스다스킬,
    배트맨스킬, 마리오스킬, 마그너스스킬, 테오도르스킬, 유미스킬1, 유미스킬2
}

// 몬스터 타입
public enum EnemyType
{
    Normal1, MiniBoss1, Normal2, Boss1,
    Normal3, MiniBoss2, Normal4, Boss2,
    Normal5, MiniBoss3, Normal6, Boss3,
    Normal7, MiniBoss4, Normal8, Boss4,
    Normal9, MiniBoss5, Normal10, Boss5
}

// 스킬 이펙트
public enum AbilityEffectType
{
    스턴, 시프, 통키, 알론소, 알리스다, 막더스,
    배트맨, 마리오, 마그너스, 테오도르, 유미슬로우, 유미빛
}

// 플로팅 텍스트
public enum FloatingTextType
{
    데미지플로팅
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    [Header ("풀링 오브젝트 부모")] public GameObject poolSet;
    [Header ("플로팅 텍스트 부모")] public GameObject floatingTextSet;
    [Header ("유닛 풀")] public Pool<UnitType> unitPool;
    [Header ("히어로 무기")] public Pool<WeaponEffect> weaponEffectPool;
    [Header ("사운드 풀")] public Pool<SoundType> soundPool;
    [Header ("몬스터 풀")] public Pool<EnemyType> enemyPool;
    [Header ("스킬 이펙트 풀")] public Pool<AbilityEffectType> abilityEffectPool;
    [Header ("플로팅 텍스트 풀")] public Pool<FloatingTextType> floatingTextPool;

    private void Awake()
    {
        instance = this;

        // (타입, 프리팹) 맵핑
        PrefMap(unitPool.allList, unitPool.prefMap);
        PrefMap(soundPool.allList, soundPool.prefMap);
        PrefMap(weaponEffectPool.allList,weaponEffectPool.prefMap);
        PrefMap(enemyPool.allList, enemyPool.prefMap);
        PrefMap(abilityEffectPool.allList, abilityEffectPool.prefMap);
        PrefMap(floatingTextPool.allList, floatingTextPool.prefMap);

        // (타입, 큐) 맵핑
        QueMap(unitPool.queMap, unitPool.prefMap);
        QueMap(soundPool.queMap, soundPool.prefMap, 100);
        QueMap(weaponEffectPool.queMap,weaponEffectPool.prefMap, 500);
        QueMap(enemyPool.queMap, enemyPool.prefMap, 130);
        QueMap(abilityEffectPool.queMap, abilityEffectPool.prefMap);
        QueMap(floatingTextPool.queMap, floatingTextPool.prefMap, 20000);
    }

    // (타입, 프리팹) 맵핑
    private void PrefMap<T>(List<ListGameObject> pList, Dictionary<T, GameObject> pMap) where T : Enum
    {
        // 타입 초기화
        T curType = default;

        for(int i = 0; i < pList.Count; i++)
        {
            for(int j = 0; j < pList[i].gameObjectList.Count; j++)
            {
                // 사운드 타입 설정
                if(typeof(T) == typeof(SoundType)) pList[i].gameObjectList[j].GetComponent<SoundDeActive>().type = (SoundType)(object)curType;

                // 현재 타입에 해당하는 프리팹 맵핑
                pMap.Add(curType, pList[i].gameObjectList[j]);
                
                // 다음 타입
                curType = (T)(object)(((int)(object)curType) + 1);
            }
        }
    }

    // (타입, 큐) 맵핑
    private void QueMap<T>(Dictionary<T, Queue<GameObject> > qMap, Dictionary<T, GameObject> pMap, int cnt = 50) where T : Enum
    {   
        // 타입을 하나씩 꺼내서
        foreach(T type in Enum.GetValues(typeof(T)))
        {
            // 타입에 해당하는 프리팹을 하나씩 꺼내서
            Queue<GameObject> queue = new Queue<GameObject>();
            GameObject prefab = pMap[type];

            // cnt 개 생성하고 비활성화 후 큐에 저장
            for(int i = 0; i < cnt; i++)
            {
                // 프리팹 생성
                GameObject obj = Instantiate(prefab);

                // 부모 설정
                if(typeof(T) == typeof(FloatingTextType))
                {
                    obj.transform.SetParent(floatingTextSet.transform);
                    obj.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else obj.transform.SetParent(poolSet.transform);

                // 비활성화
                obj.SetActive(false);

                // 큐에 저장
                queue.Enqueue(obj);
            }

            // (타입, 큐) 맵핑
            qMap.Add(type, queue);
        }
    }

    // 꺼냄
    public GameObject GetPool<T>(Dictionary<T, Queue<GameObject> > qMap, T type) where T : Enum
    {
        // 키가 존재하고 큐에 오브젝트가 있으면 꺼냄
        if(qMap.ContainsKey(type) && qMap[type].Count > 0)
        {
            // 오브젝트 꺼내서
            GameObject obj = qMap[type].Dequeue();

            // 오브젝트 활성화
            obj.SetActive(true);

            // 오브젝트 반환
            return obj;
        }

        // 사용 가능한 오브젝트가 없으면
        return null;
    }

    // 반환
    public void ReturnPool<T>(Dictionary<T, Queue<GameObject> > qMap, GameObject obj, T type) where T : Enum
    {
        // 오브젝트 비활성화
        obj.SetActive(false);

        // 해당 타입의 풀로 반환
        qMap[type].Enqueue(obj);
    }
}