using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private enum PlayerResType
    {
        게임승리, 게임오버
    }

    // 싱글톤
    public static GameManager instance;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI stageScoreTxt;
    private readonly WaitForSeconds delay = new WaitForSeconds(2f);
    
    private void Awake()
    {
        instance = this;
        
        Application.targetFrameRate = 60;

    }
    // 게임 멈추기
    // 게임 클리어 및 실패에서 호출
    // 메인씬으로 가면 다시 타임스케일 돌려주기
    private void GameStop() => Time.timeScale = 0f;

    private IEnumerator ShowGameResultPanel(PlayerResType playerResType)
    {
        yield return delay;
        if (playerResType == PlayerResType.게임승리)
        {
            gameWinPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(false);
        }

        ShowStageRes();
    }
    
    public void PlayerGameWin()
    {
        GameStop();
        StartCoroutine(ShowGameResultPanel(PlayerResType.게임승리));
    }

    public void PlayerGameOver()
    {
        GameStop();
        StartCoroutine(ShowGameResultPanel(PlayerResType.게임오버));
    }

    private void ShowStageRes()=>  stageScoreTxt.text = $"클리어 스테이지 수 : {StageManager.instance.CurStage.ToString()}";
    
    //싱글톤으로 처리된 신컨트롤러매니저가 씬을 모두 돌았을 때 캐싱해제되는 문제를 위한 코드 호출 함수
    public void GameExitBtn()
    {
        Time.timeScale = 1f; // 타임스케일 복구
        SceneCtrlManager.instance.ExitGame();
    }

    public void MenuSceneBtn()
    {
        Time.timeScale = 1f; // 타임스케일 복구
        SceneCtrlManager.instance.LoadScene("MenuScene");
    }
}
