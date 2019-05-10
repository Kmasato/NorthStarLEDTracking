using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp; //OpenCVSharpを追加

public class LeapUVCv2 : MonoBehaviour {

	public int WIDTH = 640;		//カメラの横サイズ
	public int HEIGHT = 480;	//カメラの縦サイズ
	private static int source = 1;

	private VideoCapture cam;
	private Mat cam_frame;				//Leapmotionから受け取った生データ
	public Mat[] LeftRightFrame; 		//左右の映像を格納する行列

	void Start () {
		cam = new VideoCapture(source);
		cam.Set(CaptureProperty.ConvertRgb,0);
		//Debug.Log(cam.Set(CaptureProperty.Fps,1));
		cam_frame = new Mat();
		LeftRightFrame = new Mat[2];
		setCenterLED(false);
		setLeftLED(false);
		setRightLED(false);
	}
	
	void Update () {

		//データを取得 1x614400
		cam.Read(cam_frame);
		Debug.Log(cam_frame.Size());
		//LとRの307200(640x640)x2に分ける
		/*
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
		*/
	}

	public void get(CaptureProperty param){
		this.cam.Get(param);
	}
	public void set(CaptureProperty param, double value){
		this.cam.Set(param, value);
	}
	//Sets the sensor's exposure in mircroseconds (up to 65535)
	public void setExposure(int exposureUS){
		if(exposureUS < 10)
			exposureUS = 10;
		this.cam.Set(CaptureProperty.Zoom,exposureUS);
	}
	//Sets whether the image will be in a non-linear color space approximating sqrt(x)
	public void setGammaEnabled(bool enabled){
		int value = 0;
		if(enabled) value = 1;
		else value = 0;
		this.cam.Set(CaptureProperty.Gamma,value);
	}

	public void setLEDsHDRorRotate(int selector, int value){
		this.set(CaptureProperty.Contrast, ((selector) | ((value) << 6)));
	}
	public void setHDR(bool enabled){
		int value = 0;
		if(enabled) value = 1;
		else value = 0;
		this.setLEDsHDRorRotate(0,value);
	}
	public void setLeftLED(bool enabled){
		int value = 0;
		if(enabled) value = 1;
		else value = 0;
		this.setLEDsHDRorRotate(2, value);
	}
	public void setCenterLED(bool enabled){
		int value = 0;
		if(enabled) value = 1;
		else value = 0;
		this.setLEDsHDRorRotate(2, value);
	}
	public void setRightLED(bool enabled){
		int value = 0;
		if(enabled) value = 1;
		else value = 0;
		this.setLEDsHDRorRotate(4, value);
	}
	
	//Changes the digital Gain, between 0 and 16
	public void setDigitalGain(int value){
		this.set(CaptureProperty.Brightness,value);
	}
	//Specifies the analog gain as a scalar, between 16 and 63
	public void setGain(int gain){
		this.set(CaptureProperty.Gain, gain);
	}

	public void setGainFPSRatioOrFrameInterval(int selector, int value){
		this.cam.Set(CaptureProperty.Gain, ((selector) | ((value) & 0x3fff)));
	}
	public void setFpsRatio(int ratio){
		setGainFPSRatioOrFrameInterval(0x8000, ratio);
	}
}
