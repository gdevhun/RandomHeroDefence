using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrlManager : MonoBehaviour
{
    // 싱글톤
    public static SceneCtrlManager instance;
    private GameObject loadingBar;
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

        loadingBar=GameObject.FindGameObjectWithTag("LoadingBar");
    }

    private void SetLoadingBar() => loadingBar.transform.localPosition=(new Vector3(815, 462f, 0));
    private void ResetLoadingBar() => loadingBar.transform.localPosition=(new Vector3(1064,462,0));
    
    public void AsyncLoadScene(string nextScene)
    {
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
