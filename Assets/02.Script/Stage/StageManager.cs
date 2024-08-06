using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 스테이지 타입
public enum StageType { Normal, MiniBoss, Boss }

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    private void Awake() { instance = this; }

    // 스테이지 데이터 관리
    [HideInInspector] public List<StageData> stageList = new List<StageData>();

    // 현재 스테이지
    [Header ("최대 스테이지")] [SerializeField] private int maxStage;
    private int curStage;
    public int CurStage
    {
        get { return curStage; }
        set
        {
            curStage = value;
            if(curStage < maxStage) StartStage(value);
            else Debug.Log("스테이지 종료");
        }
    }

    // 현재 몬스터 수
    [Header ("최대 몬스터 수")] [SerializeField] private int maxEnemyCnt;
    private float maxEnemyFloatCnt;
    private int enemyCnt;
    public int EnemyCnt
    {
        get { return enemyCnt; }
        set
        {
            enemyCnt = value;
            if(EnemyCnt > maxEnemyCnt) Debug.Log("몬스터 120 마리 초과");
            UpdateEnemyCntUI(value);
        }
    }
    public ListGameObject instantEnemyList = new ListGameObject(); // 소환된 몬스터 관리

    // 몬스터 이동경로
    [Header ("몬스터 이동 위치")] [SerializeField] private ListGameObject pathPosList;
    public Dictionary<GameObject, ListGameObject> stageTypePathMap = new Dictionary<GameObject, ListGameObject>(); // 몬스터 스폰 위치에 따른 이동경로 맵핑
    [Header ("모든 이동경로 리스트")] [SerializeField] private List<ListGameObject> pathList;

    // 캐싱
    public WaitForSeconds oneSecond = new WaitForSeconds(1f);
    public WaitForSeconds halfSecond = new WaitForSeconds(0.5f);

    // UI
    [Header ("스테이지 번호 텍스트")] [SerializeField] private TextMeshProUGUI stageNumText;
    [Header ("스테이지 시간 텍스트")] [SerializeField] private TextMeshProUGUI stageTimeText;
    [Header ("몬스터 수 텍스트")] [SerializeField] private TextMeshProUGUI enemyCntText;
    [Header ("몬스터 수 필 이미지")] [SerializeField] private Image enemyCntFillImage;

    // 스테이지 데이터 초기화 및 스테이지 시작
    private void Start()
    {
        maxEnemyFloatCnt = maxEnemyCnt;
        InitStage();
        StartStage(curStage);
    }

    // 스테이지 데이터 초기화
    private void InitStage()
    {
        // 몬스터 스폰위치에 따라 이동경로 맵핑
        stageTypePathMap.Add(pathPosList.gameObjectList[0], pathList[0]);
        stageTypePathMap.Add(pathPosList.gameObjectList[1], pathList[1]);
        stageTypePathMap.Add(pathPosList.gameObjectList[2], pathList[2]);

        for(int i = 0; i < maxStage; i++)
        {
            StageData stageData = ScriptableObject.CreateInstance<StageData>();

            // 스테이지 번호
            stageData.stageNumber = i + 1;

            // 스테이지 타입
            if(stageData.stageNumber % 10 == 0) stageData.stageType = StageType.Boss;
            else stageData.stageType = stageData.stageNumber % 5 == 0 ? StageType.MiniBoss : StageType.Normal;

            // 스테이지 시간, 몬스터 타입, 몬스터 소환 위치
            switch(stageData.stageType)
            {
                case StageType.Normal :
                    stageData.stageTime = 20;
                    stageData.enemyType = (EnemyType)(stageData.stageNumber / 5 * 2);
                    stageData.spawnPos.gameObjectList.Add(pathPosList.gameObjectList[0]);
                    stageData.spawnPos.gameObjectList.Add(pathPosList.gameObjectList[1]);
                    break;
                case StageType.MiniBoss :
                    stageData.stageTime = 30;
                    stageData.enemyType = (EnemyType)(1 + 4 * (stageData.stageNumber / 10));
                    stageData.spawnPos.gameObjectList.Add(pathPosList.gameObjectList[2]);
                    break;
                case StageType.Boss :
                    stageData.stageTime = 60;
                    stageData.enemyType = (EnemyType)(3 + 4 * (stageData.stageNumber / 10 - 1));
                    stageData.spawnPos.gameObjectList.Add(pathPosList.gameObjectList[2]);
                    break;
            }

            stageList.Add(stageData);
        }
    }

    // 스테이지 시작
    private void StartStage(int cur)
    {
        // 스테이지 타입에 따라 시작
        StageData stage = stageList[cur];
        switch(stage.stageType)
        {
            case StageType.Normal : StartCoroutine(StartNormalStage(stage)); break;
            case StageType.MiniBoss : StartCoroutine(StartMiniBossStage(stage)); break;
            case StageType.Boss : StartCoroutine(StartBossStage(stage)); break;
        }
        
        // UI 갱신
        UpdateStageNumUI(stage);
        UpdateStageTimeUI(stage.stageTime);
    }

    // 일반 스테이지
    private IEnumerator StartNormalStage(StageData stage)
    {
        // 사운드
        SoundManager.instance.BgmSoundPlay(BgmType.Normal);

        // 몬스터 소환
        int stageTime = stage.stageTime;
        for(int i = 0; i < stage.stageTime; i++)
        {
            EnemySpawn(stage);
            yield return oneSecond;
            UpdateStageTimeUI(--stageTime);
        }

        // 다음 스테이지
        ++CurStage;
    }

    // 미니보스 스테이지
    private IEnumerator StartMiniBossStage(StageData stage)
    {
        // 사운드
        SoundManager.instance.BgmSoundPlay(BgmType.MiniBoss);

        // 몬스터 소환
        EnemySpawn(stage);
        int stageTime = stage.stageTime;
        for(int i = 0; i < stage.stageTime; i++)
        {
            yield return oneSecond;
            UpdateStageTimeUI(--stageTime);
        }

        // 다음 스테이지
        ++CurStage;
    }

    // 보스 스테이지
    private IEnumerator StartBossStage(StageData stage)
    {
        // 사운드
        SoundManager.instance.BgmSoundPlay(BgmType.Boss);

        // 몬스터 소환
        GameObject boss = EnemySpawn(stage);
        int stageTime = stage.stageTime;
        for(int i = 0; i < stage.stageTime; i++)
        {
            yield return oneSecond;
            UpdateStageTimeUI(--stageTime);
        }

        // 보스 잡았는지 체크
        if(boss.activeSelf) Debug.Log("보스 잡기 실패");

        // 다음 스테이지
        ++CurStage;
    }

    // 몬스터 소환
    private GameObject EnemySpawn(StageData stage)
    {
        GameObject instantEnemy = null;
        for(int i = 0; i < stage.spawnPos.gameObjectList.Count; i++)
        {
            instantEnemy = PoolManager.instance.GetPool(PoolManager.instance.enemyPool.queMap, stage.enemyType);
            instantEnemy.transform.position = stage.spawnPos.gameObjectList[i].transform.position;
            instantEnemy.GetComponent<EnemyBase>().spawnPos = stage.spawnPos.gameObjectList[i];
            instantEnemy.GetComponent<EnemyBase>().enemyType = stage.enemyType;
            ++EnemyCnt;
            instantEnemyList.gameObjectList.Add(instantEnemy);
        }
        return instantEnemy;
    }

    // 스테이지 UI 갱신
    private void UpdateStageNumUI(StageData stage) { stageNumText.text = stage.stageNumber.ToString(); }
    private void UpdateStageTimeUI(int cur) { stageTimeText.text = cur.ToString(); }
    private void UpdateEnemyCntUI(int cur) { enemyCntText.text = $"{cur} / {maxEnemyCnt}"; enemyCntFillImage.fillAmount = cur / maxEnemyFloatCnt; }
}
