using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camtexture : MonoBehaviour {

	public LeapUVCv2 _leapUVC;
	public int leftright = 0; //0:left, 1:right
	private Texture2D cam_Texture;
	
	void Start () {
		cam_Texture = new Texture2D(
			_leapUVC.WIDTH, 	//カメラのwidth
			_leapUVC.HEIGHT, 	//カメラのheight
			TextureFormat.YUY2,	//フォーマットを指定
			false				//ミニマップ設定
		);
		this.GetComponent<Renderer>().material.mainTexture = cam_Texture;
	}
	
	// Update is called once per frame
	void Update () {
		cam_Texture.LoadImage(_leapUVC.LeftRightFrame[leftright].ImEncode());
	}
}
