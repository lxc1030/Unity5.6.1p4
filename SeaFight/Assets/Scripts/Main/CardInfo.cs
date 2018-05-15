using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public Transform transPeopleInfo;
    public PeopleInfo pInfo;

    public float Atk;
    public float Hp;
    private float HpMax;

    public void SetInit(bool isEnemy, float hp, float atk, string name)
    {
        HpMax = Hp = hp;
        Atk = atk;
        GameObject obj = null;
        obj = PoolManager.instance.GetPoolObjByType(PreLoadType.PeopleInfo, Camera.main.transform);
        obj.SetActive(false);
        pInfo = obj.GetComponent<PeopleInfo>();
        pInfo.Init(transPeopleInfo, name, GameManager.BackHpSpName(isEnemy));
    }

    public void SetHp(float demage)
    {
        Hp -= demage;
        float precent = Hp / HpMax;
        pInfo.SetHp(precent);
    }
}
