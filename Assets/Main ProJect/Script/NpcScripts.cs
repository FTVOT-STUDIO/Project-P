using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScripts : MonoBehaviour
{
    public enum NpcScript { Engela }
    public NpcScript npcType;

    public void OnButtonClickScripts()
    {
        if (QuestManager.Instance.isToggleActive)
            return;
        switch (npcType)
        {
            case NpcScript.Engela:
                string[] line = { "공명할 세피라를 선택해봐요!\n각 세피라 별로 특별한\n능력을 부여받을 수 있어요!",
                 "저는 당신의 듬직한\n오퍼레이터 엔젤라 입니다~!",
                  "해방시키지 못한 세피라는\n퀘스트 클리어를 통해\n해방시킬 수 있어요!",
                   "설정 탭에는 생각보다\n유용한 기능들이 많이 있어요!" };
                QuestManager.Instance.infoText.text = line[Random.Range(0, line.Length)];
                break;
        }
    }
}
