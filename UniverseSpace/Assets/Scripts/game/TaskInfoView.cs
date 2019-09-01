using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaskInfoView : Winbase
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Open(object data)
    {
        base.Open(data);
        InitFunc();
        print("child Open");
    }


    private void InitFunc()
    {
        this.transform.Find("btnClose").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Close();
        });

        this.transform.Find("btnNext").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("SpaceShipSelectView");
            GameUIManager.Instance.Close("TaskMenuView");
            Close();
        });
        this.transform.Find("btnReset").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("SpaceShipSelectView");
            GameUIManager.Instance.Close("TaskMenuView");
            Close();
        });
    }
    public override void Close()
    {
        base.Close();
    }

}
