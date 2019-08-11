using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Yaksha.Net;
using System.Reflection;

public static class Utility
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Reverse(this Vector2 vec)
	{
		return new Vector2(vec.y, vec.x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Abs(this Vector2 vec)
	{
		return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Abs(this Vector3 vec)
	{
		return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Abs(this Vector4 vec)
	{
		return new Vector4(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z), Mathf.Abs(vec.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 ToVector(this Yaksha.Point point)
	{
		return new Vector2(point.X, point.Y);
	}

	public static Yaksha.Net.Yaksha.TPFrameRotateType GetRotateType(int value)
	{
		if (value % 90 != 0)
		{
			throw new ArgumentOutOfRangeException(nameof(value), value, "Not a valid angle");
		}

		Contract.EndContractBlock();

		return (Yaksha.Net.Yaksha.TPFrameRotateType) (value % 360);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Sigmoid(float x)
	{
		return 2 / (1 + Mathf.Pow(1.2f, -38 * x)) - 1;
	}

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
	public static readonly Lazy<AndroidJavaObject> CurrentActivity = new Lazy<AndroidJavaObject>(() =>
	{
		var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	});

	public static readonly Lazy<AndroidJavaObject> ApplicationContext =
		new Lazy<AndroidJavaObject>(() => CurrentActivity.Value.Call<AndroidJavaObject>("getApplicationContext"));
#endif

	public static void ToastMessage(string message, bool longLength = true)
	{
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
		CurrentActivity.Value.Call("runOnUiThread", new AndroidJavaRunnable(() =>
		{
			var toast = new AndroidJavaClass("android.widget.Toast");
			var str = new AndroidJavaObject("java.lang.String", message);
			toast.CallStatic<AndroidJavaObject>("makeText", ApplicationContext.Value, str,
				toast.GetStatic<int>(longLength ? "LENGTH_LONG" : "LENGTH_SHORT")).Call("show");
		}));
#endif
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 ToRelative(Vector2 point, Vector2 src)
	{
		return new Vector2(point.x / src.x, point.y / src.y);
	}

	// 保持长宽比地占满 dest
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 ScaleToFit(Vector2 src, Vector2 dest)
	{
		var k = dest.y / dest.x < src.y / src.x ? dest.x / src.x : dest.y / src.y;
		return src * k;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 ScaleToFit(Vector2 point, Vector2 src, Vector2 dest)
	{
		var k = dest.y / dest.x < src.y / src.x ? dest.x / src.x : dest.y / src.y;
		return point * k;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rect ScaleToFit(Rect rect, Vector2 src, Vector2 dest)
	{
		var k = dest.y / dest.x < src.y / src.x ? dest.x / src.x : dest.y / src.y;
		return new Rect(rect.x * k, rect.y * k, rect.width * k, rect.height * k);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 CalculateOffset(Vector2 src, Vector2 dest)
	{
		return CalculateOffset(src, dest, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 CalculateOffset(Vector2 src, Vector2 dest, Vector2 anchor, Vector2 pivot)
	{
		return dest * anchor - src * pivot;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rect HFlipRect(Rect rect, Vector2 src)
	{
		return new Rect(src.x - rect.xMax, rect.y, rect.width, rect.height);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rect VFlipRect(Rect rect, Vector2 src)
	{
		return new Rect(rect.x, src.y - rect.yMax, rect.width, rect.height);
	}

	public static Rect RotateRect(Rect rect, Vector2 src, Yaksha.Net.Yaksha.TPFrameRotateType rotateType)
	{
		switch (rotateType)
		{
		case Yaksha.Net.Yaksha.TPFrameRotateType.TPFrameRotate0:
			return rect;
		case Yaksha.Net.Yaksha.TPFrameRotateType.TPFrameRotate90:
			return new Rect(src.y - rect.yMax, rect.x, rect.height, rect.width);
		case Yaksha.Net.Yaksha.TPFrameRotateType.TPFrameRotate180:
			return new Rect(src.x - rect.xMax, src.y - rect.yMax, rect.width, rect.height);
		case Yaksha.Net.Yaksha.TPFrameRotateType.TPFrameRotate270:
			return new Rect(rect.y, src.x - rect.xMax, rect.height, rect.width);
		default:
			throw new ArgumentOutOfRangeException(nameof(rotateType), rotateType, "Unknown rotateType");
		}
	}

	public static float CalculateVariance(ICollection<Vector2> points)
	{
		if (points == null)
		{
			throw new ArgumentNullException(nameof(points));
		}

		var count = points.Count;
		if (count == 0)
		{
			throw new ArgumentException("points is empty", nameof(points));
		}

		Contract.EndContractBlock();

		var center = points.AverageByOrDefault(UtilityExtension.GenericOperators<Vector2>.Add.Value,
			UtilityExtension.GenericOperators<Vector2>.Others<int>.Divide.Value);
		return points.Select(p => (p - center).sqrMagnitude).Average();
	}

	public static T[] ExtractArray<T>(this Google.Protobuf.Collections.RepeatedField<T> repeatedField)
	{
		if (repeatedField == null)
		{
			throw new ArgumentNullException(nameof(repeatedField));
		}

		Contract.EndContractBlock();

		var field = typeof(Google.Protobuf.Collections.RepeatedField<T>).GetField("array", BindingFlags.Instance | BindingFlags.NonPublic);
		Contract.Assert(field != null);
		return (T[]) field.GetValue(repeatedField);
	}
}
