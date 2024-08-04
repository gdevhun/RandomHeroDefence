using UnityEngine;

public class PercentInfoBtn : MonoBehaviour
{
    [Header ("확률 정보 패널")] [SerializeField] private GameObject percentInfoPanel;

    public void OnOffPanel(bool isInfoBtn)
    {
        if(!isInfoBtn) percentInfoPanel.SetActive(isInfoBtn);
        else
        {
            percentInfoPanel.SetActive(!percentInfoPanel.activeSelf);
            SoundManager.instance.SFXPlay(SoundType.Click);
        }
    }
}
