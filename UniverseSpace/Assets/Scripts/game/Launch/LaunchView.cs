using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchView : Winbase
{
    public GameObject countObj;
    public Text txtTime;
    public float time = 0;
    public bool countFinish = false;
    
    public void Update()
    {
        time -= Time.deltaTime;
        if (!countFinish)
        {
            if (time >= 0)
            {
                txtTime.text = time.ToString("f0");
                // Debug.Log(string.Format("Timer1 is up !!! time=${0}", Time.time));
                // timer = 1.0f;
            }
            else
            {
                countFinish = true;
            }
        }
        else
        {
            countObj.SetActive(false);
        }
    }
    public override void Init()
    {
        base.Init();
        InitFunc();
    }
    public void InitFunc()
    {
        this.transform.Find("btnSeparate").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (countFinish)
            {
                GameUIManager.Instance.Open("SeparateResultView");
                Close();
            }
            else
            {
                print("倒计时中！！！！！！！！！");
            }
        });
    }
    public override void Open(object data)
    {
        base.Open(data);
        countFinish = false;
        time = 5.0f;
    }

    public override void Close()
    {
        base.Close();
    }
    //IEnumerator Timer()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1.0f);
    //        Debug.Log(string.Format("Timer2 is up !!! time=${0}", Time.time));
    //    }
    //}

}
