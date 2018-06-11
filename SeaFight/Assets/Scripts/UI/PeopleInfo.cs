using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PeopleInfo : MonoBehaviour
{
    public Text txName;
    public Image[] imgHps;
    public Image imgHp;

    private Transform UI;
    //默认血条缩与摄像机的距离
    private float Fomat;
    //角色头顶的点， 最好让美术把这个点直接做在fbx模型里面。
    public Transform Head;
    private Camera Camera;

    public void Init(Camera camera, Transform target, string name, string spName)
    {
        Camera = camera;
        txName.text = name;
        imgHp.fillAmount = 1f;

        Head = target;
        UI = this.transform;
        Common.ImageChange(imgHp, spName);

        SetPosition();
    }
    public void SetHp(float precent)
    {
        imgHp.DOFillAmount(precent, 0.1f);
    }

    // Use this for initialization
    void Start()
    {
        //计算一下默认血条的距离，也可以写个常量，就是标记一下
        //Fomat = Vector3.Distance(Head.position, GameManager.instance.gameCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Head != null)
        {
            SetPosition();
        }
    }

    private void SetPosition()
    {
        //这里可以判断一下 如果位置没有变化就不要在赋值了
        UI.position = WorldToUI(Head.position, Camera, Camera.main);
        //计算出血条的缩放比例 
        //UI.localScale = Vector3.one * newFomat;
        UI.localScale = Vector3.one;
    }


    //核心代码在这里把3D点换算成NGUI屏幕上的2D点。
    public static Vector3 WorldToUI(Vector3 point, Camera camera3, Camera camera2)
    {
        Vector3 pt = camera3.WorldToScreenPoint(point);
        //我发现有时候UICamera.currentCamera 有时候currentCamera会取错，取的时候注意一下啊。
        Vector3 ff = camera2.ScreenToWorldPoint(pt);
        //UI的话Z轴 等于0 
        ff.z = 0;
        return ff;
    }

}
