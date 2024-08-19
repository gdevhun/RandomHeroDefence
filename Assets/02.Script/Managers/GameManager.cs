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
    public void GamePause() => Time.timeScale = 0f; //게임정지
    public void GameResume() => Time.timeScale = 1f;

    // 임시 게임속도
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1f;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2f;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3f;
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrencyManager.instance.AcquireCurrency(1000, true);
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            CurrencyManager.instance.ConsumeCurrency(1000, true);
        }

        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            CurrencyManager.instance.AcquireCurrency(1000, false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            CurrencyManager.instance.ConsumeCurrency(1000, false);
        }
    }

    // 10초 뒤 게임 시작
    public IEnumerator GameStartRoutine()
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
            ShowStageRes(gameWinPanel);
        }
        else
        {
            gameOverPanel.SetActive(true);
            ShowStageRes(gameOverPanel);
        }

        yield return StageManager.instance.halfSecond;
        GamePause();
    }
    
    public void PlayerGameWin()
    {
        StartCoroutine(ShowGameResultPanel(PlayerResType.게임승리));
    }

    public void PlayerGameOver()
    {
        StartCoroutine(ShowGameResultPanel(PlayerResType.게임오버));
    }

    private void ShowStageRes(GameObject panel)
    {
        Transform clearedStageCnt = panel.transform.GetChild(1);
        if (clearedStageCnt.TryGetComponent(out TextMeshProUGUI inText))
        {
            inText.text = $"클리어 스테이지 수 : {(panel == gameWinPanel ? StageManager.instance.maxStage : StageManager.instance.CurStage).ToString()}";
        }
    } 
    
    //싱글톤으로 처리된 신컨트롤러매니저가 씬을 모두 돌았을 때 캐싱해제되는 문제를 위한 코드 호출 함수
    public void GameExitBtn()
    {
        GameResume(); // 타임스케일 복구
        SceneCtrlManager.instance.ExitGame();
    }

    public void MenuSceneBtn()
    {
        GameResume(); // 타임스케일 복구
        SceneCtrlManager.instance.LoadScene("MenuScene");
    }
}
