using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel, sidePanel;
    [SerializeField] private Text[] mainText;
    [SerializeField] private Text sideText, playTimeText;

    private static CanvasManager instance;

    private int minute;
    void Awake()
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
    }
    public static CanvasManager Instance
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

    // Start is called before the first frame update
    void Start()
    {
        sideText.alignment = TextAnchor.MiddleCenter;
    }

    // Update is called once per frame
    void Update()
    {
        PlayTimeText();
        MainText();
        SideText();
        Settings();
        TextSettings();
    }

    public void SidePanelOff()
    {
        sidePanel.SetActive(false);
    }

    public void SidePanelOn()
    {
        sidePanel.SetActive(true);
    }

    public GameObject GetMainPanel()
    {
        return mainPanel;
    }

    public GameObject GetSidePanel()
    {
        return sidePanel;
    }

    public Text GetSideText()
    {
        return sideText;
    }

    void MainText()
    {
        switch (QuestManager.Instance.GetQuestID())
        {
            case (int)QuestID.NPC:
                mainText[0].text = "NPC 찾기";
                break;
            case (int)QuestID.Coin:
                mainText[0].text = "NPC가 떨어트린 동전 줍기";
                mainText[1].text = "모은 동전 갯수 : " + QuestManager.Instance.coinCnt + " / 10";
                break;

            case (int)QuestID.Rabbit:
                mainText[0].text = "NPC가 토끼를 잡을 수 있도록 돕기";
                mainText[1].text = "떨어진 채소로 유인하자.";
                break;

            case (int)QuestID.Flower:
                mainText[0].text = "해바라기캐서 NPC 갖다주기";
                if (!QuestFlowerCollection.getFlower) mainText[1].text = "채집 필요";
                else mainText[1].text = "채집 완료";
                break;

            case (int)QuestID.Car:
                mainText[1].text = null;
                mainText[0].text = "차 찾아서 탑승하기";
                break;

            case (int)QuestID.Path:
                mainText[0].text = "차타고 정해진 경로 이동하기";
                mainText[1].text = "남은 경로 : " + PathManager.Instance.pathCnt;
                mainText[2].text = "다음 경로까지 남은 시간 : " + (int)QuestManager.Instance.pathTime + "초";
                break;
        }
    }

    void SideText()
    {
        if (Player.isRiding)
            sideText.text = "F키 눌러서 하차하기";
        else if (!Player.isRiding)
            sideText.text = "F키 눌러서 승차하기";
    }

    void PlayTimeText()
    {
        playTimeText.text = "플레이 시간\n" + minute + "분 " + (int)QuestManager.Instance.playTime + "초";
        if ((int)QuestManager.Instance.playTime / 60 >= 1)
        {
            minute++;
            QuestManager.Instance.playTime = 0.0f;
        }
    }

    void Settings()
    {
        if (QuestManager.Instance.questClear) GetMainPanel().GetComponent<MovePanel>().currentTime = 0.0f;
    }

    void TextSettings()
    {
        switch (QuestManager.Instance.GetQuestID())
        {
            case (int)QuestID.NPC:
                mainText[0].alignment = TextAnchor.MiddleCenter;
                break;

            case (int)QuestID.Coin:
                mainText[0].alignment = TextAnchor.UpperCenter;
                mainText[0].fontSize = 18;

                mainText[1].alignment = TextAnchor.LowerCenter;
                mainText[1].fontSize = 20;
                break;

            case (int)QuestID.Rabbit:
                mainText[0].fontSize = 14;
                mainText[0].alignment = TextAnchor.UpperCenter;
                break;

            case (int)QuestID.Flower:
                mainText[0].alignment = TextAnchor.UpperCenter;
                mainText[0].fontSize = 18;

                mainText[1].alignment = TextAnchor.MiddleCenter;
                mainText[1].fontSize = 20;
                break;

            case (int)QuestID.Car:
                mainText[0].fontSize = 20;
                break;

            case (int)QuestID.Path:
                mainText[0].fontSize = 16;
                mainText[0].alignment = TextAnchor.UpperCenter;

                mainText[1].fontSize = 16;
                mainText[1].alignment = TextAnchor.MiddleCenter;

                mainText[2].fontSize = 14;
                mainText[2].alignment = TextAnchor.LowerCenter;
                break;
        }
    }
}
