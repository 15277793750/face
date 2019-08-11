//#define TEST_MAX_MIN_LANDMARK
//#define TEST_MAX_MIN_BROW

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yaksha.Net;

public class FaceView : MySingleton<FaceView>
{
	public Shader MaskShader;
	public Canvas Canvas;
	public SpriteRenderer TrackingObject;
	public Camera MainCamera;
	public Vector2 FaceObjectOffset;
	public float FaceObjectScaleFactor;
	public float FaceRectScaleFactor;
	public float VisibleRadius;
	[Range(0, 2)]
	public float WhiteFactor;

	public FaceModel.Face LastFaceResult { get; private set; }
	private bool _faceResultUpdated;

	public struct FaceObjectPair
	{
		public Yaksha.Face Face { get; }
		public GameObject FaceObject { get; }

		// [-1, 1]
		public float Pitch => Mathf.Clamp(Utility.Sigmoid(Face.Landmark.Pitch / 25), -1, 1);  //抬头正  低头负  最大角度25
		public float Roll => Mathf.Clamp(Utility.Sigmoid(Face.Landmark.Roll / 45), -1, 1);  //左片头正  右片头负  最大角度45
		public float Yaw => Mathf.Clamp(Utility.Sigmoid(Face.Landmark.Yaw / 90), -1, 1);   //左摇头负  右摇头正  最大角度90

		// [0, 1]
		public float LeftBrowHeight => Mathf.Clamp01((Vector2.Distance(Face.Landmark.Points[29].ToVector(), Face.Landmark.Points[78].ToVector()) / (Face.Rect.Bottom - Face.Rect.Top) - 0.1f) / 0.08f);
		public float RightBrowHeight => Mathf.Clamp01((Vector2.Distance(Face.Landmark.Points[45].ToVector(), Face.Landmark.Points[80].ToVector()) / (Face.Rect.Bottom - Face.Rect.Top) - 0.1f) / 0.08f);

		public float MouthOpeningFactor => Utility.CalculateVariance(
			new ArraySegment<Yaksha.Point>(Face.Landmark.Points.ExtractArray(), 63, 14).Select(p => p.ToVector())
				.ToArray());

		public FaceObjectPair(Yaksha.Face face, GameObject faceObject)
		{
			Face = face;
			FaceObject = faceObject;
		}
	}

	private FaceObjectPair? _selectedFace;
	public FaceObjectPair? SelectedFace
	{
		get { return _selectedFace; }
		private set
		{
			_selectedFace?.Let(o =>
			{
				if (o.FaceObject != value?.FaceObject)
				{
					Destroy(o.FaceObject);
				}
			});

			_selectedFace = value;
		}
	}

	public delegate void OnNewFaceSelectedDelegate(FaceObjectPair face);
	public event OnNewFaceSelectedDelegate OnNewFaceSelectedEvent;

	public void OnFaceResultAvailable(FaceModel.Face faceResult)
	{
		LastFaceResult = faceResult;
		_faceResultUpdated = true;
	}

	public Vector2? TransformLandmarkPointToWorld(int index)
	{
		return SelectedFace?.Let(f =>
		{
			var point = f.Face.Landmark.Points[index].ToVector();
			var screenSize = new Vector2(MainCamera.pixelWidth, MainCamera.pixelHeight);
			var rotatedTextureSize = LastFaceResult.OriginTexture.RotatedTextureSize;
			var scaledSize = Utility.ScaleToFit(rotatedTextureSize, screenSize);
			point = Utility.ScaleToFit(point, rotatedTextureSize, screenSize);
			var offset = (screenSize - scaledSize) / 2;
			point += offset;
			// 屏幕坐标原点在左下角，Landmark 原点在左上角
			point.y = screenSize.y - point.y;
			return MainCamera.ScreenToWorldPoint(point);
		});
	}

	private void Start()
	{
		if (!MainCamera)
		{
			MainCamera = Camera.main;
		}
	}

#if TEST_MAX_MIN_LANDMARK
	private float _maxRoll = float.MinValue, _minRoll = float.MaxValue;
	private float _maxYaw = float.MinValue, _minYaw = float.MaxValue;
	private float _maxPitch = float.MinValue, _minPitch = float.MaxValue;
#endif

#if TEST_MAX_MIN_BROW
	private float _maxLeftBrowHeight = float.MinValue, _minLeftBrowHeight = float.MaxValue;
	private float _maxRightBrowHeight = float.MinValue, _minRightBrowHeight = float.MaxValue;
#endif

//	private void LateUpdate()
//	{
//		if (!_faceResultUpdated)
//		{
//			return;
//		}

//		// 未检测到任何脸，取消选中并直接返回
//		if (LastFaceResult.FaceResult.Array.Count == 0)
//		{
//			SelectedFace = null;
//			return;
//		}

//		var isLandscape = LastFaceResult.OriginTexture.IsLandscape;

//		var width = LastFaceResult.OriginTexture.Texture.width;
//		var height = LastFaceResult.OriginTexture.Texture.height;

//		// 优先使用之前已选中的脸，使用其 FaceId 再次选中
//		if (SelectedFace != null)
//		{
//			var id = SelectedFace.Value.Face.Rect.FaceId;
//			// 若未找到（跟丢等情况）则将其置为 null，在其后进行选择
//			SelectedFace = LastFaceResult.FaceResult.Array.FirstOrDefault(f => f.Rect.FaceId == id)?.Let(f =>
//				new FaceObjectPair(f, SelectedFace.Value.FaceObject));
//		}

//		// 若未选中过脸或者跟丢，则选择当前最大面积的脸
//		if (SelectedFace == null)
//		{
//			var maxAreaFace =
//				LastFaceResult.FaceResult.Array.MaxBy(f => (f.Rect.Right - f.Rect.Left) * (f.Rect.Bottom - f.Rect.Top));
//			// 已经检查过数组长度，不会出现 null 的情况
//			Contract.Assert(maxAreaFace != null);

//			var newFaceObject = new GameObject();
//			var rectTransform = newFaceObject.AddComponent<RectTransform>();
//			newFaceObject.transform.SetParent(Canvas.transform, false);
//			rectTransform.localScale = Vector3.one;
//			rectTransform.anchorMin = new Vector2(0f, 0f);
//			rectTransform.anchorMax = new Vector2(0f, 0f);
//			rectTransform.pivot = new Vector2(0.5f, 0.5f);
//			rectTransform.localRotation = Quaternion.AngleAxis(LastFaceResult.OriginTexture.RotationAngle, Vector3.back);
//			var rawImage = newFaceObject.AddComponent<RawImage>();
//			var material = new Material(MaskShader)
//			{
//				mainTexture = LastFaceResult.OriginTexture.Texture
//			};
//			rawImage.material = material;

//			SelectedFace = new FaceObjectPair(maxAreaFace.Value.Item1, newFaceObject);
//			OnNewFaceSelectedEvent?.Invoke(SelectedFace.Value);
//		}

//		var face = SelectedFace.Value.Face;

//#if TEST_MAX_MIN_LANDMARK
//		print($"Roll: {_maxRoll = Mathf.Max(_maxRoll, SelectedFace.Value.Roll)}, {_minRoll = Mathf.Min(_minRoll, SelectedFace.Value.Roll)}, " +
//		      $"Yaw: {_maxYaw = Mathf.Max(_maxYaw, SelectedFace.Value.Yaw)}, {_minYaw = Mathf.Min(_minYaw, SelectedFace.Value.Yaw)}, " +
//		      $"Pitch: {_maxPitch = Mathf.Max(_maxPitch, SelectedFace.Value.Pitch)}, {_minPitch = Mathf.Min(_minPitch, SelectedFace.Value.Pitch)}");
//#endif

//#if TEST_MAX_MIN_BROW
//		print($"Left brow max height: {_maxLeftBrowHeight = Mathf.Max(_maxLeftBrowHeight, SelectedFace.Value.LeftBrowHeight)}, " +
//		      $"min height: {_minLeftBrowHeight = Mathf.Min(_minLeftBrowHeight, SelectedFace.Value.LeftBrowHeight)}, " +
//		      $"current height: {SelectedFace.Value.LeftBrowHeight}, " +
//		      $"Right brow max height: {_maxRightBrowHeight = Mathf.Max(_maxRightBrowHeight, SelectedFace.Value.RightBrowHeight)}, " +
//		      $"min height: {_minRightBrowHeight = Mathf.Min(_minRightBrowHeight, SelectedFace.Value.RightBrowHeight)}, " +
//		      $"current height: {SelectedFace.Value.RightBrowHeight}");
//#endif

//		var faceObject = SelectedFace.Value.FaceObject;
//		var srcSize = LastFaceResult.OriginTexture.RotatedTextureSize;
//		var originFaceRect = new Rect(face.Rect.Left, face.Rect.Top, face.Rect.Right - face.Rect.Left,
//			face.Rect.Bottom - face.Rect.Top);
//		// 已经旋转过
//		var rawFaceRect = Utility.RotateRect(originFaceRect, srcSize,
//			Utility.GetRotateType(360 - LastFaceResult.OriginTexture.RotationAngle));

//		if (!TrackingObject)
//		{
//			faceObject.SetActive(false);
//		}
//		else
//		{
//			faceObject.SetActive(true);

//			faceObject.GetComponent<RawImage>().Let(r =>
//			{
//				var size = new Vector2(rawFaceRect.width / width, rawFaceRect.height / height);
//				var position = new Vector2(rawFaceRect.xMin / width, rawFaceRect.yMin / height);
//				var scaledSize = size * FaceRectScaleFactor;
//				var scaledPosition = position + (size - scaledSize) / 2;
//				if (LastFaceResult.OriginTexture.Mirrored)
//				{
//					if (isLandscape)
//					{
//						scaledPosition.x += scaledSize.x;
//						scaledSize.x = -scaledSize.x;
//					}
//					else
//					{
//						scaledPosition.y += scaledSize.y;
//						scaledSize.y = -scaledSize.y;
//					}
//				}
//				r.material.SetVector("_MainTex_ST", new Vector4(scaledSize.x, scaledSize.y, scaledPosition.x, scaledPosition.y));
//				r.material.SetFloat("_Radius", VisibleRadius);
//				r.material.SetFloat("_WhiteFactor", WhiteFactor);
//			});

//			faceObject.GetComponent<RectTransform>().Let(t =>
//			{
//				t.SetAsLastSibling();
//				var transformedPosition = MainCamera.WorldToScreenPoint(TrackingObject.gameObject.transform.position + TrackingObject.gameObject.transform.TransformVector(FaceObjectOffset));
//				var worldSpaceSize = TrackingObject.transform.TransformVector(TrackingObject.sprite.rect.size / TrackingObject.sprite.pixelsPerUnit);
//				// 或许只在正交投影下有效
//				var screenSpaceSize = (MainCamera.WorldToScreenPoint(worldSpaceSize) - MainCamera.WorldToScreenPoint(Vector3.zero)).Abs();
//				t.anchoredPosition = transformedPosition;
//				t.sizeDelta = screenSpaceSize * FaceObjectScaleFactor;
//				t.localPosition += Vector3.back;
//			});
//		}

//		_faceResultUpdated = false;
//	}
}
