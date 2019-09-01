﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchSenceFailView : Winbase
{
    public override void Init()
    {
        base.Init();
        InitFunc();
    }
    public void InitFunc()
    {
        this.transform.Find("btnConfirm").GetComponent<Button>().onClick.AddListener(delegate ()
        {
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
