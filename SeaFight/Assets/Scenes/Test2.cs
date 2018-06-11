using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一般式为y=ax²+bx+c (a,b,c为常数,a≠0) 
/// </summary>
public class Test2 : MonoBehaviour
{
    public GameObject moveObj;
    public GameObject orgObj;
    public GameObject targetObj;



    private Vector3 orgPos;
    private Vector3 targetPos;
    public float height;
    public float xSpeed;

    public bool isInit;
    private float a;
    private float b;
    private float c;

    [ContextMenu("发射")]
    void Init()
    {
        orgPos = orgObj.transform.position;
        targetPos = targetObj.transform.position;

        Vector2 middle = new Vector2((orgPos.x + targetPos.x) / 2, (orgObj.transform.position.y + targetObj.transform.position.y) / 2 + height);

        a = (targetPos.y - middle.y) * orgPos.x - (targetPos.x - middle.x) * orgPos.y + targetPos.x * middle.y - middle.x * targetPos.y;
        a = -a;
        a /= (targetPos.x - middle.x) * (orgPos.x - targetPos.x) * (orgPos.x - middle.x);

        b = (targetPos.y - middle.y) * Mathf.Pow(orgPos.x, 2) + Mathf.Pow(targetPos.x, 2) * middle.y - Mathf.Pow(middle.x, 2) * targetPos.y - (Mathf.Pow(targetPos.x, 2) - Mathf.Pow(middle.x, 2)) * orgPos.y;
        b /= (targetPos.x - middle.x) * (orgPos.x - targetPos.x) * (orgPos.x - middle.x);

        c = (targetPos.x * middle.y - middle.x * targetPos.y) * Mathf.Pow(orgPos.x, 2) - (Mathf.Pow(targetPos.x, 2) - Mathf.Pow(middle.x, 2)) * orgPos.x + (Mathf.Pow(targetPos.x, 2) * middle.x - targetPos.x * Mathf.Pow(middle.x, 2)) * orgPos.y;
        c /= (targetPos.x - middle.x) * (orgPos.x - targetPos.x) * (orgPos.x - middle.x);
        //c = height;

        moveObj.transform.position = orgPos;
        isInit = true;
    }

    private float Calculation(float x)
    {
        return a * Mathf.Pow(x, 2) + b * x + c;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            float x = moveObj.transform.position.x + (xSpeed /** Time.deltaTime*/);
            float y = Calculation(x);
            moveObj.transform.position = new Vector3(x, y, 0);
        }
    }
}
