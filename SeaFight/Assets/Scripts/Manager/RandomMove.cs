using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomMove : MonoBehaviour
{
    /// <summary>
    /// 人物随机点以该点为中心点画圈范围
    /// </summary>
    public Vector3 center;
    /// <summary>
    /// 限制半径
    /// </summary>
    public float radius;
    /// <summary>
    /// 随机时间
    /// </summary>
    public Vector2 timeRandom;

    private float countTime;
    private float curMarkTime;
    public bool isMove;



    // Use this for initialization
    public void Init()
    {
        isMove = false;
    }

    public void SetMove()
    {
        countTime = 0;
        RandomLogic();
        isMove = true;
    }



    private void RandomLogic()
    {
        curMarkTime = Random.Range(timeRandom.x, timeRandom.y);

        float angle = Random.Range(0, 360);
        float x1 = radius * Mathf.Cos(angle * Mathf.PI / 180);
        float z1 = radius * Mathf.Sin(angle * Mathf.PI / 180);

        Vector3 endPoint = center + new Vector3(x1, 0, z1) * Random.Range(0f, 1f);
        transform.DOMove(endPoint, 2f).SetEase(Ease.Linear);
    }


    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if (countTime > curMarkTime)
            {
                countTime = 0;
                RandomLogic();
            }
            else
            {
                countTime += Time.deltaTime;
            }
        }
    }


}
