using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCtrlManager : MonoBehaviour
{
    // 싱글톤
    public static SceneCtrlManager instance;
    [HideInInspector] public GameObject loadingBar;
    [HideInInspector] public Button startBtn;
    [HideInInspector] public Button exitBtn;
    private readonly WaitForSeconds delay = new WaitForSeconds(2.5f);
    public Texture2D cursorImg;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            // 씬 전환 시 파괴 X
            DontDestroyOnLoad(instance);

            // 마우스 커서 변경
            Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware);
        }
        else Destroy(gameObject);
    }

    private void SetLoadingBar() => loadingBar.transform.localPosition=(new Vector3(815, 462f, 0));
    
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