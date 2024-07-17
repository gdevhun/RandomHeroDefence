using System;
using System.Collections.Generic;
using UnityEngine;

// 게임 오브젝트 형 리스트를 가지는 클래스
[Serializable]
public class ListGameObject
{
    public List<GameObject> gameObjectList;

    // 생성자
    public ListGameObject() {}
    public ListGameObject(List<GameObject> gameObjList) { gameObjectList = gameObjList; }
}

// 유닛 타입
public enum UnitType
{
    Alex, Assassin, Barbarian, Bunker, Warrior,
    Alonso, Barrel, Bat, Tonkey, Viking,
    Batman, Macree, Magnus, Mario, Yumie,
    Aasole, Alisda, Banies, Louizy, Makdus
}

// 사운드 타입
public enum SoundType
{
    Spawn
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    private void Awake() { instance = this; }
    public GameObject poolSet; // 오브젝트 부모
    [Header ("유닛 맵핑")] [SerializeField] private List<ListGameObject> UnitList = new List<ListGameObject>();
    [SerializeField] private ListGameObject NormalList = new ListGameObject(), EliteList = new ListGameObject(), RareList = new ListGameObject(), LegendList = new ListGameObject(), MythList = new ListGameObject();
    private Dictionary<UnitType, GameObject> prefUnitMap = new Dictionary<UnitType, GameObject>(); // (타입, 프리팹) 맵핑
    public Dictionary<UnitType, Queue<GameObject> > queUnitMap = new Dictionary<UnitType, Queue<GameObject> >(); // (타입, 큐) 맵핑
    [Header ("사운드 맵핑")] [SerializeField] private List<ListGameObject> SoundList = new List<ListGameObject>();
    [SerializeField] private ListGameObject UnitHandleSoundList = new ListGameObject();
    private Dictionary<SoundType, GameObject> prefSoundMap = new Dictionary<SoundType, GameObject>(); // (타입, 프리팹) 맵핑
    public Dictionary<SoundType, Queue<GameObject> > queSoundMap = new Dictionary<SoundType, Queue<GameObject> >(); // (타입, 큐) 맵핑

    private void Start()
    {
        // 리스트 초기화
        ListInit(UnitList, NormalList, EliteList, RareList, LegendList, MythList);
        ListInit(SoundList, UnitHandleSoundList);

        // (타입, 프리팹) 맵핑
        PrefMap(UnitList, prefUnitMap);
        PrefMap(SoundList, prefSoundMap);

        // (타입, 큐) 맵핑
        QueMap(queUnitMap, prefUnitMap);
        QueMap(queSoundMap, prefSoundMap);
    }

    // 풀링 테스트
    private void Update()
    {
        // 일반 등급 뽑기
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            int random = UnityEngine.Random.Range(0, 5);
            GameObject instantObj = GetPool(queUnitMap, (UnitType)random);
            instantObj.transform.position = Vector3.zero;
        }
        // 고급 등급 뽑기
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            int random = UnityEngine.Random.Range(5, 10);
            GameObject instantObj = GetPool(queUnitMap, (UnitType)random);
            instantObj.transform.position = Vector3.zero;
        }
        // 희귀 등급 뽑기
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            int random = UnityEngine.Random.Range(10, 15);
            GameObject instantObj = GetPool(queUnitMap, (UnitType)random);
            instantObj.transform.position = Vector3.zero;
        }
        // 전설 등급 뽑기
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            int random = UnityEngine.Random.Range(15, 20);
            GameObject instantObj = GetPool(queUnitMap, (UnitType)random);
            instantObj.transform.position = Vector3.zero;
        }
        // 신화 등급 뽑기
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            int random = UnityEngine.Random.Range(20, 25);
            GameObject instantObj = GetPool(queUnitMap, (UnitType)random);
            instantObj.transform.position = Vector3.zero;
        }
    }

    // 리스트 초기화
    private void ListInit(List<ListGameObject> allPrefList, params ListGameObject[] lists) { allPrefList.AddRange(lists); }

    // (타입, 프리팹) 맵핑
    private void PrefMap<T>(List<ListGameObject> pList, Dictionary<T, GameObject> pMap) where T : Enum
    {
        // 타입 초기화
        T curType = default;

        for(int i = 0; i < pList.Count; i++)
        {
            for(int j = 0; j < pList[i].gameObjectList.Count; j++)
            {
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