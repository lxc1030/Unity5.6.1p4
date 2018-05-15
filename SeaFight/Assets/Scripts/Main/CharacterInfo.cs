using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CharacterInfo : MonoBehaviour
{
    /// <summary>
    /// 平A CD时间
    /// </summary>
    public float shootCD;
    /// <summary>
    /// 射击点
    /// </summary>
    public Transform shootPoint;
    /// <summary>
    /// 人物编号
    /// </summary>
    public int myIndex;
    /// <summary>
    /// 平A目标
    /// </summary>
    public GameObject A_Target;
    /// <summary>
    /// 平A子弹个数
    /// </summary>
    public int bulletNum;


    public CardInfo cardInfo;

    public Transform AnimationObj;

    private SkeletonAnimation skeletonAnimation;   //gameobject的component。
    public SkeletonAnimation skAnimation
    {
        get { return skeletonAnimation; }
        set { skeletonAnimation = value; }
    }
    public Renderer render;
    private SkeletonUtility skeletonUtility;



    Spine.AnimationState spineAnimationState;
    private bool isEvent = false;
    public bool isEnemy;

    void Awake()
    {
        if (AnimationObj != null)
        {
            skeletonAnimation = AnimationObj.GetComponent<SkeletonAnimation>();
            render = AnimationObj.GetComponent<Renderer>();
            skeletonUtility = AnimationObj.GetComponent<SkeletonUtility>();
        }
        cardInfo = GetComponent<CardInfo>();
    }

    public virtual void Init(int index, bool _isEnemy)
    {
        myIndex = index;
        isEnemy = _isEnemy;
        if (!isEvent)
        {
            isEvent = true;
            if (skeletonAnimation != null)
            {
                spineAnimationState = skeletonAnimation.state;
                skeletonAnimation.state.Event += state_Event;
                skeletonAnimation.state.Complete += AnimationComplete;
            }
        }

        if (spineAnimationState != null)
        {
            //初始动画
            string animationName = "idle";
            spineAnimationState.SetAnimation(0, animationName, true);
        }

        cardInfo.SetInit(isEnemy, GameManager.BackCardHp(myIndex), GameManager.BackCardAtk(myIndex), "名称->" + myIndex);

    }




    public virtual void Shoot(GameObject target)
    {
        if (!isCanShoot)
            return;
        isCanShoot = false;
        AccumilatedTime = AccumilatedTime - shootCD;
        //
        A_Target = target;
        switch (target.tag)
        {
            case nameof(Tag.Enemy):
                break;
            case nameof(Tag.Boat):
                break;
        }

        string animationName = "atk";
        spineAnimationState.SetAnimation(0, animationName, false);
    }

    public virtual void state_Event(TrackEntry trackEntry, Spine.Event e)
    {

    }
    public virtual void AnimationComplete(TrackEntry trackEntry)
    {
        string curAni = trackEntry.ToString();
        string animationName = "idle";
        if (curAni != animationName)
        {
            spineAnimationState.SetAnimation(0, animationName, true);
        }
    }

    public virtual void BeShoot(float demage)
    {
        cardInfo.SetHp(demage);
        if (cardInfo.Hp <= 0)
        {
            Debug.LogError("编号->" + myIndex + " 死亡");
            render.material.DOFloat(1f, "_FillPhase", 0.1f).OnComplete(HitAnimationComplete);
        }
        else
        {
            render.material.DOFloat(0.5f, "_FillPhase", 0.1f).OnComplete(HitAnimationComplete);
        }
    }

    private void HitAnimationComplete()
    {
        render.material.SetFloat("_FillPhase", 0);
    }


    public void SetBullet(Vector3 pos, float angle)
    {
        GameObject obj = null;
        obj = PoolManager.instance.GetPoolObjByType(PreLoadType.Bullet, GameManager.instance.transBullet);
        obj.transform.localScale = Vector3.one;
        //
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.Init(myIndex, cardInfo.Atk);
        bullet.SetMove(pos, angle);
    }



    public bool isCanShoot;
    public float AccumilatedTime = 0f;
    // Update is called once per frame
    public virtual void Update()
    {
        GameManager.SetRenderQueueByType( render, transform.position.y);
        if (!isCanShoot)
        {
            AccumilatedTime = AccumilatedTime + Time.deltaTime;
            if (AccumilatedTime >= shootCD)
            {
                isCanShoot = true;
            }
        }
    }
    public void LateUpdate()
    {
        GameManager.SetZPosition(transform);
    }
}
