using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletGroup_6 : MonoBehaviour
{
    public GameObject showPref;

    public int yuLeiCount;

    public float rangeX;

    public float startPosSpan;
    public float endPosSpan;
    public float shakeRange;
    public float shakeTimeSpan;

    public float startSize;
    public float sizeTime;

    private GameObject[] flys;

    public Vector3 fightPoint;
    public float fightRadius;

    public Vector3[] movePositions;


    #region 需要设置的值--不然就会使用默认的
    public CardInfo cardInfo;
    public int showNum;

    #endregion

    [ContextMenu("Init")]
    public void Init()
    {
        Common.Clear(transform);
        
        GameObject obj = null;
        flys = new GameObject[showNum];
        float startY = -((showNum - 1) * startPosSpan / 2);

        for (int i = 0; i < showNum; i++)
        {
            obj = Common.Generate(showPref, transform);
            obj.transform.localPosition = new Vector3(Random.Range(-rangeX, rangeX), startY + i * startPosSpan, 0);
            obj.transform.localScale = Vector3.one * startSize;
            obj.transform.localEulerAngles = new Vector3(0, 180, 0) * (cardInfo.myTag == Tag.Player ? 1 : -1);
            flys[i] = obj;
        }
        SizeAnimation();
        //
        SetSpan();
        //
        SetMove2();
        //

    }

    public void Init2()
    {
        //for (int i = 0; i < flys.Length; i++)
        //{
        //    float angle = Random.Range(0, 360);
        //    float x1 = rangeRadius * Mathf.Cos(angle * Mathf.PI / 180);
        //    float y1 = rangeRadius * Mathf.Sin(angle * Mathf.PI / 180);
        //    Vector3 endPoint = new Vector3(x1, y1, 0) * Random.Range(0f, 1f);
        //    endPoint.x = 0;
        //    flys[i].transform.DOLocalMove(endPoint, rangeTimeSpan).SetEase(Ease.Linear);
        //}

    }

    public void SizeAnimation()
    {
        for (int i = 0; i < flys.Length; i++)
        {
            flys[i].transform.DOScale(1, sizeTime).SetEase(Ease.Linear);
        }
    }
    /// <summary>
    /// 初始排布位置
    /// </summary>
    public void SetSpan()
    {
        for (int i = 0; i < flys.Length; i++)
        {
            float endY = -((float)(showNum - 1) * endPosSpan / 2);
            flys[i].transform.DOLocalMoveY(endY + (flys.Length - 1 - i) * endPosSpan, 2f).SetEase(Ease.Linear).OnComplete(ShakePos);
        }
    }

    private void ShakePos()
    {
        for (int i = 0; i < flys.Length; i++)
        {
            flys[i].transform.DOLocalMoveY(Random.Range(-shakeRange, shakeRange), shakeTimeSpan).SetEase(Ease.Linear);
        }
        Invoke("ShakePos", 0.1f);
    }


    /// <summary>
    /// 上升动画
    /// </summary>
    public void SetMove2()
    {
        Vector3 length = movePositions[0] - transform.position;
        transform.DOMove(length, 1).SetEase(Ease.Linear).SetRelative().OnComplete(SetMove3);
    }
    /// <summary>
    /// 平移动画
    /// </summary>
    public void SetMove3()
    {
        Vector3 length = movePositions[1] - movePositions[0];
        transform.DOMove(length, 3).SetEase(Ease.Linear).SetRelative().OnComplete(SetMove4);

        //投掷鱼雷
        StartCoroutine(YuLeiSpan());
    }

    public void SetMove4()
    {
        Vector3 length = movePositions[2] - movePositions[1];
        transform.DOMove(length, 2).SetEase(Ease.Linear).SetRelative();
    }

    public float yuStartDelay;
    public float yuSpanDelay;

    private IEnumerator YuLeiSpan()
    {
        yield return Yielders.WaitSecond(yuStartDelay);
        for (int i = 0; i < yuLeiCount; i++)
        {
            yield return Yielders.WaitSecond(yuSpanDelay);
            float angle = Random.Range(0, 360);
            float x1 = fightRadius * Mathf.Cos(angle * Mathf.PI / 180);
            float z1 = fightRadius * Mathf.Sin(angle * Mathf.PI / 180);
            Vector3 endPoint = fightPoint + new Vector3(x1, 0, z1) * Random.Range(0f, 1f);

            GameObject obj = Common.Generate(DataController.prefabPath_Bullet + 5, transform);
            BulletMove bmove = obj.GetComponent<BulletMove>();
            Collider myColl = obj.GetComponent<Collider>();
            //
            obj.transform.SetParent(transform.parent);
            obj.transform.position = endPoint + new Vector3(0, bmove.deadLength - 2, 0);
            obj.transform.localEulerAngles = new Vector3(0, 0, -90);
            //
            myColl.enabled = false;
            if (cardInfo != null)
            {
                bmove.Init(cardInfo.myTag, cardInfo.Atk, Vector3.down * 10);
            }
            else
            {
                bmove.Init(Tag.Bullet, 0, Vector3.down * 10);
            }

            obj.transform.DOMove(endPoint, 1.5f).SetEase(Ease.Linear).OnComplete(() => SetMove5(myColl, obj));
        }
    }

    private void SetMove5(Collider coll, GameObject obj)
    {
        coll.enabled = true;
        //obj.transform.DOMove(new Vector3(0, -1, 0), 0.1f).SetEase(Ease.Linear).SetRelative().OnComplete(() => SetMove6(obj));
    }

}
