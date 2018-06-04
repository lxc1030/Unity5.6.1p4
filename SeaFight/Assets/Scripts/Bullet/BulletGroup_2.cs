using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup_2 : MonoBehaviour
{
    public float moveSpeed;
    public BGInfo2[] allInfos;

    #region 需要设置的值--不然就会使用默认的
    public CardInfo cardInfo;
    public Transform parent;
    public GameObject prefab;
    public float startAngle;

    #endregion

    [ContextMenu("Init")]
    public void Init()
    {
        StartCoroutine(Init(allInfos, moveSpeed));
    }

    public IEnumerator Init(BGInfo2[] infos, float moSpeed)
    {
        transform.localEulerAngles = new Vector3(0, startAngle, 0);
        for (int i = 0; i < infos.Length; i++)
        {
            yield return Yielders.WaitSecond(infos[i].delaySpan);

            for (int j = 0; j < infos[i].angle.Length; j++)
            {
                float tempAngle = (startAngle + infos[i].angle[j]);
                float x1 = 1 * Mathf.Cos(tempAngle * Mathf.PI / 180);
                float z1 = 1 * Mathf.Sin(tempAngle * Mathf.PI / 180);
                Vector3 moveDir = new Vector3(x1, 0, z1).normalized;

                GameObject obj = Common.Generate(prefab, transform);
                obj.transform.position = parent.position;
                //obj.transform.localEulerAngles = new Vector3(0, -infos[i].angle[j], 0);
                BulletMove bMove = obj.GetComponent<BulletMove>();
                bMove.SetEulerAngle(new Vector3(0, 0, -infos[i].angle[j]));
                Tag myTag = Tag.Bullet;
                float atk = 0;
                if (cardInfo != null)
                {
                    myTag = cardInfo.myTag;
                    atk = cardInfo.Atk;
                }
                bMove.Init(myTag, atk, moveDir * moSpeed);
            }

        }


    }



}

[System.Serializable]
public class BGInfo2
{
    public float delaySpan;
    public int[] angle;
}