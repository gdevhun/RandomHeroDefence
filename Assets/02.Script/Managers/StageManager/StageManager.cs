using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 스테이지 타입
public enum StageType { Normal, MiniBoss, Boss }

// 기준 스테이지 몬스터 스탯 정보
[Serializable]
public class StandardMonsterStat
{
    public float hp;
    public float increaseHp;
    public float phyDef;
    public float magDef;
    public float moveSpeed;
    public float increaseMoveSpeed;
    public float bossHp;
    public float bossPhyDef;
    public float bossMagDef;
    public float bossMoveSpeed;
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    private void Awake() { instance = this; }
    
    // 스테이지 데이터 관리
    [HideInInspector] public List<StageData> stageList = new List<StageData>();

    // 현재 스테이지
    [Header ("최대 스테이지")] public int maxStage;
    private int curStage;
    public int CurStage
    {
        get { return curStage; }
        set
        {
            curStage = value;
            if(curStage < maxStage)
            {
                StartStage(value);
                CurrencyManager.instance.AcquireCurrency((curStage - 1) * 10 + CurrencyManager.instance.Gold / 10, true);
            }
            else
            {
                // 게임 클리어
                GameManager.instance.PlayerGameWin();
                StopAllCoroutines();
            }
        }
    }
    //보스등장 알림 UI
    [Header ("보스 타입 출현 인포 패널 오브젝트")] 
    public GameObject notifyMiniBossSpawnPanel;
    public GameObject notifyBossSpawnPanel;
    private enum SpawnedBossType{
        미니보스출현패널, 보스출현패널
    }
    // 현재 몬스터 수
    [Header ("최대 몬스터 수")] [SerializeField] private int maxEnemyCnt;
    [HideInInspector] public float maxEnemyFloatCnt;
    private int enemyCnt;
    public int EnemyCnt
    {
        get { return enemyCnt; }
        set
        {
            if(value < 0) return;
            
            enemyCnt = value;
            if(EnemyCnt > maxEnemyCnt)
            {
                // 게임 실패
                GameManager.instance.PlayerGameOver();
                StopAllCoroutines();
            }
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

    // 기준 스테이지 몬스터 스탯 맵핑
    [Header ("스테이지 몬스터 스탯 정보")] [SerializeField] private List<StandardMonsterStat> standardMonsterStatList = new List<StandardMonsterStat>();
    public Dictionary<int, StandardMonsterStat> standardMonsterStatMap = new Dictionary<int, StandardMonsterStat>();

    // 스테이지 데이터 초기화 및 스테이지 시작
    private void Start()
    {
        maxEnemyFloatCnt = maxEnemyCnt;
        InitStage();
        StartCoroutine(GameManager.instance.GameStartRoutine());
    }

    // 스테이지 데이터 초기화
    private void InitStage()
    {
        // 몬스터 스폰위치에 따라 이동경로 맵핑
        stageTypePathMap.Add(pathPosList.gameObjectList[0], pathList[0]);
        stageTypePathMap.Add(pathPosList.gameObjectList[1], pathList[1]);
        stageTypePathMap.Add(pathPosList.gameObjectList[2], pathList[2]);

        // 기준 스테이지 몬스터 스탯 맵핑
        for(int i = 0; i < standardMonsterStatList.Count; i++) standardMonsterStatMap.Add(i, standardMonsterStatList[i]);

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
                    stageData.stageCurrency = 1 + stageData.stageNumber / 20;
                    break;
                case StageType.MiniBoss :
                    stageData.stageTime = 30;
                    stageData.enemyType = (EnemyType)(1 + 4 * (stageData.stageNumber / 10));
                    stageData.spawnPos.gameObjectList.Add(pathPosList.gameObjectList[2]);
                    stageData.stageCurrency = 2 + stageData.stageNumber / 10 ;
                    break;
                case StageType.Boss :
                    stageData.stageTime = 60;
                    stageData.enemyType = (EnemyType)(3 + 4 * (stageData.stageNumber / 10 - 1));
                    stageData.spawnPos.gameObjectList.Add(pathPosList.gameObjectList[2]);
                    stageData.stageCurrency = 3 + stageData.stageNumber / 10 ;
                    break;
            }

            stageList.Add(stageData);
        }
    }

    // 스테이지 시작
    public void StartStage(int cur)
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
        switch(stage.stageNumber)
        {
            case 1 : SoundManager.instance.BgmSoundPlay(BgmType.구간1에서9); break;
            case 11 : SoundManager.instance.BgmSoundPlay(BgmType.구간11에서19); break;
            case 21 : SoundManager.instance.BgmSoundPlay(BgmType.구간21에서29); break;
            case 31 : SoundManager.instance.BgmSoundPlay(BgmType.구간31에서34); break;
            case 35 : SoundManager.instance.BgmSoundPlay(BgmType.구간35에서39); break;
            case 41 : SoundManager.instance.BgmSoundPlay(BgmType.구간41에서44); break;
            case 45 : SoundManager.instance.BgmSoundPlay(BgmType.구간45에서49); break;
            default : break;
        }

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
        // UI 미니보스 출현 인포 출력
        StartCoroutine(SpawnedBossTypeInfoRoutine(SpawnedBossType.미니보스출현패널));
        
        // 몬스터 소환
        GameObject miniBoss = EnemySpawn(stage);
        int stageTime = stage.stageTime;
        for(int i = 0; i < stage.stageTime; i++)
        {
            yield return oneSecond;

            // 필드에 몬스터가없으면 시간단축
            if(EnemyCnt == 0 && stageTime > 10)
            {
                i = 0;
                stageTime = 10;
                stage.stageTime = 10;
                UpdateStageTimeUI(10);
                continue;
            }

            UpdateStageTimeUI(--stageTime);
        }

        // 미니보스 잡았는지 체크
        if(miniBoss.activeSelf)
        {
            PoolManager.instance.ReturnPool(PoolManager.instance.enemyPool.queMap, miniBoss, stage.enemyType);
            EnemyCnt--;
        }

        // 다음 스테이지
        ++CurStage;
    }

    // 보스 스테이지
    private IEnumerator StartBossStage(StageData stage)
    {
        // UI 미니보스 출현 인포 출력
        StartCoroutine(SpawnedBossTypeInfoRoutine(SpawnedBossType.보스출현패널));
        
        // 사운드 10 20 30 40 50
        switch(stage.stageNumber)
        {
            case 10 : SoundManager.instance.BgmSoundPlay(BgmType.스테이지10); break;
            case 20 : SoundManager.instance.BgmSoundPlay(BgmType.스테이지20); break;
            case 30 : SoundManager.instance.BgmSoundPlay(BgmType.스테이지30); break;
            case 40 : SoundManager.instance.BgmSoundPlay(BgmType.스테이지40); break;
            case 50 : SoundManager.instance.BgmSoundPlay(BgmType.스테이지50); break;
            default : break;
        }

        // 몬스터 소환
        GameObject boss = EnemySpawn(stage);
        int stageTime = stage.stageTime;
        int tempStageTime = stage.stageTime;
        for(int i = 0; i < stage.stageTime; i++)
        {
            yield return oneSecond;

            // 보스 잡았으면
            if(!boss.activeSelf)
            {
                // 보스 생존시간
                MissionManager.instance.curBossSurvivalSecond = 60 - tempStageTime;

                // 마지막 스테이지에서 잡았으면
                if(stage.stageNumber == maxStage)
                {
                    // 게임 클리어
                    GameManager.instance.PlayerGameWin();
                    yield break;
                }

                // 필드에 몬스터가없으면 시간단축
                if(EnemyCnt == 0 && stageTime > 10)
                {
                    i = 0;
                    stageTime = 10;
                    stage.stageTime = 10;
                    UpdateStageTimeUI(10);
                    continue;
                }
            }

            UpdateStageTimeUI(--stageTime);
            tempStageTime--;
        }

        // 보스 잡았는지 체크
        if(boss.activeSelf)
        {
            // 게임 실패
            GameManager.instance.PlayerGameOver();
            yield break;
        }

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
            EnemyBase enemyBase = instantEnemy.GetComponent<EnemyBase>();
            enemyBase.spawnPos = stage.spawnPos.gameObjectList[i];
            enemyBase.enemyType = stage.enemyType;
            enemyBase.enemyCurrency = stage.stageCurrency;
            enemyBase.isDead = false;
            ++EnemyCnt;
            instantEnemyList.gameObjectList.Add(instantEnemy);

            // 몬스터 스탯 설정

            // 일반 스테이지
            if(stage.stageType != StageType.MiniBoss && stage.stageType != StageType.Boss)
            {
                enemyBase.maxHp = standardMonsterStatMap[CurStage / 10].hp + standardMonsterStatMap[CurStage / 10].increaseHp * CurStage;
                enemyBase.CurrentHp = standardMonsterStatMap[CurStage / 10].hp + standardMonsterStatMap[CurStage / 10].increaseHp * CurStage;
                enemyBase.originMoveSpeed = standardMonsterStatMap[CurStage / 10].moveSpeed + standardMonsterStatMap[CurStage / 10].increaseMoveSpeed * CurStage;
                enemyBase.moveSpeed = standardMonsterStatMap[CurStage / 10].moveSpeed + standardMonsterStatMap[CurStage / 10].increaseMoveSpeed * CurStage;
                enemyBase.phyDef = standardMonsterStatMap[CurStage / 10].phyDef;
                enemyBase.magDef = standardMonsterStatMap[CurStage / 10].magDef;
                continue;
            }

            // 미니 보스 및 보스 스테이지
            enemyBase.maxHp = stage.stageType == StageType.MiniBoss ? standardMonsterStatMap[CurStage / 10].bossHp : standardMonsterStatMap[CurStage / 10].bossHp * 5;
            enemyBase.CurrentHp = stage.stageType == StageType.MiniBoss ? standardMonsterStatMap[CurStage / 10].bossHp : standardMonsterStatMap[CurStage / 10].bossHp * 5;
            enemyBase.originMoveSpeed = stage.stageType == StageType.MiniBoss ? 1 + standardMonsterStatMap[CurStage / 10].bossMoveSpeed : 1 + standardMonsterStatMap[CurStage / 10].bossMoveSpeed * 2;
            enemyBase.moveSpeed = stage.stageType == StageType.MiniBoss ? 1 + standardMonsterStatMap[CurStage / 10].bossMoveSpeed : 1 + standardMonsterStatMap[CurStage / 10].bossMoveSpeed * 2;
            enemyBase.phyDef = stage.stageType == StageType.MiniBoss ? standardMonsterStatMap[CurStage / 10].bossPhyDef : standardMonsterStatMap[CurStage / 10].bossPhyDef * 2;
            enemyBase.magDef = stage.stageType == StageType.MiniBoss ? standardMonsterStatMap[CurStage / 10].bossMagDef : standardMonsterStatMap[CurStage / 10].bossMagDef * 2;
        }
        return instantEnemy;
    }
    
    // 스테이지 UI 갱신
    private void UpdateStageNumUI(StageData stage) { stageNumText.text = stage.stageNumber.ToString(); }
    private void UpdateStageTimeUI(int cur) { stageTimeText.text = cur.ToString(); }
    private void UpdateEnemyCntUI(int cur) { enemyCntText.text = $"{cur} / {maxEnemyCnt}"; enemyCntFillImage.fillAmount = cur / maxEnemyFloatCnt; }

    private IEnumerator SpawnedBossTypeInfoRoutine(SpawnedBossType spawnedBossType)
    {
        if (spawnedBossType == SpawnedBossType.미니보스출현패널)
        {
            notifyMiniBossSpawnPanel.SetActive(true);
            for(int i=0; i<3; i++) {yield return oneSecond;}
            notifyMiniBossSpawnPanel.SetActive(false);
        }
        else
        {
            notifyBossSpawnPanel.SetActive(true);
            for(int i=0; i<5; i++) {yield return oneSecond;}
            notifyBossSpawnPanel.SetActive(false);
        }
        yield return null;
    }
}
