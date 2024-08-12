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

    // 게임씬으로 올 때 설정 필드
    [SerializeField] private Image bgmImage, sfxImage;
    [SerializeField] private Slider bgmSlider, sfxSlider;
    
    private void Awake()
    {
        instance = this;
        
        Application.targetFrameRate = 60;

        // 게임씬으로 올 때 사운드 매니저 설정
        SoundManager.instance.bgmImg = bgmImage;
        SoundManager.instance.sfxImg = sfxImage;
        bgmSlider.onValueChanged.AddListener(SoundManager.instance.SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.instance.SetSfxVolume);
    }
    // 게임 멈추기
    // 게임 클리어 및 실패에서 호출
    // 메인씬으로 가면 다시 타임스케일 돌려주기
    private void GameStop() => Time.timeScale = 0f;

    // 10초 뒤 게임 시작
    public IEnumerator GameStartCo()
    {
        for(int i = 0; i < 10; i++) yield return StageManager.instance.oneSecond;
        StageManager.instance.StartStage(StageManager.instance.CurStage);
    }

    private IEnumerator ShowGameResultPanel(PlayerResType playerResType)
    {
        yield return StageManager.instance.oneSecond;
        if (playerResType == PlayerResType.게임승리)
        {
            gameWinPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
        ShowStageRes();

        yield return StageManager.instance.halfSecond;
        GameStop();
    }
    
    public void PlayerGameWin()
    {
        StartCoroutine(ShowGameResultPanel(PlayerResType.게임승리));
    }

    public void PlayerGameOver()
    {
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
