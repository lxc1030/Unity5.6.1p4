using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraExchange : MonoBehaviour
{
    public CameraExchangeType myType;

    public Transform UI;
    public Transform Head;
    public Camera camera2D;
    public Camera camera3D;
    

    public void Init()
    {

    }

    void Update()
    {
        switch (myType)
        {
            case CameraExchangeType.World_UI:
                if (Head != null)
                {
                    SetPosition();
                }
                break;
            case CameraExchangeType.UI_World:
                if (UI != null)
                {
                    Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(camera2D, UI.transform.position);
                    Head.transform.position = UIToWorld(camera3D, uiPos, 10);
                }
                break;
        }



    }

    #region 3D转2D

    private void SetPosition()
    {
        //这里可以判断一下 如果位置没有变化就不要在赋值了
        UI.position = WorldToUI(Head.position, camera3D, camera2D);
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

    #endregion


    #region 2D转3D
    // 通过2d的坐标加上z轴的深度，获得该2d坐标在3d上的位置
    public static Vector3 UIToWorld(Camera camera, Vector2 vec2, float z)
    {
        Vector3 world = new Vector3(vec2.x / Screen.width, vec2.y / Screen.height, z);
        Vector3 world1 = camera.ViewportToWorldPoint(new Vector3(world.x, world.y, world.z)); // 屏幕坐标转换成场景坐标
        return world1;
    }

    #endregion

}

public enum CameraExchangeType
{
    World_UI,
    UI_World,
}