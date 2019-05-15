using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class LedTracking : MonoBehaviour
{
    public LeapUVCv2 leapUVC;

    /* Shi-Tomasiのコーナー検出パラメータ */
    private int maxCorners = 100, minDistance = 7, blockSize = 7;
    private float qualityLevel = 0.3f;

    /* Lucas-Kanade法のパラメータ */
    Size winSize = new Size(15,15);
    int maxLevel = 3;
    TermCriteria criteria = new TermCriteria(CriteriaType.Eps | CriteriaType.Count, 10, 0.03);

    private Color[] colors;

    private Mat[] grayPrev = new Mat[2], grayNext = new Mat[2];
    private Mat[] colorFrame = new Mat[2];
    private Mat[] mask = new Mat[2];
    private Point2f[] feature_prev_l, feature_prev_r;

    private bool track_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        Mat zerosGray = Mat.Zeros(leapUVC.WIDTH, leapUVC.HEIGHT,MatType.CV_8UC1);
        Mat zerosColor = Mat.Zeros(leapUVC.WIDTH, leapUVC.HEIGHT, MatType.CV_64FC3);
        grayPrev = new Mat[2]{zerosGray, zerosGray};
        colorFrame = new Mat[2]{zerosColor, zerosColor};
        mask = new Mat[2]{zerosColor, zerosColor};
    }

    // Update is called once per frame
    void Update()
    {
        if(track_flag == false)
            setTrackingPoint();
        if(track_flag == true)
            opticalFlow();
        //for(int i=0; i<feature_prev_l.Length; i++)
          //  Debug.Log(feature_prev_l[i]);
    }

    void setTrackingPoint(){
        while(true){
            for(int i=0; i < 2; i++){
                grayPrev[i] = leapUVC.LeftRightFrame[i].Clone();
                if(i == 0)
                    feature_prev_l = Cv2.GoodFeaturesToTrack(grayPrev[0], maxCorners, qualityLevel, minDistance, null, blockSize, false, 0.0f);
                else
                    feature_prev_r = Cv2.GoodFeaturesToTrack(grayPrev[0], maxCorners, qualityLevel, minDistance, null, blockSize, false, 0.0f);
                Cv2.CvtColor(grayPrev[i], colorFrame[i], ColorConversionCodes.GRAY2BGR);
                mask[i] = new Mat(colorFrame[i].Width, colorFrame[i].Height, MatType.CV_64FC3);
            }
            if(feature_prev_l.Length == feature_prev_r.Length)
                break;
        }
        track_flag = true;
    }

    void opticalFlow(){

        InputOutputArray feature_next = new Mat();
        OutputArray status = new Mat();
        OutputArray err = new Mat();
        Point2f good_prev;
        Point2f good_next;

        for(int i=0; i<2; i++){
            //歪み補正を追加?
            grayNext[i] = leapUVC.LeftRightFrame[i].Clone();
            Cv2.CvtColor(grayNext[i], colorFrame[i], ColorConversionCodes.GRAY2BGR);
            //左カメラ
            if(i == 0){
                //オプティカルフロー検出
                Mat feature_prev = new Mat(1, feature_prev_l.Length, MatType.CV_32FC2, feature_prev_l);
                feature_prev = feature_prev.T();
                //Debug.Log(feature_prev.Size());
                Cv2.CalcOpticalFlowPyrLK(grayPrev[i], grayNext[i], feature_prev, feature_next, status, err, winSize, maxLevel, criteria);
                //Mat st = status.GetMat();
            }
            else{
                Mat feature_prev = new Mat(1, feature_prev_r.Length, MatType.CV_32FC2, feature_prev_r);
                feature_prev = feature_prev.T();
                Cv2.CalcOpticalFlowPyrLK(grayPrev[i], grayNext[i], feature_prev, feature_next, status, err, winSize, maxLevel, criteria);
            }
            
            Mat feature_nextMat = feature_next.GetMat();
            for(int j = 0; j < feature_nextMat.Height; j++){
                Debug.Log(j);
            }
        }
    }
}
