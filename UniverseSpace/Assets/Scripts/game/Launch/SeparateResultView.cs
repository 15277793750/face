using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SeparateResultView : Winbase
{
    public Text txtDesc;
    private bool isSuccess;
    public override void Init()
    {
        base.Init();
        InitFunc();
    }
    public void InitFunc()
    {
        this.transform.Find("btnCheck").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("GradeResultView");
            Close();
        });
    }
    public override void Open(object data)
    {
        base.Open(data);
        isSuccess = true;

        if (isSuccess)
        {
            txtDesc.text = "恭喜你！\n分离成功！";
        }
        else
        {
            txtDesc.text = "很遗憾！\n发射失败！";
        }
        
    }


    public override void Close()
    {
        base.Close();
    }
}
