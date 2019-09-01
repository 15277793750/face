using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgView : Winbase
{
    public Image imgBg;
    public override void Init()
    {
        base.Init();
        //setBg("img_bg_1");
    }
    public override void Open(object data)
    {
        base.Open(data);
    }

    public void setBg(string bgName)
    {
        //Texture2D imgTexture = Resources.Load("bgName") as Texture2D;
        //Sprite sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0.5f, 0.5f));
        //imgBg.sprite = sprite;
        // imgBg.load
    }
    public override void Close()
    {
        base.Close();
    }
}
