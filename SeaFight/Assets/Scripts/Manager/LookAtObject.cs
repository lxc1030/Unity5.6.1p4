using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform cam;   // cube的朝向目标
    public Vector3 rot;
    void Update()
    {
        Quaternion qt = Quaternion.LookRotation(cam.forward, cam.up);//cam.up 是参考轴
        Quaternion ro = Quaternion.Euler(rot);
        qt *= ro;
        transform.rotation = qt;
    }
    
}
