using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public BulletTrriger all;
    public Vector3 MoveDirection;
    public bool isInit;
    private bool isHit;

    public int myIndex;
    [HideInInspector]
    public float Atk;
    public Tag myTag;
    public Tag targetTag;

    private Vector2 orgPos;
    private float destroyLength;
    /// <summary>
    /// 死亡限制长度
    /// </summary>
    public float deadLength;


    public void Init(Tag masterTag, float atk, Vector3 moveDir = new Vector3())
    {
        isHit = false;
        myTag = masterTag;
        Atk = atk;
        MoveDirection = moveDir;
        orgPos = transform.position;
        //
        isInit = true;
    }
    public void SetEulerAngle(Vector3 rotation, Vector3 spanRotate = new Vector3())
    {
        all.Rotation = rotation;
        all.RotateSpeed = spanRotate;
        all.Init();
    }



    private void PoolDestroy()
    {
        switch (targetTag)
        {
            case Tag.Enemy:
                GameManager.instance.SetParticle(PreLoadType.CardHurtParticle, transform.position, true);
                break;
            case Tag.Boat:
                GameManager.instance.SetParticle(PreLoadType.BoatHurtParticle, transform.position, true);
                break;
            case Tag.Player:
                GameManager.instance.SetParticle(PreLoadType.SelfHurtParticle, transform.position, true);
                break;
            default:
                Debug.LogError("射中->" + targetTag);
                //GameManager.instance.SetParticle(PreLoadType.CardHurtParticle, transform.localPosition);
                break;
        }
        Destroy(gameObject);
    }

    public void SelfDestroy()
    {
        GameManager.instance.SetParticle(PreLoadType.BulletDesParticle, transform.position, true);
        Destroy(gameObject);
    }

    public void TrrigerLogic(Collider coll)
    {
        if (isHit)
            return;

        string info = "";
        GameObject obj = coll.gameObject;
        switch (obj.tag)
        {
            case nameof(Tag.Enemy):
                if (myTag != Tag.Player)
                {
                    return;
                }
                else
                {
                    targetTag = Tag.Enemy;
                    isHit = true;
                    //info += obj.tag + "/" + obj.name;
                    CharacterInfo cInfo = obj.GetComponent<CharacterInfo>();
                    cInfo.BeShoot(Atk);
                }
                break;
            case nameof(Tag.Player):
                if (myTag != Tag.Player)
                {
                    targetTag = Tag.Player;
                    isHit = true;
                    //info += obj.tag + "/" + obj.name;
                    CharacterInfo cInfo = obj.GetComponent<CharacterInfo>();
                    cInfo.BeShoot(Atk);
                }
                else
                {
                    return;
                }
                break;
            case nameof(Tag.Support):
                if (myTag != Tag.Player)
                {
                    targetTag = Tag.Player;
                    isHit = true;
                    //info += obj.tag + "/" + obj.name;
                    SupportInfo sInfo = obj.GetComponent<SupportInfo>();
                    sInfo.BeShoot(Atk);
                }
                else
                {
                    return;
                }
                //info += myTag + "子弹射中Support";
                break;
            case nameof(Tag.Bullet):
                //info += " 子弹都能射中？";
                break;
            case nameof(Tag.Boat):
                if (myTag != Tag.Player)
                {
                    return;
                }
                else
                {
                    targetTag = Tag.Boat;
                    isHit = true;
                    BoatInfo bInfo = obj.GetComponent<BoatInfo>();
                    bInfo.BeShoot(Atk);
                }
                break;
            default:
                info += "未判断的Tag->" + obj.tag + "/" + obj.name;
                break;
        }
        if (info != "")
        {
            Debug.LogError(info);
        }
        if (isHit)
        {
            PoolDestroy();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!isInit)
            return;

        float time = Time.deltaTime;
        //orgSpeed += addSpeed * time;
        //transform.Translate(orgSpeed * Vector3.right, Space.Self);

        destroyLength = Vector2.Distance(transform.position, orgPos);
        if (destroyLength > deadLength)
        {
            isHit = true;
            SelfDestroy();
        }
        if (!isHit)
        {
            if (MoveDirection != Vector3.zero)
            {
                transform.Translate(MoveDirection * Time.deltaTime, Space.World);
            }
        }
    }
}

