using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup_4 : MonoBehaviour
{
    public float delaySpan;
    public float moveSpeed;
    public float[] angleSpan;



    #region 需要设置的值--不然就会使用默认的
    public CardInfo cardInfo;
    public float startAngle;
    public GameObject prefab;

    #endregion

    [ContextMenu("Init")]
    public void Init()
    {
        StartCoroutine(Init2());
    }

    public IEnumerator Init2()
    {
        for (int i = 0; i < angleSpan.Length; i++)
        {
            yield return Yielders.WaitSecond(delaySpan);

            float tempAngle = (startAngle + angleSpan[i]);
            float x1 = 1 * Mathf.Cos(tempAngle * Mathf.PI / 180);
            float z1 = 1 * Mathf.Sin(tempAngle * Mathf.PI / 180);
            Vector3 moveDir = new Vector3(x1, 0, z1).normalized;

            GameObject obj = Common.Generate(prefab, transform);
            obj.transform.localPosition = Vector3.zero;
            //obj.transform.eulerAngles = new Vector3(0, 0, tempAngle);
            BulletMove bMove = obj.GetComponent<BulletMove>();
            bMove.SetEulerAngle(new Vector3(0, 0, tempAngle));
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
