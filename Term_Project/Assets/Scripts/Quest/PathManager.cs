using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathManager : MonoBehaviour
{
    [SerializeField] private GameObject[] path;     // 경로 오브젝트
    private int pNum = 0;                           // 경로 갯수(경로 오브젝트 인덱스 접근 사용)
    public int pathCnt;                             // 남은 경로

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
        Destroying();

        // 차에 탑승하면 시작
        if (QuestManager.Instance.GetQuestID() == (int)QuestID.Path) 
        {
            PathQuestStart();
        }  
    }

    void Destroying()
    {
        if (SceneManager.GetSceneByName("Gameover").isLoaded || SceneManager.GetSceneByName("Gameclaer").isLoaded) 
        {
            Destroy(gameObject);
        }
    }

    /* 경로를 하나씩 활성화 */
    void PathQuestStart()
    {
        path[pNum].SetActive(true);
    }

    /* 다음 경로 */
    public void NextPath()
    {
        pNum++;
    }

    /* 경로마다 거리 및 통과시간 계산 */
    public void CalPathTime()
    {
        if (pNum < 1) QuestManager.Instance.pathTime = 10.0f;
        else if (pNum == 1) QuestManager.Instance.pathTime = 30.0f;
        else if (pNum < 5) QuestManager.Instance.pathTime = 15.0f;
        else if (pNum < 13) QuestManager.Instance.pathTime = 10.0f;
        else QuestManager.Instance.pathTime = 20.0f;
    }

    /* Path Getter */
    public int GetPathCount()
    {
        return path.Length;
    }
}
