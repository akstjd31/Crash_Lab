using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathManager : MonoBehaviour
{
    [SerializeField] private GameObject[] path;
    private int pNum = 0;
    public int pathCnt;

    private static PathManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static PathManager Instance
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
        pathCnt = path.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.Instance.GetQuestID() == (int)QuestID.Path)
        {
            PathQuestStart();
        }

        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3) // 게임오버 : 2, 게임클리어 : 3
        {
            Destroy(gameObject);
        }
    }
    void PathQuestStart()
    {
        path[pNum].SetActive(true);
    }

    public void NextPath()
    {
        pNum++;
    }

    public void CalPathTime()
    {
        if (pNum < 1) QuestManager.Instance.pathTime = 10.0f;
        else if (pNum == 1) QuestManager.Instance.pathTime = 30.0f;
        else if (pNum < 5) QuestManager.Instance.pathTime = 15.0f;
        else if (pNum < 13) QuestManager.Instance.pathTime = 10.0f;
        else QuestManager.Instance.pathTime = 20.0f;
    }

    public int GetPathCount()
    {
        return path.Length;
    }
}
