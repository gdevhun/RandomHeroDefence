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
    public List<GameObject> gameObjectList;

    // 생성자
    public ListGameObject() { gameObjectList = new List<GameObject>(); }
    public ListGameObject(List<GameObject> gameObjList) { gameObjectList = gameObjList; }
}

// 유닛 타입
public enum UnitType
{
    Ganster, Hunter, Soldier, Thief, Wrestler,
    Alex, Assassin, Barbarian, Bunker, Warrior,
    Alonso, Barrel, Bat, Tonkey, Viking,
    Aasole, Alisda, Banies, Louizy, Makdus,
    Batman, Macree, Magnus, Mario, Yumie
}
// 공격 무기 이펙트 (원거리무기총알, 근접무기이펙트)
public enum WeaponEffect
{
    GansterMelee, HunteBullet, SoldierBullet, ThiefMelee, WrestlerMelee,
    AlexBullet, AssassinMelee, BarbarianMelee, BunkerBullet, WarriorMelee,
    AlonsoMelee, BarrelBullet, BatBullet, TonkeyBullet, VikingMelee,
    AasoleBullet, AlisdaMelee, BaniesBullet, LouizyBullet, MakdusMelee,
    BatmanBullet, MacreeBullet, MagnusMelee, MarioBullet, YumieMelee
}
// 사운드 타입
public enum SoundType
{
    GetUnit, Sell, Click, Upgrade, NotEnough
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    private void Awake() { instance = this; }
    [Header ("풀링 오브젝트 부모")] public GameObject poolSet;
    [Header ("유닛 풀")] public Pool<UnitType> unitPool;
    [Header ("히어로 무기")] public Pool<WeaponEffect> weaponEffectPool;
    [Header ("사운드 풀")] public Pool<SoundType> soundPool;

    private void Start()
    {
        // (타입, 프리팹) 맵핑
        PrefMap(unitPool.allList, unitPool.prefMap);
        PrefMap(soundPool.allList, soundPool.prefMap);

        // (타입, 큐) 맵핑
        QueMap(unitPool.queMap, unitPool.prefMap);
        QueMap(soundPool.queMap, soundPool.prefMap);
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

                // 부모를 풀셋으로
                obj.transform.SetParent(poolSet.transform);

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