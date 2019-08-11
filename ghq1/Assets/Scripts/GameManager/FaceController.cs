using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaksha;

public class FaceController : MySingleton<FaceController>
{
	public FaceModel FaceModel { get; } = new FaceModel();
    public FaceModel.Face LastFaceResult;

    public void SetupFaceTexture(FaceModel.FaceTexture faceTexture)
	{
		FaceModel.SetupFaceTexture(faceTexture);
	}



	private void LateUpdate()
	{
	    var face = FaceModel.UpdateFace();
	    if (face.FaceResult != null)
	    {
            LastFaceResult = face;

            //FaceView.Instance.OnFaceResultAvailable(faceResult);
		}
	}
}
