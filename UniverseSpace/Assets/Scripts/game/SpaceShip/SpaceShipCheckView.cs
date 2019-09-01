using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpaceShipCheckView : Winbase
{
    public override void Init()
    {
        base.Init();
        InitFunc();
    }
    public void InitFunc()
    {
        print("InitFunc CheckView");
        this.transform.Find("btnRename").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("SpaceShipRenameView");
        });
        this.transform.Find("btnNext").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("SpaceRocketSelectView");
            GameUIManager.Instance.Close("SpaceShipSelectView");
            Close();
        });
        this.transform.Find("btnClose").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Close();
        });
    }
    public override void Open(object data)
    {
        base.Open(data);
        UpdateView();

    }
    public void UpdateView()
    {
        ShipStructure data = (ShipStructure)winParams;
        this.transform.Find("txtName").GetComponent<Text>().text = data.id + "飞船";
    }

    public override void Close()
    {
        base.Close();
    }

}
