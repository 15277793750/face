using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LaunchSenceStructure
{
    public int id;
    public LaunchSenceStructure(int id)
    {
        this.id = id;
    }
}
public class LaunchSelView : Winbase
{

    public List<RocketStructure> launchSenceData = new List<RocketStructure>();
    public GameObject svContent;
    public GameObject launchItemPre;
    public GameObject[] launchSenceList;
    public Text txtName;

    public override void Init()
    {
        base.Init();
        ////////////////////////// 假数据 //////////////////////////
        RocketStructure[] launchTempData = new RocketStructure[10];

        launchTempData[0] = new RocketStructure(0);
        launchTempData[1] = new RocketStructure(1);
        launchTempData[2] = new RocketStructure(2);
        launchTempData[3] = new RocketStructure(3);
        launchTempData[4] = new RocketStructure(4);
        ////////////////////////// //////////////////////////
        for (int i = 0; i < launchTempData.Length; i++)
        {
            launchSenceData.Add(launchTempData[i]);
        }
        print("launchSenceData.Count " + launchSenceData.Count);
        launchSenceList = new GameObject[launchSenceData.Count];
        for (int i = 0; i < launchSenceData.Count; i++)
        {
            int index = i;
            launchSenceList[i] = GameObject.Instantiate(launchItemPre, svContent.transform, false);
            launchSenceList[i].transform.Find("imgSence").GetComponent<Button>().onClick.AddListener(delegate
            {
                GameUIManager.Instance.Open("LaunchCfmView", launchSenceData[index]);
            }
            );
        }
    }
    public override void Open(object data)
    {
        base.Open(data);
        InitFunc();
        UpdateView();

    }

    public override void Close()
    {
        base.Close();
    }

    public void InitFunc()
    {

    }
    public void UpdateView()
    {

    }
}
