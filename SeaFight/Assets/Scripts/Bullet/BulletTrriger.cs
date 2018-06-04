using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrriger : MonoBehaviour
{
    public BulletMove myMove;

    public Vector3 addRotation;
    public Vector3 RotateSpeed;
    private Vector3 saveRotation;

    void OnTriggerEnter(Collider coll)
    {
        myMove.TrrigerLogic(coll);
    }


    private void Update()
    {
        Transform cam = null;
        if (GameManager.instance != null)
        {
            cam = GameManager.instance.gameCamera.transform;
        }
        if (cam != null)
        {
            if (RotateSpeed != Vector3.zero)//自身旋转
            {
                transform.Rotate(RotateSpeed * Time.deltaTime);
            }
            else
            {
                Quaternion qt = Quaternion.LookRotation(cam.forward, cam.up);
                transform.rotation = qt;
            }
        }
    }





}
