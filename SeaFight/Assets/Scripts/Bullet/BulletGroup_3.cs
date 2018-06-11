using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup_3 : MonoBehaviour
{
    public float delaySpan;
    public float moveSpeed;
    public float[] angleSpan;

    #region 鱼雷

    public float delayYuLei;
    public float[] yuLeiAngle;
    public float radius;
    public float moveSpeed2;

    #endregion



    #region 需要设置的值--不然就会使用默认的
    public Transform parent;
    public CardInfo target;
    public CardInfo cardInfo;
    public float startAngle;
    public GameObject prefab;

    public GameObject prefab2;//鱼雷

    #endregion

    [ContextMenu("Init")]
    public void Init()
    {
        StartCoroutine(Init1());
        StartCoroutine(Init2());
    }

    public IEnumerator Init1()
    {
        transform.localEulerAngles = new Vector3(0, startAngle, 0);
        for (int i = 0; i < angleSpan.Length; i++)
        {
            yield return Yielders.WaitSecond(delaySpan);

            float tempAngle = (startAngle + angleSpan[i]);
            float x1 = 1 * Mathf.Cos(tempAngle * Mathf.PI / 180);
            float z1 = 1 * Mathf.Sin(tempAngle * Mathf.PI / 180);
            Vector3 moveDir = new Vector3(x1, 0, z1).normalized;

            if (parent != null && target != null && target.isLive)
            {
                GameObject obj = Common.Generate(prefab, transform);
                obj.transform.position = parent.transform.position;
                //obj.transform.eulerAngles = new Vector3(0, 0, tempAngle);
                BulletMove bMove = obj.GetComponent<BulletMove>();
                bMove.SetEulerAngle(new Vector3(0, 0, startAngle + angleSpan[i]));
                Tag myTag = Tag.Bullet;
                float atk = 0;
                if (cardInfo != null)
                {
                    myTag = cardInfo.myTag;
                    atk = cardInfo.Atk;
                }
                bMove.Init(myTag, atk, moveDir * moveSpeed);
            }
        }
    }

    public IEnumerator Init2()
    {
        yield return Yielders.WaitSecond(delayYuLei);
        for (int i = 0; i < yuLeiAngle.Length; i++)
        {
            float posAngle = startAngle + (i * 360 / yuLeiAngle.Length);
            float x1 = radius * Mathf.Cos(posAngle * Mathf.PI / 180);
            float z1 = radius * Mathf.Sin(posAngle * Mathf.PI / 180);
            Vector3 setPosition = new Vector3(x1, 0, z1);

            float lookAngle = startAngle + yuLeiAngle[i];
            float x2 = radius * Mathf.Cos(lookAngle * Mathf.PI / 180);
            float z2 = radius * Mathf.Sin(lookAngle * Mathf.PI / 180);
            Vector3 moveDir = new Vector3(x2, 0, z2).normalized;

            if (parent != null && target != null && target.isLive)
            {
                GameObject obj = Common.Generate(prefab2, transform);
                obj.transform.position = parent.transform.position;
                obj.transform.position += setPosition;
                //obj.transform.eulerAngles = new Vector3(0, 0, startAngle + yuLeiAngle[i]);
                BulletMove bMove = obj.GetComponent<BulletMove>();
                bMove.SetEulerAngle(new Vector3(0, 0, lookAngle));
                Tag myTag = Tag.Bullet;
                float atk = 0;
                if (cardInfo != null)
                {
                    myTag = cardInfo.myTag;
                    atk = cardInfo.Atk;
                }
                bMove.Init(myTag, atk, moveDir * moveSpeed2);
            }
        }
    }
}

