using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleInfoView : Winbase
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
    }
    private void InitFunc()
    {
        this.transform.Find("btnExit").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Close();
        });

        this.transform.Find("btnTask").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("TaskMenuView");
        });
        this.transform.Find("btnBegin").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameUIManager.Instance.Open("SpaceShipSelectView");
        });
    }
    public override void Close()
    {
        base.Close();
    }
}
