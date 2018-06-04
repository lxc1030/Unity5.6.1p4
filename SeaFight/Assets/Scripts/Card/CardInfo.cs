using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInfo : MonoBehaviour
{
    public Transform transPeopleInfo;
    public PeopleInfo pInfo;


    #region 需要设置的值--不然就会使用默认的
    public bool isEnemy;
    public Tag myTag;
    public float Atk;
    public float Hp;
    private float HpMax;
    public string myName;
    #endregion

    public void SetInit()
    {
        gameObject.tag = myTag.ToString();
        HpMax = Hp;
        GameObject obj = null;
        obj = PoolManager.instance.GetPoolObjByType(PreLoadType.PeopleInfo, Camera.main.transform);
        obj.SetActive(false);
        pInfo = obj.GetComponent<PeopleInfo>();
        pInfo.Init(transPeopleInfo, myName, GameManager.BackHpSpName(isEnemy));
    }

    public void UpdateHp(float demage)
    {
        Hp -= demage;
        float precent = Hp / HpMax;
        pInfo.SetHp(precent);
    }
    public static void SetBullet(Tag ownTag, int bulletType, int index, float atk, Vector3 pos, float angle)
    {
        GameObject obj = null;
        obj = Common.Generate(DataController.prefabPath_Bullet + bulletType, GameManager.instance.transBullet);
        BulletMove bullet = obj.GetComponent<BulletMove>();
        bullet.SetEulerAngle(new Vector3(0, 0, angle));
        //
        obj.transform.position = pos;
        obj.transform.localScale = Vector3.one;
        //
        float x1 = 1 * Mathf.Cos(angle * Mathf.PI / 180);
        float y1 = 1 * Mathf.Sin(angle * Mathf.PI / 180);
        Vector3 moveDir = new Vector3(x1, y1, 0).normalized * 30;
        bullet.Init(ownTag, atk, moveDir);
    }
}