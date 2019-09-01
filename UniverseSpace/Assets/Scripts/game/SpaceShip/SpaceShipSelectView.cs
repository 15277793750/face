using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuSubType
{
    allType,
    type1,
    type2,
};
public class SpaceShipSelectView : Winbase
{

    public Button[] menuBtn;
    public Button[] menuSubBtn;
    public Color selColor;
    public Color normalColor;
    public MenuType selTabType;
    public MenuSubType selSubype;
    public GameObject svContent;
    public GameObject shipItemPre;

    public Dictionary<int, List<ShipStructure>> ShipDic = new Dictionary<int, List<ShipStructure>>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        base.Init();

        selTabType = MenuType.type1;
        selSubype = MenuSubType.allType;


        ShipStructure[] ShipData = new ShipStructure[10];
        //for (int i = 0; i < ShipData.Length; i++)
        //{
        //    ShipData[i] = new ShipStructure(i, i, i % 2);
        //}
        ShipData[0] = new ShipStructure(0, 0, 1);
        ShipData[1] = new ShipStructure(1, 0, 1);
        ShipData[2] = new ShipStructure(2, 0, 2);
        ShipData[3] = new ShipStructure(3, 0, 2);
        ShipData[4] = new ShipStructure(4, 0, 2);
        ShipData[5] = new ShipStructure(5, 1, 1);
        ShipData[6] = new ShipStructure(6, 1, 1);
        ShipData[7] = new ShipStructure(7, 1, 2);
        ShipData[8] = new ShipStructure(8, 1, 2);
        ShipData[9] = new ShipStructure(9, 1, 2);


        List<ShipStructure> typeList1 = new List<ShipStructure>();
        List<ShipStructure> typeList2 = new List<ShipStructure>();

        for (int i = 0; i < ShipData.Length; i++)
        {
            if (ShipData[i].tabType == (int)MenuType.type1)
            {
                typeList1.Add(ShipData[i]);
            }
            else if (ShipData[i].tabType == (int)MenuType.type2)
            {
                typeList2.Add(ShipData[i]);
            }

        }
        ShipDic.Add((int)MenuType.type1, typeList1);
        ShipDic.Add((int)MenuType.type2, typeList2);
    }

    public override void Open(object data)
    {
        base.Open(data);
        InitFunc();
        UpdateView();

    }
    public void UpdateView()
    {
        ChangeSelTabMenu((int)selTabType);
    }

    public void InitFunc()
    {
        print("InitFunc");
        for (int i = 0; i < menuBtn.Length; i++)
        {
            int tempId = i;
            menuBtn[i].onClick.AddListener(delegate ()
            {
                ChangeSelTabMenu((int)tempId);
            }
            );
        }
        for (int i = 0; i < menuSubBtn.Length; i++)
        {
            int tempId = i;
            menuSubBtn[i].onClick.AddListener(delegate ()
            {
                SelSubType((int)tempId);
            }
            );
        }
    }
    public void ChangeSelTabMenu(int idx)
    {
        selTabType = (MenuType)idx;
        print(idx + " idxidxidxidx");

        Image selImg;
        for (int i = 0; i < menuBtn.Length; i++)
        {
            if (idx == i)
            {
                Image img = menuBtn[idx].GetComponent<Image>();//.transform.Find("Image").
                                                               //  Button btn = menuBtn[idx].transform.Find("Button").GetComponent<Button>();
                img.color = selColor;
                selTabType = (MenuType)idx;
                selSubype = MenuSubType.allType;
                SelSubType((int)selSubype);
            }
            else
            {
                selImg = menuBtn[i].GetComponent<Image>();
                selImg.color = normalColor;
            }
        }
    }
    
    public void SelSubType(int subType)
    {
        selSubype = (MenuSubType)subType;
        Image img;
        for (int i = 0; i < menuSubBtn.Length; i++)
        {
            img = menuSubBtn[i].GetComponent<Image>();
            img.color = subType == i ? selColor : normalColor;
        }
        List<ShipStructure> showAllData = Utils.GetDicValueByKey((int)selTabType, ShipDic);

        List<ShipStructure> showData = new List<ShipStructure>();
        for (int i = 0; i < showAllData.Count; i++)
        {
            if (selSubype == MenuSubType.allType)
            {
                showData.Add(showAllData[i]);
            }
            else
            {
                if (showAllData[i].subType == (int)selSubype)
                {
                    showData.Add(showAllData[i]);
                }
            }
          
        }

        foreach (var item in showData)
        {
            print(item.id);
        }
        for (int i = 0; i < svContent.transform.childCount; i++)
        {
            GameObject.Destroy(svContent.transform.GetChild(i).gameObject);
        }
        GameObject[] svItemList = new GameObject[showData.Count];
       
        for (int i = 0; i < showData.Count; i++)
        {
            int index = i;
            svItemList[i] = GameObject.Instantiate(shipItemPre, svContent.transform, false);
            svItemList[i].transform.Find("txtName").GetComponent<Text>().text = i+"飞船";
            svItemList[i].transform.Find("btnSel").GetComponent<Button>().onClick.AddListener(delegate
            {
                GameUIManager.Instance.Open("SpaceShipCheckView", showData[index]);
            }
            );
        }
    }
    public override void Close()
    {
        base.Close();
    }
}

/// <summary>
/// 飞船结构
/// </summary>
public class ShipStructure
{
    public int tabType;
    public int subType;
    public int id;
    public Utils.NoParamsDelegate callBack;
    public ShipStructure(int id, int tabType, int subType)
    {
        this.id = id;
        this.tabType = tabType;
        this.subType = subType;
    }
}
