using UnityEngine;

// 스테이지 데이터
[CreateAssetMenu(menuName = "StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber;
    public int stageTime;
    public StageType stageType;
    public EnemyType enemyType;
    public int stageCurrency;
    public ListGameObject spawnPos = new ListGameObject();
}
