using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtHandler : MonoBehaviour {

    public Camera mainCam;
    public GameObject LookAt;

    public void LookAtHandlerSync()
    {
        // 同步
        LookAt.transform.rotation = mainCam.transform.rotation;
        LookAt.transform.position = mainCam.transform.position;

    }
	void Start ()
    {
        LookAtHandlerSync();
    }	
	void Update ()
    {
        LookAtHandlerSync();
    }
}
