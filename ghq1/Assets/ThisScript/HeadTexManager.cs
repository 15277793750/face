using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadTexManager : MonoBehaviour
{
    public GameObject[] headTex;
    public bool IsHasFace = true ; //标记没有切换人

    private int headIndex = 0;
    private GameObject curHeadTex;

    public Text testTxt;

    void Start()
    {
        
    }
    
    void Update()
    {
        //如果没人
        if (FaceView.Instance.SelectedFace == null)
        {
            testTxt.text = "没人";
            FaceView.Instance.TrackingObject = null;
            IsHasFace = true;
        }
        else
        {
            testTxt.text = "有人";
            if (IsHasFace)
            {
                headIndex = Random.Range(0, 3);
                curHeadTex = Instantiate(headTex[headIndex]);
                IsHasFace = false;
            }
            //人脸鼻子坐标转换成世界坐标
            Vector3 face_pos = FaceView.Instance.TransformLandmarkPointToWorld(82) ?? Vector2.zero;
            curHeadTex.transform.position = new Vector3(face_pos.x, face_pos.y, 85);
            curHeadTex.transform.rotation = Quaternion.Euler(new Vector3(FaceView.Instance.SelectedFace.Value.Pitch * 25, FaceView.Instance.SelectedFace.Value.Yaw * 90, FaceView.Instance.SelectedFace.Value.Roll * 45));
        }
    }
}
