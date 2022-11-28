using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private GameObject[] path;
    private int pNum = 0;

    private static PathManager instance = null;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.Instance.GetQuestID() == (int)QuestID.Path)
        {
            PathQuestStart();
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
