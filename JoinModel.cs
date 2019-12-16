using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinModel : MonoBehaviour {
    public GameObject headHead; //头部模型的Head骨骼
    public GameObject bodyHead; //身体模型的Head骨骼
    public GameObject headNeck; //头部模型的Neck骨骼
    public GameObject bodyNeck; //身体模型的Neck骨骼
    public GameObject headPosition;//这个没用

    public void JoinHeadAndNeck () //(ÒωÓױ)yareyare nanodesu☆
    {
        //Head位置同步
        headHead.transform.rotation = bodyHead.transform.rotation;
        headHead.transform.position = bodyHead.transform.position;

        //Neck位置同步
        headNeck.transform.rotation = bodyNeck.transform.rotation;
        headNeck.transform.position = bodyNeck.transform.position;
    }
    public void Start ()
    {
        JoinHeadAndNeck(); //初始化同步☆
    }
    public void Update()
    {
        JoinHeadAndNeck(); //维持同步☆
    }
}
