using UnityEngine;

public class MythicCombPanel : MonoBehaviour
{
    private void OnEnable()
    {
        // 신화 조합 가능 체크
        for(int i = 4; i > -1; i--)
        {
            MythicUnit.instance.SelectMythic(MythicUnit.instance.mythicCombList[i].mythicType.ToString());
            
            bool isComb = true;
            for(int j = 0; j < 3; j++)
            {
                if(!MythicUnit.instance.requireImageList[j].transform.GetChild(0).gameObject.activeSelf)
                {
                    isComb = false;
                    break;
                }
            }

            MythicUnit.instance.mythicCombCheckList.gameObjectList[i].SetActive(isComb);
        }
    }
}
