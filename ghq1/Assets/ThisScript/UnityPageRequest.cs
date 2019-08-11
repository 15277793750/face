using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using LitJson;
using System.Text;
using proto.FaceMesage;
using ProtoBuf;
using System.Net.NetworkInformation;
using rongyi.report;
using rongyi.face.log;
using UnityEngine.UI;

public class UnityPageRequest : MySingleton<UnityPageRequest>
{
    public MeshRenderer m;
    public RawImage img;
    string jsonDataPost; //jison格式数据
    private int StartIndex = 1;//程序启动后的递增数列

    //测试
    public Texture2D t2d;
    public int testWidth;
    public int testHeight;
    public int testX;
    public int testY;

    public Text txt;

    private string mac;  //本机mac
    //IEnumerator Start()
    //{
    //    var url = "https://www.baidu.com";
    //    var www = UnityWebRequest.Get(url);
    //    yield return www.SendWebRequest();

    //    if (www.isHttpError || www.isNetworkError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Debug.Log(www.downloadHandler.text);
    //    }
    //}

    public const string url = "http://192.168.1.104:8181/data/report/screen_detector_logs1?access_token=1234";
    public const string ur2 = "http://api.rongyi.com/data/report/screen_detector_log?";  //人脸数据上报地址
    //public const string ur2 = "http://manage.preview.rongyi.com/data/report/screen_detector_log?";

    //public const string ur2 = "http://47.102.131.178:8181/data/report/screen_detector_log";
    //public const string ur2 = "http://47.102.131.178:8181/data/report/screen_detector_log_2";

    //public const string ur3 = "http://rongyi.b0.rongyi.com/ruxin/arGame/CC-4B-73-7B-DE-86/argame.png";
    public const string ur3 = "http://rongyi.b0.rongyi.com/ruxin/arGame/CC-4B-73-7B-DE-86/argame.png"; //图片
    //public const string ur3 = "http://rongyi.b0.rongyi.com/ruxin/arGame/CC-4B-73-7B-DE-86/argame.png"; 



    void Start()
    {
        //StartCoroutine(StartDownload(ur3, (tex) =>
        //{
        //    print("接收完毕");
        //}));

        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var item in networkInterfaces)
        {
           Debug.Log("Name = " + item.Name);
           Debug.Log("Des = " + item.Description);
           Debug.Log("Type = " + item.NetworkInterfaceType.ToString());
           Debug.Log("Mac = " + item.GetPhysicalAddress().ToString());
        }
        mac = networkInterfaces[0].GetPhysicalAddress().ToString();
        //txt.text = mac;
        //解析字符串
        string s = "";
        int index = 0;  
        for (int i = 0; i < mac.Length;)
        {
            index++;
            if(index <= 2)//分割字符的个数
            {
                s += mac[i];
                i++;
            }
            else
            {
                s += ":"; //插入分号
                index = 0;
            }
        }
        mac = s;
        //txt.text += "  "+mac;

        //测试
        //StartCoroutine(SendHttpMessage(false));
        // string s = "CgtzZGZhc2RmYXNkZhCP4azlwi0aHHJvbmd5aS5mYWNlLmxvZy5Vc2VyRmFjZUluZm8iagoMaWQxNTYzNzYxODEwEIjhrOXCLSIKdGFkY3NkZjIzNCoOEP7grOXCLRi2+Kzlwi0qDhD+4Kzlwi0Ytvis5cItKhQKBAgXGAIQgeGs5cItGLn4rOXCLTIRYjA6ZjE6ZWM6M2U6N2M6ZjI=";
        // byte[] b = UnBase64String(s);
        // print(Encoding.UTF8.GetString(b));
        // //MemoryStream ms = new MemoryStream();
        // //Serializer.Serialize(ms, b);
        // //byte[] c = ms.ToArray();

        // byte[] pkg = SocketManager.ProtoBuf_Deserialize<byte[]>(b);

        // print(pkg.Length);
        // //print(pkg.ToString());
        //UnityPageRequest.Instance.RequiretBgLoad();
        //img.rectTransform.sizeDelta = new Vector2(testWidth, testHeight);
        //img.texture = CutFacePicture(t2d, testWidth, testHeight, testX, testY);
        //img.material.mainTexture = ;
    }

    //请求背景图片下载
    public void RequiretBgLoad()
    {
        StartCoroutine(StartDownload(ur3, (tex) =>
        {
            print("接收完毕");
        }));
    }
    
    public IEnumerator StartDownload(string ur3, Action<Texture2D> act)
    {
        WWW www = new WWW(ur3);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
            yield return null;
        }
        Texture2D newTexture = www.texture;
        byte[] pngData = newTexture.EncodeToPNG();
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                print("安卓");
                File.WriteAllBytes(Application.persistentDataPath + "/ICO.png", pngData);

            }
            else
            {
                print("非安卓");
                //File.WriteAllBytes(Application.dataPath + "/download/ICO.png", pngData);
                File.WriteAllBytes(Application.dataPath + "/TitleBg.png", pngData);
            }
        }
        catch (IOException e)
        {
            Debug.LogError(e);
        }
        img.texture = www.texture;
        Debug.Log("释放资源");
        // 释放资源
        www.Dispose();
        

        //发送字段
        //var form = new WWWForm();
        //form.AddBinaryData("screenshot", bytes);
        //form.AddBinaryData("msg", SendFaceMessage());
        ////form.AddField("name", "王老五");
        ////form.AddField("password", "这是密码");
        //form.AddBinaryData("msg", SendFaceMessage());//SendFaceMessage 返回的是整调数据的byte数组
        ////form.AddField("passoword", "username");
        //var www = UnityWebRequest.Post(url, form);
        //www.AddHeader("Content-Type", "application/json; charset=utf-8");
        //www.RequestFormat = DataFormat.Json;
        //yield return www.SendWebRequest();
        //if (www.isHttpError || www.isNetworkError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    print("成功收到返回");
        //    Debug.Log(www.downloadHandler.text);
        //}
    }

    //请求上报人脸数据
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isPostPicture">是否上传人脸照片</param>
    /// <returns></returns>
    public IEnumerator SendHttpMessage(bool isPostPicture)
    {
        // url可以是网络网址，也可以是本地网址
        Dictionary<string, string> header = new Dictionary<string, string>();
        // JsonDic.Add("Content-Type", "application/json");//json 类型
        header.Add("Content-Type", "application/octet-stream");//protobuf 类型
        //header.Add("Content-Type", "application/x-protobuf");//protobuf 类型
        //header.Add("Content-Type", "application/json");//protobuf 类型
        
        header.Add("access-token", "eyJVc2VyIjoi5p2l5LyK5Lu9LeeBteaipi3kurrohLgiLCJleHAiOjE1OTY3MDQ5MjV9.6g0nsJ2TLGO6aguQrH4upgPyGPBj1XMyNduC3_2XEWU");
        // JsonDic.Add("access_token", "eyJVc2VyIjoi5p2l5LyK5Lu9LeeBteaipi3kurrohLgiLCJleHAiOjE1OTUwNTEwMjZ9.6TKsWL65nN72QRS6G3eTFKXRBkHzlaYJ0idQEMk3C5c");
        
        // //WWW www = new WWW(ur2, Encoding.UTF8.GetBytes(jsonDataPost), JsonDic);
        // WWW www = new WWW(ur2, SendFaceMessage(isPostPicture), JsonDic);
        WWW www = new WWW(ur2, SendMsg(isPostPicture),header);//url  byty数据  header添加
        print("数据请求中");
        //检测是否下载完毕，也可以通过IsDone函数检测
        yield return www;
        print("请求完毕");
        if (www.error != null)
        {
            Debug.LogError(www.error);
            yield return null;
        }
        Debug.Log(www.text);
    }

    public byte[] SendMsg(bool isPostPicture)
    {
        UniversalReportRequest reportRequest = new UniversalReportRequest();
        reportRequest.id = mac + MyTimeTool.GetTimeStamp(false);
        reportRequest.timestamp = MyTimeTool.GetTimeStamp(false);
        reportRequest.message_name = "rongyi.face.log.UserFaceInfo";// rongyi.face.log.UserFaceInfo
        List<byte[]> a = new List<byte[]>();
        for (int i = 0; i < 1; i++)
        {
            a.Add(SendFaceMessage(isPostPicture));
        }
        reportRequest.message_data = a;

        MemoryStream ms = new MemoryStream();
        Serializer.Serialize(ms, reportRequest);
        byte[] b = ms.ToArray();
        return b;
    }

    public byte[] SendFaceMessage(bool isPostPicture)
    {
        UserFaceInfo userFaceInfo = new UserFaceInfo();
        string c = ""; //byte类型字段的测试数据
        byte[] t = Encoding.UTF8.GetBytes(c);
        //byte[] t = new byte[1];
        //t[0] = 1;
        userFaceInfo.id = Encoding.UTF8.GetBytes(Md5Sum(mac + MyTimeTool.GetTimeStamp(false) + StartIndex.ToString()));//Encoding.UTF8.GetBytes("10000")
        StartIndex += 1;
        userFaceInfo.timestamp = MyTimeTool.GetTimeStamp(false);
        userFaceInfo.mall_id = t;
        userFaceInfo.terminal_id = t;
        userFaceInfo.terminal_mac = Encoding.UTF8.GetBytes(mac);//"E0:D5:5E:C0:DA:CA"

        userFaceInfo.face = new List<CapturedUserFace>();
        CapturedUserFace cuf = new CapturedUserFace();
        for (int i = 0; i < GameReady.Instance.picturesList.Count; i++)
        {
            cuf.profile = new UserProfile();
            int age = UnityEngine.Random.Range(18, 23);
            cuf.profile.begin_age = age;
            cuf.profile.end_age = age;
            int sex = UnityEngine.Random.Range(0, 2);
            cuf.profile.gender = sex == 0? Gender.GENDER_MALE:Gender.GENDER_FEMALE;
            //cuf.profile.face_code = t;
            cuf.profile.expression = UserExpression.Happy;
            cuf.profile.wear_glasses = false;
            //cuf.profile.age_confidence = 1;
            //cuf.profile.gender_confidence = 1;
            //cuf.profile.expression_confidence = 1;

            cuf.enter_time = GameReady.Instance.picturesList[i].enterTime;  //必须
            cuf.leave_time = GameReady.Instance.picturesList[i].exitTime; //必须
            cuf.position = FacePosition.FacePositionPositive;
            if (isPostPicture)
            {
                //Texture2D tex2d = SaveCamPicture(WebCam.Instance.GetWebCamTexture());LastFaceResult.OriginTexture.Texture
                Texture2D tex2d = CutFacePicture(WebCam.Instance.GetWebCamTexture(), GameReady.Instance.picturesList[i]);
                //Texture2D tex2d = CutFacePicture(FaceController.Instance.LastFaceResult.OriginTexture.Texture, GameReady.Instance.picturesList[i]);
                cuf.face_image = tex2d.EncodeToJPG();
                cuf.image_width = tex2d.width;
                cuf.image_height = tex2d.height;
            }
            //cuf.image_mime_type = t;
            //cuf.land_marks = new List<int>();
            //for (int j = 0; j < 5; j++)
            //{
            //    cuf.land_marks.Add(j);
            //}

            cuf.event_type = FaceEventType.NO_EVENT;
            
            userFaceInfo.face.Add(cuf);
        }
        MemoryStream ms = new MemoryStream();
        Serializer.Serialize(ms, userFaceInfo);
        byte[] b = ms.ToArray();
        return b;
    }

    public IEnumerator CutScreeByRect(Rect mRect, string mFileName)
    {

        yield return new WaitForEndOfFrame();
        //初始化Texture2D  
        Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据  
        
        mTexture.ReadPixels(mRect, 0, 0);
        mTexture.Apply();
        img.texture = mTexture;
        ////将图片信息编码为字节信息  
        //byte[] bytes = mTexture.EncodeToPNG();
        ////保存  
        //System.IO.File.WriteAllBytes(mFileName, bytes);
    }

    //// 根据一个Rect类型来截取指定范围的屏幕  
    //private IEnumerator CaptureByRect(Rect mRect, string mFileName)
    //｛
    //    //等待渲染线程结束  
    //    yield return new WaitForEndOfFrame();
    ////初始化Texture2D  
    //Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
    ////读取屏幕像素信息并存储为纹理数据  
    //mTexture.ReadPixels(mRect, 0, 0);
    //    mTexture.Apply();
    //    //将图片信息编码为字节信息  
    //    byte[] bytes = mTexture.EncodeToPNG();
    ////保存  
    //System.IO.File.WriteAllBytes(mFileName, bytes);
    // ｝

    //保存相机图片
    public Texture2D SaveCamPicture(WebCamTexture tex)
    {
        //创建一个texture2D 获取相机图片
        Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, true);
        texture2D.SetPixels32(tex.GetPixels32());
        texture2D.Apply();
        //byte[] imageByte = texture2D.EncodeToJPG();
        return texture2D;

        //if(imageByte != null && imageByte.Length > 0)
        //{
        //    //判断Android平台 设置路径
        //    string savePath;
        //    string platformPath = Application.streamingAssetsPath + "/tyd";

        //    if (!Directory.Exists(platformPath))
        //    {
        //        Directory.CreateDirectory(platformPath);
        //    }

        //    //保存图片
        //    savePath = platformPath + "/" + Time.deltaTime + ".jpg";
        //    File.WriteAllBytes(savePath, imageByte);
        //}
    }

    //string base64解码
    public static byte[] UnBase64String(string value)
    {
        if (value == null || value == "")
        {
            return null;
        }
        byte[] bytes = Convert.FromBase64String(value);
        //return Encoding.UTF8.GetString(bytes);
        return bytes;
    }

    //string base64编码
    public static string ToBase64String(string value)
    {
        if (value == null || value == "")
        {
            return "";
        }
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    //进行md5加密
    public string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }


    public RawImage r_origin;
    public RawImage r_cut;
    //剪切人脸图片
    public Texture2D CutFacePicture(WebCamTexture tex, OneFacePictureObject faceObj)
    {
        ////测试
        //Texture2D t = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, true);
        //t.SetPixels32(originPixel);
        //t.Apply();
        //r_origin.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
        //r_origin.texture = SaveCamPicture(tex);
        Color[] originPixel = tex.GetPixels();

        int curRight_x = (int)faceObj.face_up_pos.y;
        int curLeft_x = (int)faceObj.face_down_pos.y;
        int curUp_y = (int)faceObj.face_right_pos.x;
        int curDown_y = (int)faceObj.face_lelf_pos.x;
        

        int cutWidth = curRight_x - curLeft_x;
        cutWidth = cutWidth > 0 ? cutWidth : -cutWidth;
        int offsetLeftX = curLeft_x - cutWidth / 2;
        curLeft_x = offsetLeftX < 0 ? 0 : offsetLeftX;
        cutWidth = curRight_x - curLeft_x;
        cutWidth = cutWidth * 2;

        
        int max_j = curLeft_x + cutWidth > tex.width ? tex.width : curLeft_x + cutWidth; //最大j索引
        cutWidth = cutWidth + curLeft_x > tex.width ? tex.width - curLeft_x : cutWidth; //真实宽

        int cutHeight = curUp_y - curDown_y;
        cutHeight = cutHeight > 0 ? cutHeight : -cutHeight;
        int offsetLeftY = curLeft_x - cutHeight / 2;
        curDown_y = offsetLeftY < 0 ? 0 : offsetLeftY;
        cutHeight = curUp_y - curDown_y;
        cutHeight = cutHeight * 2;

        
        int max_i = curDown_y + cutHeight > tex.height ? tex.height : curDown_y + cutHeight; //最大i索引
        cutHeight = cutHeight + curDown_y > tex.height ? tex.height - curDown_y : cutHeight;  //真实高
        Texture2D destTex = new Texture2D(cutWidth, cutHeight, TextureFormat.RGB24, true);
        Color[] destPix = new Color[cutWidth * cutHeight];

        //txt.text = "剪切宽：" + cutWidth + "  剪切高：" + cutHeight + "\n原图宽：" + tex.width + "\n原图高：" + tex.height + "\n当前上Y：" + curUp_y
        //    + "\n当前下Y:" + curDown_y + "\n当前左X:" + curLeft_x + "\n当前右X:" + curRight_x + "\n max_i:" + max_i + "\n max_j" + max_j + "\n原来上Y：" + (int)faceObj.face_up_pos.y
        //    + "\n原来下Y:" + (int)faceObj.face_down_pos.y + "\n原来左X:" + (int)faceObj.face_lelf_pos.x + "\n原来右X:" + (int)faceObj.face_right_pos.x;
        int tagetIndex = -1;
        for (int i = curDown_y; i < max_i; i++)
        {
            for (int j = curLeft_x; j< max_j; j++)
            {
                tagetIndex++;
                destPix[tagetIndex] = originPixel[i * tex.width + j];
            }
        }
        destTex.SetPixels(destPix);
        destTex.Apply();
        //r_cut.rectTransform.sizeDelta = new Vector2(cutWidth, cutHeight);
        //r_cut.texture = destTex;
        return destTex;
    }

    /// <summary>
    /// 剪切图片
    /// </summary>
    /// <param name="tex">原图</param>
    /// <param name="width">图片宽</param>
    /// <param name="height">图片高</param>
    /// <param name="originStartPosX">x最小点</param>
    /// <param name="originStartPosY">y最小点</param>
    /// <returns></returns>
    public Texture2D CutFacePicture(Texture2D tex, int width, int height, int originStartPosX, int originStartPosY)
    {
        Color32[] originPixel = tex.GetPixels32();

        //测试
        Texture2D t = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, true);
        t.SetPixels32(originPixel);
        t.Apply();
        //r_origin.texture = t;
        //print("原图像素个数：" + originPixel.Length); 
        //print("原图宽：" + tex.width);
        //print("原图高：" + tex.height);
        int realHight = originStartPosY + height > tex.height ? tex.height - originStartPosY : height; //真实高度
        int realWidth = originStartPosX + width > tex.width ? tex.width - originStartPosX : width; //真实宽度
        Texture2D destTex = new Texture2D(realWidth, realHight, TextureFormat.RGB24, true);
        Color32[] destPix = new Color32[realWidth * realHight];
        int tagetY = -1;
        int max_i = originStartPosY + height > tex.height? tex.height: originStartPosY + height; //最大i索引
        int max_j = originStartPosX + width > tex.width ? tex.width : originStartPosX + width; //最大j索引
        for (int i = originStartPosY; i < max_i; i++)
        {
            for (int j = originStartPosX; j < max_j; j++)
            {
                tagetY++;
                destPix[tagetY] = originPixel[i * tex.width + j];
            }
        }
        //print("剪切后像素个数：" + destPix.Length);
        destTex.SetPixels32(destPix);
        destTex.Apply();
        //r_cut.rectTransform.sizeDelta = new Vector2(realWidth, realHight);
        //r_cut.texture = destTex;
        return destTex;
    }
}
