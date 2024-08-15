using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicBtn : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button exitBtn;

    private void Start()
    {
        SceneCtrlManager.instance.loadingBar = GameObject.FindGameObjectWithTag("LoadingBar");
        startBtn.onClick.AddListener(() => SceneCtrlManager.instance.AsyncLoadScene("GameScene"));
        exitBtn.onClick.AddListener(SceneCtrlManager.instance.ExitGame);
        SoundManager.instance.BgmSoundPlay(BgmType.게임메뉴);
    }
}
