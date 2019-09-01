using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishSelRocketView : Winbase
{
    public override void Init()
    {
        base.Init();
        InitFunc();
    }
    public void InitFunc()
    {
        this.transform.Find("btnSel").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("LaunchSelView");
            GameUIManager.Instance.Close("SpaceRocketCheckView");
            GameUIManager.Instance.Close("SpaceRocketSelectView");
            Close();
        });
    }
    public override void Open(object data)
    {
        base.Open(data);
    }


    public override void Close()
    {
        base.Close();
    }
}
