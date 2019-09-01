using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceRocketCheckView : Winbase
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
            GameUIManager.Instance.Open("FinishSelRocketView");
        });
        this.transform.Find("btnClose").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Close();
        });
    }
}
