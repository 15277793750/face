using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
public enum MenuType
{
    type1,
    type2,
};

public enum TaskType
{
    finish,
    unfinish,
};

public class TaskMenuView : Winbase
{
    public UnityEngine.UI.Button[] menuBtn;
    public Color selColor;
    public Color normalColor;
    public MenuType selType;
    public GameObject scrollViewContent;

    public GameObject taskItemPre;//任务项预制体
    

    public Dictionary<int, List<TaskStructure>> taskDic = new Dictionary<int, List<TaskStructure>>();
    private void Start()
    {

       
        // ChangeSelView((int)selType);
    }
    public void ChangeSelView(int type)
    {
        print("bbbbbbbbbbbb");
        // List<TaskStructure> showData = new List<TaskStructure>();
        List<TaskStructure> showData = Utils.GetDicValueByKey(type, taskDic);
        // scrollView.Clear();

     //任务项列表
        for (int i = 0; i < scrollViewContent.transform.childCount; i++)
        {
            GameObject.Destroy(scrollViewContent.transform.GetChild(i).gameObject);
        }
        foreach (var item in showData)
        {
            print(item.id);
        }
       // print(showData.Count);
        GameObject[] scItemList = new GameObject[showData.Count];
        for (int i = 0; i < showData.Count; i++)
        {
            int index = i;
            scItemList[i] = GameObject.Instantiate(taskItemPre, scrollViewContent.transform, false);
            scItemList[i].AddComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate
            {
                GameUIManager.Instance.Open("TaskInfoView", showData[index]);
            }
            );
        }
    }
    public override void Init()
    {
        base.Init();

        selType = MenuType.type1;

        TaskStructure[] p = new TaskStructure[10];
        for (int i = 0; i < p.Length; i++)
        {
            p[i] = new TaskStructure(i, i % 2);
        }

        List<TaskStructure> typeList1 = new List<TaskStructure>();
        List<TaskStructure> typeList2 = new List<TaskStructure>();

        for (int i = 0; i < p.Length; i++)
        {
            if (p[i].type == (int)TaskType.finish)
            {
                typeList1.Add(p[i]);
            }
            else if (p[i].type == (int)TaskType.unfinish)
            {
                typeList2.Add(p[i]);
            }

        }
        taskDic.Add((int)TaskType.finish, typeList1);
        taskDic.Add((int)TaskType.unfinish, typeList2);
        print("aaaaaaaaaa");
    }
    public override void Open(object data)
    {
        base.Open(data);
        InitFunc();
        UpdateView();
    }

    private void InitFunc()
    {
        transform.Find("btnClose").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate ()
        {
            Close();
        });

        for (int i = 0; i < menuBtn.Length; i++)
        {
            int tempId = i;
            menuBtn[i].onClick.AddListener(delegate ()
            {
                ChangeSelMenu(tempId);
            }
            );
        }
    }
    public override void Close()
    {
        base.Close();
    }

    public void UpdateView()
    {
        ChangeSelMenu((int)selType);
    }

    public void SelCallBack(int idx)
    {
        ChangeSelView(idx);
        if (idx == (int)MenuType.type1)
        {
            print("sel 1111111");
        }
        else if (idx == (int)MenuType.type2)
        {
            print("sel 222222222");
        }
        //Utils.NoParamsDelegate a = delegate ()
        //{

        //};
    }
    public void ChangeSelMenu(int idx)
    {
        Text text;
        for (int i = 0; i < menuBtn.Length; i++)
        {
            if (idx == i)
            {
                Text selText = menuBtn[idx].transform.Find("Text").GetComponent<Text>();
                selText.color = selColor;
                selType = (MenuType)idx;
                SelCallBack(idx);
            }
            else
            {
                text = menuBtn[i].transform.Find("Text").GetComponent<Text>();
                text.color = normalColor;
            }
        }
    }
}
public class TaskStructure
{
    public int id;
    public int type;
    public Utils.NoParamsDelegate callBack;
    public TaskStructure(int id, int type)
    {
        this.id = id;
        this.type = type;
    }

    //public void SetCallBack(Utils.NoParamsDelegate callBack)
    //{
    //    this.callBack = callBack;
    //}
}

