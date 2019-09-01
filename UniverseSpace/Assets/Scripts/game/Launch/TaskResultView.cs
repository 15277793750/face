using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskResultView : Winbase
{
    public bool isSuccess;
    public override void Init()
    {
        base.Init();
        InitFunc();
    }
    public void InitFunc()
    {
        this.transform.Find("btnReturn").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("RoleInfoView");
            GameUIManager.Instance.GetWin("BgView").GetComponent<BgView>().setBg("bg_1");
            Close();
        });
    }
    
    public override void Open(object data)
    {
        base.Open(data);
        isSuccess = true;
        if (isSuccess)
        {

        }
    }


    public override void Close()
    {
        base.Close();
    }
}
