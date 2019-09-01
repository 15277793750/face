using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : Singleton<GameUIManager>
{
    public Dictionary<string, GameObject> winDic = new Dictionary<string, GameObject>();
    public List<string> hasOpenWinList = new List<string>();
    public Transform[] allWins;

    public void Open(string winName, object data = null)
    {
        foreach (var item in allWins)
        {
            if (item.name == winName)
            {
                print("Open: " + winName);
                item.transform.gameObject.SetActive(true);
                if (!hasOpenWinList.Contains(winName))
                {
                    item.transform.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
                    hasOpenWinList.Add(winName);
                }
                item.transform.SendMessage("Open", data == null ? new object() : data, SendMessageOptions.DontRequireReceiver);
                return;
            }
        }
    }

    public void Close(string winName)
    {
        GameObject win = GetWin(winName);
        if (win)
        {
            win.transform.SendMessage("Close", SendMessageOptions.DontRequireReceiver);
        }
    }

    public GameObject GetWin(string winName)
    {
        GameObject target = Utils.GetDicValueByKey(winName, winDic);
        if (target)
        {
            return target;
        }
        return null;
    }

    //public void Open(GameObject win, Person data)
    //{
    //    Winbase target = win.transform.GetComponent<Winbase>();
    //    if (target)
    //    {
    //        print("xxxxxxxxxxxxxxxxxx");
    //        target.Open(data);
    //    }


    //    // win
    //}
    void Start()
    {
        //transform.childCount
        allWins = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i).GetComponent<Transform>();
            allWins[i] = child;
            winDic.Add(child.name, child.gameObject);
        }
        Open("RoleInfoView");
        //   allWins = transform.GetComponentsInChildren<Transform>(false);
        //foreach (var item in allWins)
        //{
        //    print(item.name);
        //    //winDic.Add(item.name, item.gameObject);
        //}
        //GameObject target = Utils.GetDicValueByKey(winName, winList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
