using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //FaceMesage msgJson = new FaceMesage();
        //msgJson.id = 1;
        //msgJson.timestamp = 1;
        //msgJson.mallId = "商场编号1";
        //msgJson.terminalId = "点位编号1";
        //msgJson.face.enterTime = 2;
        //msgJson.face.leaveTime = 3;
        //msgJson.face.profile.faceCode = 4;
        //string jsonDataPost = JsonMapper.ToJson(msgJson);
        //print(jsonDataPost);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//public class FaceMesage
//{
//    public int id;
//    public int timestamp;  //时间戳精确到毫秒(必须)
//    public string mallId; //商场编号(必须)
//    public string terminalId; //点位编号(必须)
//    public Face face = new Face(); //人脸信息
//    public string terminalMac; //终端屏mac地址（必须）
//}
//public class Face
//{
//    public Profile profile = new Profile();
//    public int enterTime; //进入时间精确到毫秒(必须)
//    public int leaveTime; //进入时间精确到毫秒(必须)
//    public string position; //可选值为 FacePositionPositive 正脸;  FacePositionSide 侧脸;
//    //public byte[] faceImage; //jpg格式的字节
//    public string imageMimeType;
//    //public float[] landMarks = new float[5]; // (x << 16) + y ，即每个值是横坐标数值左移16位后加上纵坐标数值，并按照“左眼”、“右眼中间”、“鼻尖”、“左嘴边”、“右嘴边”(必须)
//    public string eventType; //可选值为NO_EVENT;  ENTER_SIGHT;  DETECT_INFO; COME_CLOSE; LEAVE_SIGHT;
//    public double imageWidth;
//    public double imageHeight;
//}

//public class Profile
//{
//    public string beginAge;
//    public string endAge;
//    public string gender;//可选值为GENDER_UNKNOWN;  GENDER_MALE;  GENDER_FEMALE;
//    public int faceCode;//屏前人流的编号id(进入屏后id保持一致， 离开后再进入会变化)
//    public string expression;//"UserExpressionUnknown",  # 可选值为UserExpressionUnknown；Happy； Afraifd； Surprised；Sad；Angry；Serene；Depressed；UserExpressionOthers
//    public bool wearGlasses;
//    public double ageConfidence;
//    public double genderConfidence;
//    public double expressionConfidence;
//}