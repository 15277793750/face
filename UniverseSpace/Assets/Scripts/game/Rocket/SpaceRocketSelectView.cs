using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketStructure
{
    public int id;
    public RocketStructure(int id)
    {
        this.id = id;
    }
}
public class SpaceRocketSelectView : Winbase
{
    public List<RocketStructure> rocketData = new List<RocketStructure>();
    public GameObject svContent;
    public GameObject rocketItemPre;
    public GameObject[] rocketList;


    public override void Init()
    {
        base.Init();
        ////////////////////////// 假数据 //////////////////////////
        RocketStructure[] rocketTempData = new RocketStructure[10];

        rocketTempData[0] = new RocketStructure(0);
        rocketTempData[1] = new RocketStructure(1);
        rocketTempData[2] = new RocketStructure(2);
        rocketTempData[3] = new RocketStructure(3);
        rocketTempData[4] = new RocketStructure(4);
        rocketTempData[5] = new RocketStructure(5);
        rocketTempData[6] = new RocketStructure(6);
        rocketTempData[7] = new RocketStructure(7);
        rocketTempData[8] = new RocketStructure(8);
        rocketTempData[9] = new RocketStructure(9);
        ////////////////////////// //////////////////////////
        for (int i = 0; i < rocketTempData.Length; i++)
        {
            rocketData.Add(rocketTempData[i]);
        }
        print("rocketData.Count "+ rocketData.Count);
        rocketList = new GameObject[rocketData.Count];
        for (int i = 0; i < rocketData.Count; i++)
        {
            int index = i;
            rocketList[i] = GameObject.Instantiate(rocketItemPre, svContent.transform, false);
            rocketList[i].transform.Find("txtName").GetComponent<Text>().text = i + "火箭";
            rocketList[i].transform.Find("btnSel").GetComponent<Button>().onClick.AddListener(delegate
            {
                GameUIManager.Instance.Open("SpaceRocketCheckView", rocketData[index]);
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
