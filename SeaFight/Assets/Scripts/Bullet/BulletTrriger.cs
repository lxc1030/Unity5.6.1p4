using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrriger : MonoBehaviour
{
    public BulletMove myMove;

    public Vector3 Rotation;
    public Vector3 RotateSpeed;
    Transform cam = null;


    public void Init()
    {
        if (GameManager.instance != null)
        {
            cam = GameManager.instance.gameCamera.transform;
        }
        if (cam != null)//正交相机只需要朝向一次
        {
            Quaternion qt = Quaternion.LookRotation(cam.forward, cam.up);
            //Quaternion ro = Quaternion.Euler(Rotation);
            //qt *= ro;
            transform.rotation = qt;
            transform.Rotate(Rotation, Space.Self);
        }
    }


    void OnTriggerEnter(Collider coll)
    {
        myMove.TrrigerLogic(coll);
    }


    private void Update()
    {
        if (RotateSpeed != Vector3.zero)//自身旋转
        {
            transform.Rotate(RotateSpeed * Time.deltaTime);
        }
    }





}
