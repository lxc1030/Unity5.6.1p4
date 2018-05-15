using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D selfCollider;
    public int myIndex;
    public float Atk;

    public GameObject[] typeShow;
    private bool isHit;

    public Vector3 moveDirection;
    public float orgSpeed = 0;
    public float addSpeed = 0;

    public float moveLifeTime;

    public Tag targetTag;

    // Use this for initialization
    void Awake()
    {
        selfCollider = GetComponent<BoxCollider2D>();
    }
    public void Init(int index, float atk)
    {
        myIndex = index;
        Atk = atk;
        for (int i = 0; i < typeShow.Length; i++)
        {
            typeShow[i].SetActive(i == myIndex);
        }
        orgSpeed = BackOrgSpeed(myIndex);
        addSpeed = BackSpeedAdd(myIndex);
    }

    public void SetMove(Vector3 orgPos, float angle)
    {
        transform.position = orgPos;
        transform.eulerAngles = new Vector3(0, 0, angle);
        moveLifeTime = 0;
        isHit = false;
    }


    //public void SetTarget(Controller enemy, Transform transOrg)
    //{
    //    float width = 0;
    //    float height = 0;
    //    Vector3 add = Vector3.zero;
    //    isHit = true;
    //    gameObject.SetActive(false);
    //    //
    //    width = DataController.instance.bulletTargetRange.x;
    //    height = DataController.instance.bulletTargetRange.y;
    //    add = new Vector3(Random.Range(-width, width), Random.Range(-height, height), 0);
    //    Vector3 point = enemy.transBulletTarget.position/* + add*/;
    //    //
    //    float angle = Mathf.Atan2(point.y - transform.localPosition.y, point.x - transform.localPosition.x);
    //    angle = 180 / Mathf.PI * angle;
    //    transform.localEulerAngles = new Vector3(0, 0, angle);
    //    transform.DOMove(point, 0.5f).SetEase(Ease.Linear).OnPlay(() => SetHid()).OnComplete(PoolDestroy);
    //}

    private float BackOrgSpeed(int index)
    {
        float speed = 0;
        if (index == 1)
        {
            speed = 0.6f;
        }
        if (index == 2)
        {
            speed = 0.5f;
        }
        if (index == 3)
        {
            speed = 0.6f;
        }
        return speed;
    }

    private float BackSpeedAdd(int index)
    {
        float add = 0;
        if (index == 1)
        {
            add = 0.1f;
        }
        if (index == 1)
        {
            add = 0.09f;
        }
        if (index == 1)
        {
            add = 0.08f;
        }
        return add;
    }


    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;
        orgSpeed += addSpeed * time;
        moveLifeTime += time;
        transform.Translate(orgSpeed * Vector3.right, Space.Self);

        if (moveLifeTime > 1)
        {
            PoolDestroy();
        }

    }

    void OnTriggerEnter(Collider coll)
    {
        if (isHit)
            return;

        string info = "射中->";
        GameObject obj = coll.gameObject;
        switch (obj.tag)
        {
            case nameof(Tag.Enemy):
                targetTag = Tag.Enemy;
                isHit = true;
                info += obj.tag + "/" + obj.name;
                CharacterInfo cInfo = obj.GetComponent<CharacterInfo>();
                cInfo.BeShoot(Atk);
                break;
            case nameof(Tag.Player):
                return;
            case nameof(Tag.Bullet):
                info += " 子弹都能射中？";
                break;
            case nameof(Tag.Boat):
                targetTag = Tag.Boat;
                isHit = true;
                BoatInfo bInfo = obj.GetComponent<BoatInfo>();
                bInfo.BeShoot(Atk);
                break;
            default:
                info += "未判断的Tag->" + obj.tag + "/" + obj.name;
                Debug.LogError(info);
                break;
        }
        //Debug.LogError(info);
        if (isHit)
        {
            PoolDestroy();
        }
    }

    private void PoolDestroy()
    {
        switch (targetTag)
        {
            case Tag.Enemy:
                GameManager.instance.SetParticle(PreLoadType.CardHurtParticle, transform.localPosition);
                break;
            case Tag.Boat:
                GameManager.instance.SetParticle(PreLoadType.BoatHurtParticle, transform.localPosition);
                break;
        }
        PoolManager.instance.SetPoolObjByType(PreLoadType.Bullet, this.gameObject);
    }


}

public class BulletInfo
{
    public float orgSpeed;//初速度
    public float addSpeed;//加速度

}