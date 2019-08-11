using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Yaksha.Net;

public class WebCam : MySingleton<WebCam>
{
	public const int RequestWidth = 1080;
	public const int RequestHeight = 1920;

	public int CameraDeviceIndex;
	private WebCamTexture _camTexture;

    // TODO: 设置这个字段以覆盖摄像头旋转角度设定
    //private int? _overrideRotationAngle;
    //private int RotationAngle => _overrideRotationAngle ?? _camTexture.videoRotationAngle;
    public int _overrideRotationAngle;
    public int RotationAngle;
    public int camRotate;
    public Text txt;

    //private bool? _overrideMirrored;
	// 前置摄像头的画面是翻转的
	//private bool Mirrored => _overrideMirrored ??
	//						 _camTexture.videoVerticallyMirrored ^ WebCamTexture.devices[CameraDeviceIndex].isFrontFacing;
    private bool Mirrored = true;

    private RawImage _rawImage;
	private RectTransform _rectTransform;

    //测试
    public void ChangeRotate()
    {
        RotationAngle += 90;
        if(RotationAngle >= 360 || RotationAngle <= 0)
        {
            RotationAngle = 0;
        }
    }

	private WebCamTexture ConfigureCamTexture(string deviceName, int width, int height)
	{
		var texture = new WebCamTexture(deviceName, width, height);
		_rawImage?.Let(rawImage =>
		{
			rawImage.texture = texture;
			rawImage.material.mainTexture = texture;
		});

		texture.Play();
		return texture;
	}

	private IEnumerator Start()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
		if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			yield break;
		}

		var devices = WebCamTexture.devices;
		if (devices.Length == 0)
		{
			yield break;
		}

		_rawImage = GetComponent<RawImage>();
		_rectTransform = GetComponent<RectTransform>();

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
		var intentFilter = new AndroidJavaObject("android.content.IntentFilter",
			new AndroidJavaClass("android.content.Intent").GetStatic<AndroidJavaObject>("ACTION_BATTERY_CHANGED"));
		var batteryStatus = Utility.ApplicationContext.Value.Call<AndroidJavaObject>("registerReceiver", null, intentFilter);
		Contract.Assert(batteryStatus != null);
		var batteryManager = new AndroidJavaClass("android.os.BatteryManager");
		var chargePlug = batteryStatus.Call<int>("getIntExtra",
			batteryManager.GetStatic<AndroidJavaObject>("EXTRA_PLUGGED"), -1);
		var isAd = chargePlug == batteryManager.GetStatic<int>("BATTERY_PLUGGED_AC") &&
				   SystemInfo.batteryStatus == BatteryStatus.Discharging;
		if (isAd)
		{
            txt.text = "进来了";
			//_overrideRotationAngle = 0;
			//_overrideMirrored = true;
			//CameraDeviceIndex = 0;
		}
#endif

        _camTexture = ConfigureCamTexture(devices[CameraDeviceIndex].name, RequestWidth, RequestHeight);
        //txt.text = _camTexture.videoRotationAngle.ToString();
        FaceController.Instance.SetupFaceTexture(new FaceModel.FaceTexture(_camTexture, camRotate, Mirrored));
	}

	private void Update()
	{
		if (!_camTexture)
		{
			return;
		}

		var rotationAngle = RotationAngle;
		var isLandscape = rotationAngle % 180 == 0;
		_rectTransform?.Let(rectTransform =>
		{
			rectTransform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.back);
			rectTransform.sizeDelta = Utility.ScaleToFit(new Vector2(_camTexture.width, _camTexture.height),
				isLandscape ? new Vector2(Screen.width, Screen.height) : new Vector2(Screen.height, Screen.width));
		});

		_rawImage?.Let(rawImage =>
		{
			var mirrored = Mirrored;
			// 参见 https://docs.unity3d.com/Manual/SL-PropertiesInPrograms.html
			rawImage.material.SetVector("_MainTex_ST", !mirrored ? new Vector4(1, 1, 0, 0) : isLandscape ? new Vector4(-1, 1, 1, 0) : new Vector4(1, -1, 0, 1));
		});
	}

    public WebCamTexture GetWebCamTexture()
    {
        return _camTexture;
    }
}
