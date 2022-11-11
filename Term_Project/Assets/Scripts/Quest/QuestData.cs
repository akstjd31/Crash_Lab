using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string questName;
    public static int questCount = 0;
    
    public QuestData()
    {

    }

    public QuestData(string name)
    {
        questName = name;
    }
}
