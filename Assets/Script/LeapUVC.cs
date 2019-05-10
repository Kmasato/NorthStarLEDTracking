using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp; //OpenCVSharpを追加

public class LeapUVC : MonoBehaviour {

	public int WIDTH = 640;		//カメラの横サイズ
	public int HEIGHT = 480;	//カメラの縦サイズ
	private static int source = 1;

	private VideoCapture cam;
	private Mat cam_frame;				//Leapmotionから受け取った生データ
	public Mat[] LeftRightFrame; 		//左右の映像を格納する行列

	void Start () {
		cam = new VideoCapture(source);
		cam.Set(CaptureProperty.ConvertRgb,0);
		cam_frame = new Mat();
		LeftRightFrame = new Mat[2];
	}
	
	void Update () {

		//データを取得 1x614400
		cam.Read(cam_frame);

		//LとRの307200(640x640)x2に分ける
		cam_frame = cam_frame.Reshape(1, HEIGHT * WIDTH, 2);

		//1列目は左データ　307200 x 1
		Mat leftFrame = cam_frame.ColRange(0, 1);
		//2列目は右データ　307200 x 1
		Mat rightFrame = cam_frame.ColRange(1, 2);

		//行列を転置させる 1x307200
		leftFrame = leftFrame.T();
        rightFrame = rightFrame.T();

		//1x307200 を　480x640に変換
		leftFrame = leftFrame.Reshape(1, HEIGHT, WIDTH);
        rightFrame = rightFrame.Reshape(1, HEIGHT, WIDTH);

		//1つの配列にまとめる
		LeftRightFrame = new Mat[2] {leftFrame, rightFrame};
	}
}
