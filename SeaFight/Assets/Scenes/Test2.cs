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
    public GameObject middleObj;



    private Vector3 orgPos;
    private Vector3 targetPos;
    private Vector3 middlePos;
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
        middlePos = middleObj.transform.position;

        a = (targetPos.y -middlePos.y) * orgPos.x - (targetPos.x -middlePos.x) * orgPos.y + targetPos.x *middlePos.y -middlePos.x * targetPos.y;
        a = -a;
        a /= (targetPos.x -middlePos.x) * (orgPos.x - targetPos.x) * (orgPos.x -middlePos.x);

        b = (targetPos.y -middlePos.y) * Mathf.Pow(orgPos.x, 2) + Mathf.Pow(targetPos.x, 2) *middlePos.y - Mathf.Pow(middleObj.transform.position.x, 2) * targetPos.y - (Mathf.Pow(targetPos.x, 2) - Mathf.Pow(middleObj.transform.position.x, 2)) * orgPos.y;
        b /= (targetPos.x -middlePos.x) * (orgPos.x - targetPos.x) * (orgPos.x -middlePos.x);
        
        c = middlePos.y - a * Mathf.Pow(middlePos.x, 2) - b * middlePos.x;

        
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
