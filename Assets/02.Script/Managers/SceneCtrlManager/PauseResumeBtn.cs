using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseResumeBtn : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI thisText;
    [SerializeField] ListGameObject pausePanelList = new ListGameObject();

    private void Start()
    {
        button = GetComponent<Button>();
        thisText = GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.AddListener(OnPlayerPause);
    }
    
    private void OnPlayerPause()
    {
        GameManager.instance.GamePause();
        UpdateButtonState("게임재개", OnPlayerResume, OnPlayerPause);

        for(int i = 0; i < pausePanelList.gameObjectList.Count; i++) pausePanelList.gameObjectList[i].SetActive(true);
    }

    private void OnPlayerResume()
    {
        GameManager.instance.GameResume();
        UpdateButtonState("게임정지", OnPlayerPause, OnPlayerResume);

        for(int i = 0; i < pausePanelList.gameObjectList.Count; i++) pausePanelList.gameObjectList[i].SetActive(false);
    }

    private void UpdateButtonState(string newText, UnityAction newAction, UnityAction oldAction)
    {
        thisText.text = newText;
        button.onClick.RemoveListener(oldAction);
        button.onClick.AddListener(newAction);
    }
}
