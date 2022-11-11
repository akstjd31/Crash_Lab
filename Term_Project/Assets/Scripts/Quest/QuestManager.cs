using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum QuestID
{
    CoinQuest = 0, FindRabbit, FlowerCollection
} 

public class QuestManager : MonoBehaviour
{
    public int questId;
    public static int coinCnt = 0;
    public static int flowerCnt = 0;
    public static Dictionary<int, QuestData> questList;
    public Text questNameText, itemText;
    public GameObject[] questItem;
    public UnityEngine.AI.NavMeshAgent rabbitAgent;
    public bool questItemRecall = false;
    public static bool questClear = false;
    public static bool gatherArea = false;

    void Awake() // 초기화
    {
        questList = new Dictionary<int, QuestData>();
        questId = 1;
        GenerateData();
    }

    void GenerateData() // 퀘스트 데이터 저장
    {
        questList.Add(0, new QuestData("코인 10개 먹기"));
        questList.Add(1, new QuestData("토끼 찾기"));
        questList.Add(2, new QuestData("해바라기 채집하기"));
        questList.Add(3, new QuestData("1"));
    }

    public int GetQuestID() // 현재 퀘스트의 ID를 반환
    {
        return questId;
    }

    public string QuestName(int id) // 현재 퀘스트명을 반환
    {
        ControlObject();
        return questList[id].questName;
    }

    void NextQuest() // 다음 퀘스트로 넘어가기
    {
        questId += 1;
        questClear = false;
    }

    void CheckQuest() // 현재 퀘스트가 완료되었다면 다음 퀘스트로 넘어감.
    {
        if (QuestClear(questId))
        {
            NextQuest();
        }
    }

    bool QuestClear(int id) // id에 해당하는 퀘스트가 완료했다면 true
    {
        switch (id) {
            case (int)QuestID.CoinQuest:
                if (coinCnt == 10)
                {
                    questClear = true;
                    questItemRecall = false;
                    itemText.text = null;
                    return true;
                }
                break;
            case (int)QuestID.FindRabbit:
                if (questClear)
                {
                    questItemRecall = false;
                    return true;
                }
            break;

            case (int)QuestID.FlowerCollection:
                if (flowerCnt == 1)
                {
                    questItemRecall = false;
                    return true;
                }
                break;
        }
        return false;
    }

    void StartQuest() // 임시로 게임을 시작한지 5초가 지나면 퀘스트 부여
    {
        if (Player.playTime > 5f)
        {
            questNameText.text = QuestName(questId);
        }
    }

    void ControlObject() // 퀘스트와 관련된 오브젝트들 수행
    {
        switch(questId)
        {
            case (int)QuestID.CoinQuest:
                itemText.text = "현재 획득한 코인 갯수 : " + coinCnt;
                if (!questItemRecall)
                    SpawnCoin();

                questItemRecall = true;
                break;
            case (int)QuestID.FindRabbit:
                if (!questItemRecall)
                    SpawnRabbit();

                questItemRecall = true;
                break;

            case (int)QuestID.FlowerCollection:
                itemText.text = "현재 채집한 해바라기 갯수 : " + flowerCnt;
                if (!questItemRecall)
                    SpawnFlower();

                questItemRecall = true;
                break;
                
        }
    }

    void SpawnCoin() // 코인 퀘스트
    {
        int maxCoin = 10;
        for(int i = 0; i < maxCoin; i++)
        {
            Instantiate(questItem[questId], new Vector3(Random.Range(-15, 15), 2, Random.Range(-15, 15)), Quaternion.identity);
        }
    }

    void SpawnRabbit()
    {
        Instantiate(questItem[questId], new Vector3(Random.Range(-15, 15), 0.5f, Random.Range(-15, 15)), Quaternion.identity);
        if (QuestRabbit.isSafePos) rabbitAgent.SetDestination(QuestRabbit.randPos);
    }

    void SpawnFlower()
    {
        Instantiate(questItem[questId], new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15)), Quaternion.identity);
    }

    void Update()
    { 
        StartQuest();
        CheckQuest();
    }
}
