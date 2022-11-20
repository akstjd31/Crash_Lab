using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum QuestID
{
    FindNPC = 0, CoinQuest, FindRabbit, FlowerCollection, 
} 

public class QuestManager : MonoBehaviour
{
    public int questId;
    public static int coinCnt = 0;
    public static int flowerCnt = 0;
    public static Dictionary<int, QuestData> questList;
    public Text questNameText, itemText;
    public GameObject[] questItem;
    public bool questItemRecall = false;
    public static bool gatherArea = false;
    public GameObject radish;

    void Awake() // �ʱ�ȭ
    {
        questList = new Dictionary<int, QuestData>();
        questId = 0;
        GenerateData();
    }

    void GenerateData() // ����Ʈ ������ ����
    {
        questList.Add(0, new QuestData("NPC ã��"));
        questList.Add(1, new QuestData("10�� ��Ƽ� NPC �����ֱ�"));
        questList.Add(2, new QuestData("�䳢 �����ؼ� NPC �����ֱ�"));
        questList.Add(3, new QuestData("�عٶ�� ä���ϱ�"));
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
        if (FindNPC.findNPC)
        {
            FindNPC.findNPC = false;
            questItemRecall = false;
            MovePanel.currentTime = 0f; // �ǳ� �ð� �ʱ�ȭ
            switch (id)
            {
                case (int)QuestID.FindNPC: break;
                case (int)QuestID.CoinQuest:
                    coinCnt = 0;
                    itemText.text = null;
                    break;
                case (int)QuestID.FindRabbit:
                    break;
                case (int)QuestID.FlowerCollection: break;
            }
            return true;
        }
        return false;
    }

    void StartQuest() // �ӽ÷� ������ �������� 5�ʰ� ������ ����Ʈ �ο�
    {
        /* ����Ʈ���� �ؽ�Ʈ ���� ���� */
        if (GetQuestID() == 1 || GetQuestID() == 2 || GetQuestID() == 3)
        {
            questNameText.fontSize = 14;
            questNameText.alignment = TextAnchor.UpperCenter;
        }
        else questNameText.fontSize = 20;
        questNameText.text = QuestName(questId);
    }

    void ControlObject() // ����Ʈ�� ���õ� ������Ʈ�� ����
    {
        if (!questItemRecall)
        {
            if(GetQuestID() == (int)QuestID.CoinQuest)
            {
                int maxCoin = 10;
                for (int i = 0; i < maxCoin; i++) Spawn();
            }
            else Spawn();
            questItemRecall = true;
        }

        switch (questId)
        {
            case (int)QuestID.FindNPC: break;
            case (int)QuestID.CoinQuest:
                itemText.text = "���� ���� ���� : " + coinCnt;
                break;

            case (int)QuestID.FindRabbit:
                if (QuestRabbit.getRadish) radish.SetActive(true); // ä�� Ȱ��ȭ
                if (FindNPC.isGetRabbit) radish.SetActive(false);
                itemText.text = "������ ä�ҷ� ��������.";
                break;

            case (int)QuestID.FlowerCollection:
                itemText.text = "���� ä���� �عٶ�� ���� : " + flowerCnt;
                break;
        }
    }

    void Spawn()
    {
        //Instantiate(questItem[questId], new Vector3(Random.Range(-10, 10), questItem[questId].transform.position.y, Random.Range(-10, 10)), Quaternion.identity);
        Instantiate(questItem[questId], new Vector3(Random.Range(-110, 105), 4f, Random.Range(-68, 100)), Quaternion.identity);
    }

    void Update()
    {
        StartQuest();
        CheckQuest();
    }
}
