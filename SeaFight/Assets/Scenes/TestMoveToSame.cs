using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveToSame : MonoBehaviour
{

    public Vector2 moveDirection;
    public Transform endPos;
    public GameObject[] objs;

    public float speed;

    public bool isMove = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isMove = !isMove;
        }
        if (isMove)
        {
            float step = speed * Time.deltaTime;
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].transform.localPosition = Vector3.MoveTowards(objs[i].transform.localPosition, endPos.localPosition, step + i * step);
            }
        }
    }
}
