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

    void Awake() // �ʱ�ȭ
    {
        questList = new Dictionary<int, QuestData>();
        questId = 1;
        GenerateData();
    }

    void GenerateData() // ����Ʈ ������ ����
    {
        questList.Add(0, new QuestData("���� 10�� �Ա�"));
        questList.Add(1, new QuestData("�䳢 ã��"));
        questList.Add(2, new QuestData("�عٶ�� ä���ϱ�"));
        questList.Add(3, new QuestData("1"));
    }

    public int GetQuestID() // ���� ����Ʈ�� ID�� ��ȯ
    {
        return questId;
    }

    public string QuestName(int id) // ���� ����Ʈ���� ��ȯ
    {
        ControlObject();
        return questList[id].questName;
    }

    void NextQuest() // ���� ����Ʈ�� �Ѿ��
    {
        questId += 1;
        questClear = false;
    }

    void CheckQuest() // ���� ����Ʈ�� �Ϸ�Ǿ��ٸ� ���� ����Ʈ�� �Ѿ.
    {
        if (QuestClear(questId))
        {
            NextQuest();
        }
    }

    bool QuestClear(int id) // id�� �ش��ϴ� ����Ʈ�� �Ϸ��ߴٸ� true
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

    void StartQuest() // �ӽ÷� ������ �������� 5�ʰ� ������ ����Ʈ �ο�
    {
        if (Player.playTime > 5f)
        {
            questNameText.text = QuestName(questId);
        }
    }

    void ControlObject() // ����Ʈ�� ���õ� ������Ʈ�� ����
    {
        switch(questId)
        {
            case (int)QuestID.CoinQuest:
                itemText.text = "���� ȹ���� ���� ���� : " + coinCnt;
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
                itemText.text = "���� ä���� �عٶ�� ���� : " + flowerCnt;
                if (!questItemRecall)
                    SpawnFlower();

                questItemRecall = true;
                break;
                
        }
    }

    void SpawnCoin() // ���� ����Ʈ
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
