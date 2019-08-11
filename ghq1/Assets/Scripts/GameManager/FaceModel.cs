#if !UNITY_EDITOR_WIN
#define YAKSHA_ENABLED
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using UnityEngine;
using Yaksha;
using Yaksha.Net;
using Rect = UnityEngine.Rect;

public class FaceModel
{
	public struct FaceTexture
	{
		public WebCamTexture Texture { get; }
		public int RotationAngle { get; }
		public bool Mirrored { get; }
		public bool IsLandscape => RotationAngle % 180 == 0;
		public Vector2 TextureSize => new Vector2(Texture.width, Texture.height);
		public Vector2 RotatedTextureSize => IsLandscape ? TextureSize : TextureSize.Reverse();

		public FaceTexture(WebCamTexture texture, int rotationAngle, bool mirrored)
		{
			Texture = texture;
			RotationAngle = rotationAngle;
			Mirrored = mirrored;
		}
	}

	public struct Face
	{
		public FaceTexture OriginTexture { get; }
	    public FaceResult FaceResult { get; }

	    public Face(FaceTexture originTexture, FaceResult faceResult)
	    {
	        OriginTexture = originTexture;
	        FaceResult = faceResult;
	    }
	}

	public FaceTexture UsingFaceTexture { get; private set; }

	private class FileLoader : Yaksha.Net.Yaksha.IFileLoader
	{
		private readonly string _pathPrefix;

		public FileLoader(string pathPrefix)
		{
			_pathPrefix = pathPrefix ?? string.Empty;
		}

		public ReadOnlySpan<byte> LoadFileFromPath(string path, int enginePlatform)
		{
			Contract.Assert(!string.IsNullOrEmpty(path));

			var fileContent = Resources.Load<TextAsset>(_pathPrefix + path);
			if (!fileContent)
			{
				throw new ArgumentException($"Cannot load file from path \"{path}\"", nameof(path));
			}

			return fileContent.bytes;
		}
	}

	static FaceModel()
	{
#if YAKSHA_ENABLED
		Yaksha.Net.Yaksha.RegisterFileLoader(new FileLoader("Models/"));
#endif
	}

	public void SetupFaceTexture(FaceTexture faceTexture)
	{
		UsingFaceTexture = faceTexture;
	}

	public Face UpdateFace()
	{
#if YAKSHA_ENABLED
		var texture = UsingFaceTexture.Texture;
		var pixels = texture.GetPixels32();
		var width = texture.width;
		var height = texture.height;

		ReadOnlySpan<byte> rawPixels;

		if (BitConverter.IsLittleEndian)
		{
			// 注意：包含对 Color32 的内存布局的假设
			rawPixels = MemoryMarshal.AsBytes(new ReadOnlySpan<Color32>(pixels));
		}
		else
		{
			var rawPixelArray = new byte[width * height * 4];

			for (var i = 0; i < pixels.Length; ++i)
			{
				var pixel = pixels[i];
				rawPixelArray[i * 4 + 0] = pixel.a;
				rawPixelArray[i * 4 + 1] = pixel.b;
				rawPixelArray[i * 4 + 2] = pixel.g;
				rawPixelArray[i * 4 + 3] = pixel.r;
			}

			rawPixels = rawPixelArray;
		}

		var faceLandmarkResult = Yaksha.Net.Yaksha.GetFaceLandmarkResult(rawPixels, width, height,
			Utility.GetRotateType(UsingFaceTexture.RotationAngle), !UsingFaceTexture.Mirrored,
			Yaksha.Net.Yaksha.TPFrameType.FrameType_ABGR);

	    return new Face(UsingFaceTexture, faceLandmarkResult);
#else
		return new Face(UsingFaceTexture, null);
#endif
	}
}
