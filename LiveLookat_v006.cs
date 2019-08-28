using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
☆☆☆ CGSS模型专用视线追踪脚本 【适配kobayakawa Sae 0132，0132_1011, 0132_1311 (HQ)】☆☆☆
☆☆☆ 计算 Eye 与 Camera LookAt Handler 之间的 Vector3、Quaternion 差，得出相应x、w补偿值 ☆☆☆
*需要将 Eye 设为 Eye Locator 的子骨骼，调整 Eye Locator Rotation
*/

public class LiveLookat : MonoBehaviour
{
    [Header("相机替身")]
    public GameObject LookAtHandler;
    //↓官模自带双眼定位GameObject↓可通过脚本同步左右眼骨骼↓（已过时，现在直接使用Eye骨骼沿EyeLocator移动）
    [Header("左眼定位（过时）")]
    public GameObject eyeLocatorL;
    [Header("左眼")]
    public GameObject eyeLeft;
    [Header("右眼定位（过时）")]
    public GameObject eyeLocatorR;
    [Header("右眼")]
    public GameObject eyeRight;

    [Header("平滑速度")]
    public float speed = 8.75f; // Lerp speed;

    // Var link（未实装）
    [Header("Var未实装")]
    public float originDiff = 1.5; // 原位置偏移（计算用）
    public float eRelModDiv = 175; //  总体灵敏度分母（计算用）
    public float angleDiffWeight = 0.015; // 角度补偿灵敏度（计算用）
    public float ReyeMaxOut = 0.00625f; // 右眼外侧限制
    public float ReyeMaxIn = -0.00785f; //右眼内侧限制
    public float LeyeMaxOut = -0.00625f; // 左眼外侧限制
    public float LeyeMaxIn = 0.00785f; //左眼内侧限制

    public void calcCamPos()
    {
        //==========Var==========//
        Vector3 camPos = LookAtHandler.transform.localPosition; // Camera LookAt Handler Position | 相机 世界to本地位置

        /*
        Vector3 eLOrigin = eyeLocatorL.transform.position; // Left Eye World Position | 左眼定位 世界位置
        Vector3 eROrigin = eyeLocatorR.transform.position; // Right Eye World Position |右眼定位 世界位置
        Vector3 eLLocalOrigin = eyeLocatorL.transform.localPosition; // Left Eye Local Position | 左眼定位 本地位置
        Vector3 eRLocalOrigin = eyeLocatorR.transform.localPosition; // Right Eye Local Position | 右眼定位 本地位置
        */

        Vector3 eyeLOrigin = eyeLeft.transform.position; // Left Eye World Position | 左眼 世界位置
        Vector3 eyeROrigin = eyeRight.transform.position; // Right Eye World Position |右眼 世界位置

        Vector3 eyeLLocalOrigin = eyeLeft.transform.localPosition; // Left Eye Local Position | 左眼 本地位置
        Vector3 eyeRLocalOrigin = eyeRight.transform.localPosition; // Right Eye Local Position | 右眼 本地位置

        Quaternion camRotation = LookAtHandler.transform.rotation; // Camera LookAt Handler Rotation | 相机旋转


        //==========Func==========//

        /* RIGHT EYE */
        // Calculate Rotation difference between Right Eye and LookAt Handler | 计算右眼与相机之间的角度差
        Quaternion ReyeRotation = eyeLocatorR.transform.rotation; // 旧版
        Quaternion RighteyeRotation = eyeRight.transform.rotation;
        Quaternion RighteyeRotationDiff = new Quaternion(RighteyeRotation.x, RighteyeRotation.y, RighteyeRotation.z, RighteyeRotation.w - camRotation.w); // 虽然写了一长串但真正会用到的只是第四个w值_(:з」∠)_

        Vector3 eRRelative = new Vector3( camPos.x - eyeRLocalOrigin.x - originDiff * (1.000f + angleDiffWeight + RighteyeRotationDiff.w), eyeRLocalOrigin.x - camPos.x, eyeRLocalOrigin.x - camPos.x); // Local Position difference between Right Eye and Camera LookAt Handler | 计算右眼与相机之间的x轴差
        Vector3 eRTarget = eRRelative / eRelModDiv; //+ new Vector3(0.04776929f, 0.07901000f, 0.08240000f); // Calculate new target position | 右眼原位置 + x轴差距

        eRTarget.x = Mathf.Clamp(eRTarget.x, ReyeMaxIn, ReyeMaxOut); // Clamp new target position | 限制移动范围 [内,外]
        Vector3 desR = new Vector3(eRTarget.x, eyeRight.transform.localPosition.y, eyeRight.transform.localPosition.z);

        eyeRight.transform.localPosition = desR; // Set new target position | 应用新位置
        eyeRight.transform.localPosition = Vector3.Lerp(eyeRight.transform.localPosition, desR, Time.deltaTime * speed);

        /* LEFT EYE */
        // Calculate Rotation difference between Left Eye and LookAt Handler | 计算左眼与相机之间的角度差
        Quaternion LeyeRotation = eyeLocatorL.transform.rotation; // 旧版
        Quaternion LefteyeRotation = eyeLeft.transform.rotation;
        Quaternion LefteyeRotationDiff = new Quaternion(LefteyeRotation.x, LefteyeRotation.y, LefteyeRotation.z, LefteyeRotation.w - camRotation.w);

        Vector3 eLRelative = new Vector3( camPos.x - eyeLLocalOrigin.x - originDiff * (1.000f - angleDiffWeight + LefteyeRotationDiff.w), eyeLLocalOrigin.x - camPos.x, eyeLLocalOrigin.x - camPos.x); // Local Position difference between Left eye and Camera LookAt Handler | 计算左眼与相机之间的x轴差
        Vector3 eLTarget = eLRelative / eRelModDiv; //+ new Vector3(-0.04776929f, 0.07901000f, 0.082400000f); // Calculate new target position | 左眼原位置 + x轴差距

        eLTarget.x = Mathf.Clamp(eLTarget.x, LeyeMaxOut, LeyeMaxIn); // Clamp new target position | 限制移动范围 [外,内]
        Vector3 desL = new Vector3(eLTarget.x, eyeLeft.transform.localPosition.y, eyeLeft.transform.localPosition.z);

        eyeLeft.transform.localPosition = desL; // Set new target position | 应用新位置
        eyeLeft.transform.localPosition = Vector3.Lerp(eyeLeft.transform.localPosition, desL, Time.deltaTime * speed);


        //==========Test==========//

        //Debug.Log("Right Eye X Modification: " + (eRTarget.x));
        Debug.Log("Left Eye X Modification: " + (eLTarget.x));
        //Debug.Log("Right eye Rotation Diff: "+ReyeRotationDiff);
        //Debug.Log("Left eye Rotation Diff: "+LeyeRotationDiff);

        //以下内容已经没用了

     /*
         if (eLRelative.x > 0)
         {
            eLTarget.x = Mathf.Clamp(eLTarget.x, -0.0530f, -0.0470f);
             eRTarget.x = Mathf.Clamp(eRTarget.x, 0.0420f, 0.0480f);
         }
             if(eLRelative.x < 0)
         {
            eLTarget.x = Mathf.Clamp(eLTarget.x, -0.0480f, -0.0420f);
            eRTarget.x = Mathf.Clamp(eRTarget.x, 0.0470f, 0.0530f);
         }
     */
    }

    //==========Apply==========//
    void Start ()
    {
      //calcCamPos();
      InvokeRepeating("CustomUpdate",0.0f,3.0f)
    }
	  void Update ()
    {
      //calcCamPos();
    }

    private void FixedUpdate()
    {
      calcCamPos();
    }

    public void CustomUpdate(){
      calcCampos();
    }

    /*
      void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    string myLog;
    Queue myLogQueue = new Queue();
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }
    void OnGUI()
    {
        GUILayout.Label(myLog);
    }
    */
}
