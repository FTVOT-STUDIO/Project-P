using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;
    [System.Serializable]
    public class Achievement
    {
        public string achiveName;
        public Slider slider;
        public int maxItems;
        public Text CountText;
        public GameObject rewardUI;
        public GameObject clearUI;
        [HideInInspector] public int currentItemCount = 0;
        [HideInInspector] public bool isAchieved = false;
    }
    public Achievement[] achievements;
    public GameObject allClearbadge;
    public GameObject[] ClearQuestPanel;

    private bool isSoundPlayed = false; // 중복 사운드 재생 방지용 변수

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateSliders();

        /*  if (Input.GetKey(KeyCode.Q))
          {
              for (int i = 0; i < achievements.Length - 1; i++)
              {
                  CollectItem(i);
              }

          } */
    }

    public void CollectItem(int achievementIndex)
    {
        if (achievementIndex < 0 || achievementIndex >= achievements.Length) return;

        Achievement achievement = achievements[achievementIndex];

        if (!achievement.isAchieved)
        {
            achievement.currentItemCount++;

            if (achievement.currentItemCount >= achievement.maxItems)
            {
                achievement.isAchieved = true;
                AchieveClear(achievement, achievementIndex);
            }
        }
        AllClearAchieve();
        UpdateItemCountText(achievement);
    }

    private void UpdateSliders()
    {
        foreach (Achievement achievement in achievements)
        {
            achievement.slider.value = (float)achievement.currentItemCount / achievement.maxItems;
            UpdateItemCountText(achievement);
        }
    }

    private void UpdateItemCountText(Achievement achievement)
    {
        if (achievement.CountText != null)
        {
            achievement.CountText.text = $"{achievement.currentItemCount}/{achievement.maxItems}";
        }
    }

    private void AchieveClear(Achievement achievement, int achievementIndex)
    {
        achievement.clearUI.SetActive(true);

        StartCoroutine(ClearQuest(achievement, achievementIndex));

        achievement.rewardUI.SetActive(false);

        GameManager.Instance.qeustClearCount++;

        GameManager.Instance.stagePoint += 3000;

        // 사운드가 한 번만 재생되도록 체크
        if (!isSoundPlayed)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.max);
            isSoundPlayed = true;
        }

        CollectItem(9);
    }

    void AllClearAchieve()
    {
        bool isAllClear = true;

        foreach (Achievement achievement in achievements)
        {
            if (!achievement.isAchieved)
            {
                isAllClear = false;
                break;
            }
        }

        if (isAllClear)
        {
            allClearbadge.SetActive(true);
        }
    }

    IEnumerator ClearQuest(Achievement achievement, int index)
    {
        ClearQuestPanel[index].SetActive(true);

        ClearQuestPanel[index].GetComponentInChildren<Text>().text = achievement.achiveName + " 퀘스트 클리어!";
        yield return new WaitForSeconds(3.5f);

        ClearQuestPanel[index].SetActive(false);
        isSoundPlayed = false;
    }

    public void AllClearCheat()
    {
        for (int i = 0; i < achievements.Length - 1; i++)
        {
            Achievement achievement = achievements[i];
            AchieveClear(achievement, i);
        }
        allClearbadge.SetActive(true);
    }
}
