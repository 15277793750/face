using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yaksha.Net;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// 准备游戏状态
/// </summary>
public class GameReady : MySingleton<GameReady>
{
	private float timer = 0; //进入游戏计时器
	private float time = 4;//捕捉时间
    private float erWeiMatimer = 0; //进入游戏计时器
    private float erWeiMatime = 10;//扫码时间

    private float back_timer = 0; //回到无人页计时器
	private float back_time = 2;

	private bool IsCreate = false; //标志只创建一次
	private bool IsReady = false; //是否准备好了
    
    public Camera MainCamera;
    //火箭  预制体
    public GameObject head1_prefab;  //鼻眼镜
    //兔子   预制体
    public GameObject head2_mouth_prefab;  //鼻子部分
    public GameObject head2_ear_prefab;  //耳朵部分
    //眼镜  预制体
    public GameObject head3_glass_prefab;  //鼻眼镜

    //实例
    private int MaxFaceNum = 10; //最大人脸个人数
    private GameObject[] head1;
    private GameObject[] head2_mouth;
    private GameObject[] head2_ear;
    private float earDistance = 3.1f; //耳朵上偏移的距离
    private GameObject[] head3_glass;
    private GameObject[] headRoot;//贴纸的根节点
    public Vector3[] face_pos; //鼻子转化在屏幕上的坐标
    private int[] FaceId;  //贴纸索引
    private int[] headIndex;  //贴纸索引
    public long[] enterTime;//人脸进入时间
    public long[] exitTime;//人脸退出时间

    public GameObject pointPrefab;//点预制体
    private GameObject[] point = new GameObject[83];  //脸部所有检测点
    public RawImage faceImage;  //脸部图片
    public GameObject titleBg;//主题背景
    public GameObject camBound;//相框
    public GameObject erWeiMa;//二维码
    private bool IserWeiMa = false;//是否是显示二维码状态
    private bool IsCatch = false;//是否是捕捉人脸状态
    private bool IsEnterNone = true; //第一次进入无人状态

    public PostProcessDebug post_debug;
    public PostProcessLayer post_layer;
    public PostProcessVolume post_volume;

    private float requireBgTimer = 0; //请求海报的计时器
    private float requireBgTime = 600; //请求海报的时间

    private float testNumX = 1f;  //测试增加数据
    private float testNumY = 0.2f;  //测试增加数据
    private float testNumZ = 0.2f;
    //private float testBeaty = 1f; //美颜程度
    private float testHead1Scale = 1.15f;
    private float testHead2Scale = 1f;
    private float testHead3Scale = 1f;
    private float testearPos = 3.9f; //耳朵位移距离
    private bool testBool = true;
    private int num = 0;

    //private OneFacePictureObject testFace = new OneFacePictureObject();

    public List<OneFacePictureObject> picturesList = new List<OneFacePictureObject>(); //所有贴纸

    public Text txt_test; //测试文本1
    public Text txt_test2; //测试文本2

    public void TestAddNumX()
    {
        testNumX = testNumX + 0.1f;
    }
    public void TestReduceNumX()
    {
        testNumX = testNumX - 0.1f;
    }
    public void TestAddNumY()
    {
        testearPos = testearPos + 0.1f;
    }
    public void TestReduceNumY()
    {
        time = time - 1f;
    }
    public void TestAddNumZ()
    {
        erWeiMatime = erWeiMatime + 0.1f;
    }
    public void TestReduceNumZ()
    {
        erWeiMatime = erWeiMatime - 0.1f;
    }
    public void TestBeaty()
    {
        //testBeaty = testBeaty + 0.1f;
        //faceImage.material.SetFloat("_Radius", testBeaty);
    }
    public void TestHead1Scale()
    {
        testHead1Scale = testHead1Scale + 0.1f;
    }
    public void TestHead2Scale()
    {
        testHead2Scale = testHead2Scale + 0.1f;
    }
    public void TestHead3Scale()
    {
        testHead3Scale = testHead3Scale + 0.1f;
    }
    public void TestEarPos()
    {
        testearPos = testearPos + 0.1f;
    }
    public void TestHideBg()
    {
        titleBg.SetActive(false);
    }
    public void TestQuit()
    {
        Application.Quit();
    }

    private void Start()
    {
        //testFace = CreateFacePicture(1);
        //faceImage.material.SetFloat("_Radius", testBeaty);
        UnityPageRequest.Instance.RequiretBgLoad();
        //for (int i = 0; i < 83; i++)
        //{
        //    GameObject a = Instantiate(pointPrefab);
        //    point[i] = a;
        //}

        //for (int i = 0; i < point.Length; i++)
        //{
        //    if (i % 5 == 0)
        //    {
        //        point[i].GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.red);
        //    }
        //    else if (i % 5 == 1)
        //    {
        //        point[i].GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.white);
        //    }
        //    else if (i % 5 == 2)
        //    {
        //        point[i].GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.yellow);
        //    }
        //    else if (i % 5 == 3)
        //    {
        //        point[i].GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.green);
        //    }
        //    else if (i % 5 == 4)
        //    {
        //        point[i].GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.blue);
        //    }
        //    point[i].transform.position = new Vector3(i, 0, 0);
        //}

    }

    private void LateUpdate()
	{
        requireBgTimer += Time.deltaTime;
        if (requireBgTimer >= requireBgTime)
        {
            requireBgTimer = 0;
            UnityPageRequest.Instance.RequiretBgLoad();
        }
        
        //没人
        if (FaceController.Instance.LastFaceResult.FaceResult.Array.Count == 0 && !IserWeiMa)
        {
            if (IsEnterNone)
            {
                IsEnterNone = false;
                back_timer = 0;
                IsCreate = false;
                IsCatch = false;
                IserWeiMa = false;
            }

            if (picturesList.Count > 0)
            {
                //请求人脸数据上报（离开状态）
                foreach (var item in picturesList)
                {
                    item.exitTime = MyTimeTool.GetTimeStamp(false); //记录离开时间
                }
                StartCoroutine(UnityPageRequest.Instance.SendHttpMessage(false));

                foreach (var item in picturesList)
                {
                    if (item.headRoot)
                    {
                        Destroy(item.headRoot);
                        item.headRoot = null;
                    }
                    picturesList.Remove(item);
                }
            }
            //txt_test.text = "没人";
            back_timer += Time.deltaTime;
            //IsCreate = false;
            //IsCatch = true;

            if (back_timer > back_time)
            {
                //进入无人状态
                back_timer = 0;
                titleBg.SetActive(true);
                erWeiMa.SetActive(false);
                post_debug.enabled = false;
                post_layer.enabled = false;
                post_volume.enabled = false;
                camBound.SetActive(false);
            }
        }
        if (FaceController.Instance.LastFaceResult.FaceResult.Array.Count >0 )
        {
            //Vector2 v_l = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[0].ToVector(); //左
            //Vector2 v_r = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[18].ToVector(); //右
            //Vector2 v_u = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[29].ToVector(); //上
            //Vector2 v_d = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[9].ToVector(); //下
            //txt_test.text = "上：" + v_u.x + "  " + v_u.y + "\n下：" + v_d.x + "  " + v_d.y + "\n左：" + v_l.x + "  " + v_l.y + "\n右：" + v_r.x + "  " + v_r.y;


            if (!IsCreate)  //开始进入捕捉
            {
                camBound.SetActive(true);
                IsEnterNone = true;
                IsCreate = true;
                IsCatch = true;
                post_debug.enabled = true;
                post_layer.enabled = true;
                post_volume.enabled = true;
                timer = 0;
                erWeiMatimer = 0;
                titleBg.SetActive(false);
                erWeiMa.SetActive(false);
            }

            bool ishaveId = false; //是否存在不存在的贴纸id
            //检测不存在的贴纸id，并销毁
            foreach (var item in picturesList)
            {
                ishaveId = false;
                for (int i = 0; i < FaceController.Instance.LastFaceResult.FaceResult.Array.Count; i++)
                {
                    if (item.faceId == FaceController.Instance.LastFaceResult.FaceResult.Array[i].Rect.FaceId)
                    {
                        ishaveId = true;
                        break;
                    }
                }
                if (ishaveId == false)
                {
                    if (item.headRoot)
                    {
                        Destroy(item.headRoot);
                        item.headRoot = null;
                    }
                    picturesList.Remove(item);
                }
            }

            OneFacePictureObject curFace = null; //贴纸引用
            //先创建贴纸
            bool isHavaPicture = false; //是否已有贴纸
            for (int i = 0; i < FaceController.Instance.LastFaceResult.FaceResult.Array.Count; i++)
            {
                if (picturesList.Count > 0)
                {
                    isHavaPicture = false;
                    foreach (var item in picturesList)//检测是否已有贴纸
                    {
                        if (item.faceId == FaceController.Instance.LastFaceResult.FaceResult.Array[i].Rect.FaceId)
                        {
                            isHavaPicture = true;
                            break;
                        }
                    }
                    if (isHavaPicture == false)
                    {
                        //Vector2 v_l = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[0].ToVector(); //左
                        //Vector2 v_r = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[18].ToVector(); //右
                        //Vector2 v_u = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[29].ToVector(); //上
                        //Vector2 v_d = FaceController.Instance.LastFaceResult.FaceResult.Array[0].Landmark.Points[9].ToVector(); //下
                        OneFacePictureObject f1 = CreateFacePicture(FaceController.Instance.LastFaceResult.FaceResult.Array[i].Rect.FaceId);
                        f1.face_up_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 29));
                        f1.face_down_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 9));
                        f1.face_lelf_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 0));
                        f1.face_right_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 18));
                        picturesList.Add(f1);
                    }
                }
                else
                {
                    //创建贴纸
                    OneFacePictureObject f2 = CreateFacePicture(FaceController.Instance.LastFaceResult.FaceResult.Array[i].Rect.FaceId);
                    f2.face_up_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 29));
                    f2.face_down_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 9));
                    f2.face_lelf_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 0));
                    f2.face_right_pos = Camera.main.WorldToScreenPoint(TransformLandmarkPointToWorld(i, 18));
                    picturesList.Add(f2);
                }

                //找到对应id的贴纸
                foreach (var item in picturesList)
                {
                    if (item.faceId == FaceController.Instance.LastFaceResult.FaceResult.Array[i].Rect.FaceId)
                    {
                        curFace = item;
                        break;
                    }
                }
                if (curFace == null) return;


                //转化人脸鼻子坐标
                Vector3 face_pos = TransformLandmarkPointToWorld(i, 82);
                //int i_x = (int)(face_pos.x * 100);
                //float f_x = i_x;
                //int i_y = (int)(face_pos.y * 100);
                //float f_y = i_y;
                //face_pos.x = f_x / 100;
                //face_pos.y = f_y / 100;
                //face_pos.z = 0;

                //计算头旋转值
                float x = Mathf.Clamp(Utility.Sigmoid(FaceController.Instance.LastFaceResult.FaceResult.Array[i].Landmark.Pitch / 25), -1, 1);
                float y = Mathf.Clamp(Utility.Sigmoid(FaceController.Instance.LastFaceResult.FaceResult.Array[i].Landmark.Yaw / 90), -1, 1);
                float z = Mathf.Clamp(Utility.Sigmoid(FaceController.Instance.LastFaceResult.FaceResult.Array[i].Landmark.Roll / 45), -1, 1);

                //txt_test.text = "远近缩放：" + testNumX + "\n捕捉时间：" + time + "\n扫码时间：" + erWeiMatime + "\n美颜程度：" + testBeaty
                //       + "\n火箭大小：" + testHead1Scale + "\n兔子大小：" + testHead2Scale + "\n眼镜大小：" + testHead3Scale + "\n耳朵偏移：" + earDistance;
                float scale = (Vector2.Distance(TransformLandmarkPointToWorld(i, 82), TransformLandmarkPointToWorld(i, 55))) / 0.02f * 0.04f;
                curFace.headRoot.transform.localPosition = face_pos;
                curFace.headRoot.transform.localRotation = Quaternion.Euler(new Vector3(x * 45, y * 45, z * 20));
                if ((x <= testNumX && x >= -testNumX) && (y <= testNumX && y >= -testNumX) && (z <= testNumX && z >= -testNumX))
                {
                    curFace.headRoot.transform.localScale = new Vector3(2 * scale, 2 * scale, 1);
                }
            }

            //捕捉状态计时
            if (IsCatch && !IserWeiMa)
            {
                timer += Time.deltaTime;
                if (timer > time)  //进入二维码界面
                {
                    //post_debug.enabled = false;
                    //post_layer.enabled = false;
                    //post_volume.enabled = false;
                    timer = 0;
                    erWeiMa.SetActive(true);
                    IserWeiMa = true;
                    IsCatch = false;

                    //请求人脸数据上报（离开状态）
                    foreach (var item in picturesList)
                    {
                        item.exitTime = MyTimeTool.GetTimeStamp(false); //记录离开时间
                    }
                    StartCoroutine(UnityPageRequest.Instance.SendHttpMessage(true));

                }
            }

 
            //txt_test.text = "捕捉时间："+ timer + "\n扫码时间：" + erWeiMatimer;
        }

        //二维码界面计时
        if (!IsCatch && IserWeiMa)
        {
            erWeiMatimer += Time.deltaTime;
            if (erWeiMatimer >= erWeiMatime)
            {
                erWeiMatimer = 0;
                IsCreate = false;
                IserWeiMa = false;
                IsCatch = true;
                //titleBg.SetActive(true);
                erWeiMa.SetActive(false);
            }
        }
    }

    //脸坐标转世界坐标
    public Vector2 TransformLandmarkPointToWorld(int faceIndex,int facePointIndex)
    {
        var point = FaceController.Instance.LastFaceResult.FaceResult.Array[faceIndex].Landmark.Points[facePointIndex].ToVector();
        var screenSize = new Vector2(MainCamera.pixelWidth, MainCamera.pixelHeight);
        var rotatedTextureSize = FaceController.Instance.LastFaceResult.OriginTexture.RotatedTextureSize;
        var scaledSize = Utility.ScaleToFit(rotatedTextureSize, screenSize);
        point = Utility.ScaleToFit(point, rotatedTextureSize, screenSize);
        var offset = (screenSize - scaledSize) / 2;
        point += offset;
        // 屏幕坐标原点在左下角，Landmark 原点在左上角
        point.y = screenSize.y - point.y;
        return MainCamera.ScreenToWorldPoint(point);
    }

    //创建人脸贴纸对象
    public OneFacePictureObject CreateFacePicture(int faceId)
    {
        //创建贴纸
        OneFacePictureObject pic = new OneFacePictureObject();
        pic.enterTime = MyTimeTool.GetTimeStamp(false);
        pic.facePictureIndex = UnityEngine.Random.Range(0, 3);
        pic.faceId = faceId;
        if (pic.facePictureIndex == 0)
        {
            //星星火箭

            GameObject head = Instantiate(head1_prefab);
            head.transform.parent = pic.headRoot.transform;
            head.transform.localPosition = new Vector3(0.3f, 1.5f, 0);
            head.transform.localScale = new Vector3(2 * testHead1Scale, 2 * testHead1Scale, 1);
        }
        else if (pic.facePictureIndex == 1)
        {
            //兔子
            GameObject ear = Instantiate(head2_ear_prefab);
            ear.transform.parent = pic.headRoot.transform;

            GameObject mouth = Instantiate(head2_mouth_prefab);
            mouth.transform.parent = pic.headRoot.transform;

            ear.transform.localPosition = new Vector3(0.1f, testearPos, 0);
            ear.transform.localScale = new Vector3(2 * testHead2Scale, 2 * testHead2Scale, 1);
            mouth.transform.localPosition = new Vector3(0, 0, 0);
            mouth.transform.localScale = new Vector3(2 * testHead2Scale, 2 * testHead2Scale, 1);
        }
        else if (pic.facePictureIndex == 2)
        {
            //眼镜
            GameObject glass = Instantiate(head3_glass_prefab);
            glass.transform.parent = pic.headRoot.transform;
            glass.transform.localScale = new Vector3(2 * testHead3Scale, 2 * testHead3Scale, 2 * testHead3Scale);
            glass.transform.localPosition = new Vector3(0, 1, 0);
            glass.transform.localScale = new Vector3(2 * testHead3Scale, 2 * testHead3Scale, 2 * testHead3Scale);
        }
        return pic;
    }
}

//贴纸对象
public class OneFacePictureObject
{
    public int faceId;
    public GameObject headRoot = new GameObject(); //整个贴纸
    public long enterTime = MyTimeTool.GetTimeStamp(false);
    public long exitTime;
    public int facePictureIndex;
    public Vector2 face_up_pos; //脸最上边的像素坐标（序号29）
    public Vector2 face_down_pos; //脸最下边的像素坐标（序号9）
    public Vector2 face_lelf_pos; //脸最左边的像素坐标（序号0）
    public Vector2 face_right_pos; //脸最右边的像素坐标（序号18）
}
