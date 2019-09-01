using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpaceShipRenameView : Winbase
{
    public Text text;
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
        InitFunc();

    }
    public void InitFunc()
    {
        this.transform.Find("btnComfirm").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            // GameUIManager.Instance.Open("ShipRenameView");
            print("您输入的飞船名字为：" + text.text);
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
   
    }

    public override void Close()
    {
        base.Close();
    }
}
