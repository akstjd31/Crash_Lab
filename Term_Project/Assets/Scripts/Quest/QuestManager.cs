using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QuestID
{
    NPC = 0, Coin, Rabbit, Flower, Car, Path
}

public class QuestManager : MonoBehaviour
{
    [SerializeField] private int questId;
    public int coinCnt = 0;
    public int flowerCnt = 0;
    public Dictionary<int, QuestData> questList;
    [SerializeField] private Text questNameText, itemText, timeText;
    [SerializeField] private GameObject[] questItem;
    private bool questItemRecall = false;
    public bool gatherArea = false;
    public bool questClear = false;
    public float pathTime = 15.0f; // 다음 경로까지 남은 시간
    public int pathCnt;

    [SerializeField] private GameObject radish;

    private static QuestManager instance = null;

    void Awake() // 초기화
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        questList = new Dictionary<int, QuestData>();
        questId = 4;
        GenerateData();
    }

    void Start()
    {
        pathCnt = PathManager.Instance.GetPathCount();
    }

    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    void GenerateData() // 퀘스트 데이터 저장
    {   
        // Forest
        questList.Add(0, new QuestData("NPC 찾기"));
        questList.Add(1, new QuestData("10개 모아서 NPC 갖다주기"));
        questList.Add(2, new QuestData("토끼 유인해서 NPC 갖다주기"));
        questList.Add(3, new QuestData("해바라기 채집하기"));

        // City
        questList.Add(4, new QuestData("차 찾기"));
        questList.Add(5, new QuestData("지정된 경로 통과하기"));
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
    }

    void CheckQuest() // 현재 퀘스트가 완료되었다면 다음 퀘스트로 넘어감.
    {
        if (QuestClear(questId))
        {
            NextQuest();
            questClear = false;
        }
    }

    bool QuestClear(int id) // id에 해당하는 퀘스트가 완료했다면 true
    {
        if (questClear)
        {
            questItemRecall = false;
            MovePanel.currentTime = 0f; // 판넬 시간 초기화
            switch (id)
            {
                case (int)QuestID.NPC: break;
                case (int)QuestID.Coin:
                    coinCnt = 0;
                    break;
                case (int)QuestID.Rabbit:
                    FindNPC.NPCGetRabbit = false;
                    break;
                case (int)QuestID.Flower:
                    itemText.text = null;
                    flowerCnt = 0;
                    break;
                case (int)QuestID.Car: break;
                case (int)QuestID.Path:
                    itemText.text = null;
                    timeText.text = null;
                    break;
            }
            return true;
        }
        return false;
    }

    void StartQuest() // 임시로 게임을 시작한지 5초가 지나면 퀘스트 부여
    {
        /* 퀘스트마다 텍스트 길이 조절 */
        CanvasOnText();
        questNameText.text = QuestName(questId);
    }

    void CanvasOnText()
    {
        switch (GetQuestID())
        {
            case (int)QuestID.NPC:
                questNameText.alignment = TextAnchor.MiddleCenter;
                break;

            case (int)QuestID.Coin:
            case (int)QuestID.Rabbit:
                questNameText.fontSize = 14;
                questNameText.alignment = TextAnchor.UpperCenter;
                break;
            case (int)QuestID.Flower:
                questNameText.fontSize = 12;
                break;
            case (int)QuestID.Car:
                questNameText.fontSize = 20;
                break;
            case (int)QuestID.Path:
                questNameText.fontSize = 20;
                questNameText.alignment = TextAnchor.UpperCenter;

                itemText.fontSize = 18;
                itemText.alignment = TextAnchor.MiddleCenter;

                timeText.fontSize = 16;
                timeText.alignment = TextAnchor.LowerCenter;
                break;
            default: 
                questNameText.fontSize = 20;
                break;
        }
    }

    void ControlObject() // 퀘스트와 관련된 오브젝트들 수행
    {
        if (!questItemRecall)
        {
            if(GetQuestID() == (int)QuestID.Coin)
            {
                int maxCoin = 10;
                for (int i = 0; i < maxCoin; i++) Spawn();
            }
            else Spawn();
            questItemRecall = true;
        }

        switch (questId)
        {
            case (int)QuestID.NPC: break;
            case (int)QuestID.Coin:
                itemText.text = "모은 동전 갯수 : " + coinCnt;
                break;

            case (int)QuestID.Rabbit:
                if (QuestRabbit.getRadish) radish.SetActive(true); // 채소 활성화
                if (FindNPC.NPCGetRabbit) radish.SetActive(false);
                itemText.text = "떨어진 채소로 유인하자.";
                break;

            case (int)QuestID.Flower:
                itemText.text = "현재 채집한 해바라기 갯수 : " + flowerCnt;
                break;
            case (int)QuestID.Car: break;
            case (int)QuestID.Path:
                itemText.text = "남은 경로 : " + pathCnt;
                timeText.text = "다음 경로까지 남은 시간 : " + (int)pathTime + "초";
                pathTime -= Time.deltaTime;
                break;
        }
    }

    void Spawn()
    {
        if (GetQuestID() < 4) Instantiate(questItem[questId], new Vector3(Random.Range(-10, 10), questItem[questId].transform.position.y, Random.Range(-10, 10)), Quaternion.identity);
        //Instantiate(questItem[questId], new Vector3(Random.Range(-110, 105), 4f, Random.Range(-68, 100)), Quaternion.identity);
    }

    void Update()
    {
        StartQuest();
        CheckQuest();
    }
}
