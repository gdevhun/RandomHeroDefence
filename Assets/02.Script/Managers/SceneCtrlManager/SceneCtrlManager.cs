using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCtrlManager : MonoBehaviour
{
    // 싱글톤
    public static SceneCtrlManager instance;
    
    [HideInInspector] public GameObject introEffect;
    [HideInInspector] public GameObject loadingCircle;
    [HideInInspector] public Button startBtn;
    [HideInInspector] public Button exitBtn;
    
    private readonly WaitForSeconds delay = new WaitForSeconds(2.5f);
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            // 씬 전환 시 파괴 X
            DontDestroyOnLoad(instance);

        }
        else Destroy(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {   //씬이 로드되면 호출 되는 (이벤트 리스너) 함수
        
        // 메뉴씬이 완전히 로드된 후 호출될 코드
        if (scene.name == "MenuScene")
        {
            OnMenuSceneLoaded();
        }
        // 게임씬이 완전히 로드된 후 호출될 코드
        else if (scene.name == "GameScene") 
        {
            OnGameSceneLoaded();
        }
    }
    private void CheckDontDestroyOnLoad(GameObject obj)
    {
        if (obj.scene.name != "DontDestroyOnLoad")
        {
            DontDestroyOnLoad(loadingCircle); 
            DontDestroyOnLoad(introEffect);
        }
    }
    private void OnMenuSceneLoaded()
    {
        SoundManager.instance.BgmSoundPlay(BgmType.게임메뉴);
        
        loadingCircle = GameObject.FindGameObjectWithTag("LoadingCircle");
        introEffect = GameObject.FindGameObjectWithTag("IntroEffect");
        loadingCircle.SetActive(true);
        introEffect.SetActive(true);
        
        CheckDontDestroyOnLoad(introEffect);
        CheckDontDestroyOnLoad(loadingCircle);
        
        startBtn = GameObject.FindGameObjectWithTag("StartBtn").GetComponent<Button>();
        exitBtn = GameObject.FindGameObjectWithTag("ExitBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(() => AsyncLoadScene("GameScene"));
        exitBtn.onClick.AddListener(ExitGame);
    }


    private void OnGameSceneLoaded()
    {
        loadingCircle.SetActive(false);
        introEffect.SetActive(false);
        
    }

    private void SetLoadingBar() => loadingCircle.transform.localPosition=(new Vector3(815, 462f, 0));
    private void ResetLoadingBar() => loadingCircle.transform.localPosition=(new Vector3(1064,462,0));
    
    public void AsyncLoadScene(string nextScene)
    {
        startBtn.interactable = exitBtn.interactable = false;
        SetLoadingBar();
        StartCoroutine(AsyncLoadSceneRoutine(nextScene));
    }

    private IEnumerator AsyncLoadSceneRoutine(string nextScene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene);
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    yield return delay;
                    ResetLoadingBar();
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
