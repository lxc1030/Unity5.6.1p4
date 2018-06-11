using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup_5 : MonoBehaviour
{
    public float moveSpeed;
    public int count;
    public float angleSpan;

    #region 需要设置的值--不然就会使用默认的
    public Transform parent;
    public CardInfo target;
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
        yield return null;
        float startFix = (startAngle - 0.5f * (count - 1) * angleSpan);

        for (int i = 0; i < count; i++)
        {
            float tempAngle = (startFix + i * angleSpan);
            float x1 = 1 * Mathf.Cos(tempAngle * Mathf.PI / 180);
            float z1 = 1 * Mathf.Sin(tempAngle * Mathf.PI / 180);
            Vector3 moveDir = new Vector3(x1, 0, z1).normalized;

            if (parent != null && target != null && target.isLive)
            {
                GameObject obj = Common.Generate(prefab, transform);
                obj.transform.position = parent.transform.position;
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



}
