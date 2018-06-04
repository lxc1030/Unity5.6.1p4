using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool isOnce;
    // Use this for initialization
    void OnEnable()
    {
        if (isOnce)
        {
            LookLogic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnce)
        {
            LookLogic();
        }
    }

    void LookLogic()
    {
        Transform cam = null;
        if (GameManager.instance != null)
        {
            cam = GameManager.instance.gameCamera.transform;
        }
        if (cam != null)
        {
            Quaternion qt = Quaternion.LookRotation(cam.forward, cam.up);
            transform.rotation = qt;
        }
    }



}
